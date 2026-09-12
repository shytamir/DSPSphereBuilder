"""Inspect the two native blueprint exports from the focused owner session."""
import argparse
import base64
import gzip
import hashlib
import json
import math
import pathlib
import re
import struct
from blueprint_geometry import decode, edges, unit, read_layer
from derive_patches import derive
from verify_geometry import check


def decode_capture(text, layer_id=None):
    blueprint = re.search(r'DYBP:[^"\s]+"[A-Za-z0-9+/=]+"[0-9A-F]{32}', text).group()
    header, payload, signature = blueprint.split('"')
    kind = int(header.split(',')[3])
    if kind == 1:
        return decode(text)
    check(kind == 4 and layer_id is not None and layer_id > 0, 'Whole-sphere export requires an explicit positive layer ID')
    data = gzip.decompress(base64.b64decode(payload, validate=True))
    offset = 0

    def read(fmt):
        nonlocal offset
        value = struct.unpack_from('<' + fmt, data, offset)
        offset += struct.calcsize('<' + fmt)
        return value[0] if len(value) == 1 else value

    def orbit():
        version, identity, radius, x, y, z, w, enabled = read('iifffff?')
        check(version == 0, 'Unknown orbit record version')
        return identity, radius

    check(read('i') == 0, 'Unknown blueprint container version')
    read('ii')  # swarm render masks
    for _ in range(20): orbit()
    colors = read('i')
    check(colors >= 0, 'Negative orbit color count')
    for _ in range(colors): read('ffff')
    read('ii')  # layer render masks
    orbit_count = read('i')
    check(orbit_count >= 0, 'Negative layer orbit count')
    orbits = {}
    for _ in range(orbit_count):
        if read('?'):
            identity, radius = orbit()
            check(identity not in orbits, 'Duplicate layer orbit')
            orbits[identity] = radius
    layer_count = read('i')
    check(layer_count >= 0, 'Negative layer pool length')
    layers, inventory = {}, {}
    for identity in range(layer_count):
        if read('?'):
            start = offset
            layers[identity] = records = read_layer(read)
            check(identity in orbits, 'Layer has no orbit metadata')
            inventory[identity] = {'radius': orbits[identity],
                'counts': {kind: len(records[kind]) for kind in ('nodes', 'frames', 'shells')},
                'sha256': hashlib.sha256(data[start:offset]).hexdigest(),
                'decoded_graph_sha256': hashlib.sha256(json.dumps(records, sort_keys=True).encode('utf-8')).hexdigest()}
    check(offset == len(data), 'Unconsumed whole-sphere data')
    check(layer_id in layers, 'Requested layer is absent')
    records = layers[layer_id]
    records['source'] = {'sha256': hashlib.sha256(blueprint.encode('ascii')).hexdigest(),
        'payload_sha256': hashlib.sha256(data).hexdigest(), 'header': header,
        'signature': signature, 'bytes': len(data), 'selected_layer': layer_id,
        'radius': orbits[layer_id], 'layers': inventory}
    return records


def check_capture(records, plan, count, radius):
    check(math.isfinite(radius) and radius > 0, 'Invalid recorded radius')
    nodes = {n for step in plan['steps'][:count] for n in step['add_nodes']}
    frames = {tuple(e) for step in plan['steps'][:count] for e in step['add_frames']}
    check(len(records['nodes']) == len(nodes), 'Unexpected node count')
    mapping = {}
    max_direction = max_radius = 0.0
    for native_id, node in records['nodes'].items():
        position = node['position']
        check(all(math.isfinite(v) for v in position), 'Invalid node coordinate')
        length = math.sqrt(sum(v*v for v in position))
        check(length > 0, 'Zero node coordinate')
        matches = [n for n in nodes if math.dist(unit(position), unit(plan['positions'][n])) <= 4 / (16777216 - 1)]
        check(len(matches) == 1 and matches[0] not in mapping.values(), 'Ambiguous or displaced node')
        canonical = matches[0]
        mapping[native_id] = canonical
        max_direction = max(max_direction, math.dist(unit(position), unit(plan['positions'][canonical])))
        max_radius = max(max_radius, abs(length - radius) / radius)
    check(max_radius <= 10 / (16777216 - 10), 'Radius differs from the recorded layer')
    actual_edges = []
    for frame in records['frames'].values():
        check(not frame['euler'] and all(n in mapping for n in frame['ends']), 'Euler or dangling frame')
        actual_edges.append(tuple(sorted(mapping[n] for n in frame['ends'])))
    check(len(actual_edges) == len(frames) and set(actual_edges) == frames, 'Frame set differs')
    shell_faces = []
    for shell in records['shells'].values():
        check(all(n in mapping for n in shell['boundary']), 'Dangling shell')
        boundary = [mapping[n] for n in shell['boundary']]
        face_edges = frozenset(edges(boundary))
        check(len(set(boundary)) == len(boundary) and any(face_edges == frozenset(edges(face)) for face in plan['faces']), 'Non-reference shell')
        check(face_edges <= frames and face_edges not in shell_faces, 'Unclosed or duplicate shell')
        shell_faces.append(face_edges)
    return {'patches': count, 'nodes': len(nodes), 'frames': len(frames), 'shells': len(shell_faces),
            'shell_boundary_sizes': [len(s['boundary']) for s in records['shells'].values()],
            'max_direction_error': max_direction, 'max_relative_radius_error': max_radius}


def compare(before, after, radius):
    for records in (before, after):
        if 'radius' in records.get('source', {}):
            check(records['source']['radius'] == radius, 'Exported orbit radius differs from the log')
    plan = derive(decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8')))
    results = {'before': check_capture(before, plan, 3, radius), 'after': check_capture(after, plan, 12, radius)}
    check(len(before['shells']) == 1, 'Expected one manually designated reference shell')
    check(before['shells'] == after['shells'], 'Shell designation or boundary changed')
    for kind, fields in (('nodes', ('id', 'prototype', 'position')), ('frames', ('id', 'prototype', 'ends', 'euler'))):
        for identity, old in before[kind].items():
            new = after[kind].get(identity)
            check(new is not None and all(old[field] == new[field] for field in fields), f'Existing {kind} record changed: {identity}')
    return results


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--before', type=pathlib.Path, required=True)
    parser.add_argument('--after', type=pathlib.Path, required=True)
    parser.add_argument('--radius', type=float, required=True)
    parser.add_argument('--layer', type=int, help='Required layer ID for whole-sphere exports')
    args = parser.parse_args()
    before, after = (decode_capture(path.read_text(encoding='utf-8-sig'), args.layer) for path in (args.before, args.after))
    result = compare(before, after, args.radius)
    result['captures'] = {'before': before['source'], 'after': after['source']}
    print(json.dumps(result, indent=2))
