"""Inspect the two native single-layer exports from the focused owner session."""
import argparse
import json
import math
import pathlib
from blueprint_geometry import decode, edges, unit
from derive_patches import derive
from verify_geometry import check


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
            'max_direction_error': max_direction, 'max_relative_radius_error': max_radius}


def compare(before, after, radius):
    plan = derive(decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8')))
    results = {'before': check_capture(before, plan, 3, radius), 'after': check_capture(after, plan, 12, radius)}
    check(len(before['shells']) == 1 and len(next(iter(before['shells'].values()))['boundary']) == 5, 'Expected one manually designated pentagon')
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
    args = parser.parse_args()
    before, after = (decode(path.read_text(encoding='utf-8-sig')) for path in (args.before, args.after))
    result = compare(before, after, args.radius)
    result['captures'] = {'before': before['source'], 'after': after['source']}
    print(json.dumps(result, indent=2))
