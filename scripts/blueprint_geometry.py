import argparse
import base64
import collections
import gzip
import hashlib
import json
import math
import pathlib
import re
import struct


def decode(text):
    blueprint = re.search(r'DYBP:[^"\s]+"[A-Za-z0-9+/=]+"[0-9A-F]{32}', text).group()
    header, payload, signature = blueprint.split('"')
    if header.split(',')[3] != '1':
        raise ValueError('Expected single-layer blueprint')
    data = gzip.decompress(base64.b64decode(payload, validate=True))
    offset = 0

    def read(fmt):
        nonlocal offset
        result = struct.unpack_from('<' + fmt, data, offset)
        offset += struct.calcsize('<' + fmt)
        return result[0] if len(result) == 1 else result

    format_version, layer_version = read('ii')
    if format_version != 0 or layer_version not in (0, 1):
        raise ValueError((format_version, layer_version))
    records = {}
    for kind in ('nodes', 'frames', 'shells'):
        capacity, cursor, recycled = read('iii')
        pool = {}
        for slot in range(1, cursor):
            present = read('i')
            if not present:
                continue
            version, identity, prototype = read('iii')
            if present != slot or identity != slot:
                raise ValueError((kind, slot, present, identity))
            item = {'id': identity, 'prototype': prototype, 'version': version}
            if kind == 'nodes':
                if not 0 <= version <= 5:
                    raise ValueError(version)
                item['use'], item['reserved'] = read('??')
                item['position'] = read('fff')
                read('i')  # maximum structure points
                if version >= 2:
                    read('i')  # render index
                read('i')  # frame turn
                if version >= 1:
                    read('i')  # shell turn
                read('i')  # requested structure points
                if version >= 4:
                    read('i')  # requested cell points
                if version >= 5:
                    read('4B')
            elif kind == 'frames':
                if version not in (0, 1):
                    raise ValueError(version)
                item['reserved'] = read('?')
                item['ends'] = read('ii')
                item['euler'] = read('?')
                read('i')
                if version >= 1:
                    read('4B')
            else:
                if version not in (0, 1, 2):
                    raise ValueError(version)
                read('i')  # random seed
                if version >= 2:
                    read('4B')
                item['boundary'] = [read('i') for _ in range(read('i'))]
            pool[identity] = item
        for _ in range(recycled):
            if read('i') in pool:
                raise ValueError('Occupied recycled slot')
        if cursor > capacity:
            raise ValueError('Pool capacity exceeded')
        records[kind] = pool
    if layer_version >= 1:
        read('i')
        if read('?'):
            read(str(4 * read('i')) + 'B')
    if offset != len(data):
        raise ValueError(('Unconsumed data', offset, len(data)))
    records['source'] = {'sha256': hashlib.sha256(blueprint.encode('ascii')).hexdigest(),
                         'payload_sha256': hashlib.sha256(data).hexdigest(),
                         'header': header, 'signature': signature, 'bytes': len(data)}
    return records


def unit(v):
    length = math.sqrt(sum(x*x for x in v))
    return tuple(x / length for x in v)


def dot(a, b):
    return sum(x*y for x, y in zip(a, b))


def sub(a, b):
    return tuple(x-y for x, y in zip(a, b))


def edges(cycle):
    return [tuple(sorted((a, b))) for a, b in zip(cycle, cycle[1:] + cycle[:1])]


def inspect(records):
    points = {i: unit(n['position']) for i, n in records['nodes'].items()}
    frames = {tuple(sorted(f['ends'])): i for i, f in records['frames'].items()}
    adjacency = collections.defaultdict(set)
    for a, b in frames:
        adjacency[a].add(b)
        adjacency[b].add(a)
    lengths = {e: math.dist(points[e[0]], points[e[1]]) for e in frames}
    faces = [s['boundary'] for s in records['shells'].values()]
    incidences = collections.Counter(e for face in faces for e in edges(face))
    result = {'source': records['source'], 'counts': {k: len(records[k]) for k in ('nodes', 'frames', 'shells')},
              'degrees': dict(collections.Counter(map(len, adjacency.values()))),
              'face_sizes': dict(collections.Counter(map(len, faces))),
              'edge_incidence': dict(collections.Counter(incidences.values())),
              'all_boundaries_have_frames': set(incidences) == set(frames),
              'prototypes': {k: sorted({x['prototype'] for x in records[k].values()}) for k in ('nodes', 'frames', 'shells')},
              'euler_frames': sum(f['euler'] for f in records['frames'].values()),
              'edge_chords': sorted(lengths.values()),
              'raw_norms': sorted({math.sqrt(dot(n['position'], n['position'])) for n in records['nodes'].values()})}
    return result


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('input', type=pathlib.Path)
    parser.add_argument('--output', type=pathlib.Path, required=True)
    args = parser.parse_args()
    records = decode(args.input.read_text(encoding='utf-8'))
    args.output.write_text(json.dumps({'records': records, 'measurements': inspect(records)}, indent=2) + '\n', encoding='utf-8')
    summary = inspect(records)
    for key in ('edge_chords', 'raw_norms'):
        summary[key] = [min(summary[key]), max(summary[key])]
    print(json.dumps(summary, indent=2))
