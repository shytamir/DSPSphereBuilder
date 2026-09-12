import collections
import json
import math
import pathlib

import argparse

from blueprint_geometry import decode, unit, dot, sub, edges

ROUNDING_UNIT = 2**-24
DIRECTION_TOLERANCE = 4 * ROUNDING_UNIT / (1 - ROUNDING_UNIT)


def check(condition, detail):
    if not condition:
        raise ValueError(detail)


def cross(a, b):
    return (a[1]*b[2]-a[2]*b[1], a[2]*b[0]-a[0]*b[2], a[0]*b[1]-a[1]*b[0])


def centroid(vertices):
    return tuple(sum(v[k] for v in vertices) / len(vertices) for k in range(3))


def canonical_cycle(cycle):
    variants = []
    for order in (cycle, list(reversed(cycle))):
        variants.extend(tuple(order[i:] + order[:i]) for i in range(len(order)))
    return min(variants)


def verify_geometry(records):
    points = {i: unit(n['position']) for i, n in records['nodes'].items()}
    frame_list = [tuple(sorted(f['ends'])) for f in records['frames'].values()]
    frames = set(frame_list)
    check(len(points) == 60 and len(frames) == len(frame_list) == 90, 'Node/frame counts or duplicate frames')
    check(all(a in points and b in points and a != b for a, b in frames), 'Invalid endpoint')
    check(not any(f['euler'] for f in records['frames'].values()), 'Non-geodesic frame')
    adjacency = {i: set() for i in points}
    for a, b in frames:
        adjacency[a].add(b)
        adjacency[b].add(a)
    check(all(len(a) == 3 for a in adjacency.values()), 'Vertex degree')
    visited, pending = set(), [min(points)]
    while pending:
        i = pending.pop()
        if i not in visited:
            visited.add(i)
            pending.extend(adjacency[i] - visited)
    check(visited == set(points), 'Disconnected framework')
    faces = sorted(canonical_cycle(s['boundary']) for s in records['shells'].values())
    check(len(set(faces)) == 32, 'Duplicate/missing face')
    check(collections.Counter(map(len, faces)) == {5: 12, 6: 20}, 'Face counts')
    check(all(len(set(f)) == len(f) and set(f) <= points.keys() for f in faces), 'Invalid face')
    incidence = collections.Counter(e for f in faces for e in edges(f))
    check(set(incidence) == frames and set(incidence.values()) == {2}, 'Face/frame incidence')
    pentagons = [f for f in faces if len(f) == 5]
    check(collections.Counter(i for f in pentagons for i in f) == {i: 1 for i in points}, 'Pentagon partition')
    perimeter = {e for f in pentagons for e in edges(f)}
    spokes = frames - perimeter
    chord = lambda e: math.dist(points[e[0]], points[e[1]])
    long_lengths, short_lengths = list(map(chord, perimeter)), list(map(chord, spokes))
    check(len(perimeter) == 60 and len(spokes) == 30, 'Edge classes')
    for lengths in (long_lengths, short_lengths):
        check(max(lengths)-min(lengths) <= 2*DIRECTION_TOLERANCE, 'Unequal edge class')
    check(max(short_lengths) < min(long_lengths), 'Expected shorter spokes')
    planarity = 0.0
    for f in faces:
        v = [points[i] for i in f]
        center = unit(centroid(v))
        plane_distances = [dot(p, center) for p in v]
        deviation = max(plane_distances) - min(plane_distances)
        planarity = max(planarity, deviation)
        check(deviation <= 2*DIRECTION_TOLERANCE, ('Face not planar', f))
        # An outward supporting plane excludes folded or overlapping face records.
        check(all(dot(p, center) <= max(plane_distances) + DIRECTION_TOLERANCE for p in points.values()), ('Non-convex face', f))
        if len(f) == 6:
            classes = [e in perimeter for e in edges(f)]
            check(all(classes[i] != classes[(i+1) % 6] for i in range(6)), ('Non-alternating hexagon', f))
    return {'pentagons': pentagons, 'hexagons': [f for f in faces if len(f) == 6],
            'perimeter_chord_range': [min(long_lengths), max(long_lengths)],
            'spoke_chord_range': [min(short_lengths), max(short_lengths)],
            'maximum_face_plane_spread': planarity, 'direction_tolerance': DIRECTION_TOLERANCE}


def compare(reference, candidate):
    rp = {i: unit(n['position']) for i,n in reference['nodes'].items()}
    cp = {i: unit(n['position']) for i,n in candidate['nodes'].items()}
    mapping = {}
    errors = []
    for i, p in cp.items():
        distance, target = min((math.dist(p, q), j) for j,q in rp.items())
        check(distance <= DIRECTION_TOLERANCE, ('Position differs', i, distance))
        mapping[i] = target
        errors.append(distance)
    check(len(set(mapping.values())) == len(cp), 'Ambiguous node mapping')
    reference_edges = {tuple(sorted(f['ends'])) for f in reference['frames'].values()}
    check(all(tuple(sorted(mapping[i] for i in f['ends'])) in reference_edges for f in candidate['frames'].values()), 'Non-reference connection')
    reference_faces = {canonical_cycle(s['boundary']) for s in reference['shells'].values()}
    check(all(canonical_cycle([mapping[i] for i in s['boundary']]) in reference_faces for s in candidate['shells'].values()), 'Non-reference face')
    return {'node_mapping': mapping, 'maximum_direction_difference': max(errors)}


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--published', type=pathlib.Path)
    parser.add_argument('--output', type=pathlib.Path, required=True)
    args = parser.parse_args()
    repository = decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8'))
    sample = decode(pathlib.Path('CONCEPT.md').read_text(encoding='utf-8'))
    result = {'source': repository['source'], 'geometry': verify_geometry(repository),
              'sample_comparison': compare(repository, sample)}
    if args.published:
        published = decode(args.published.read_text(encoding='utf-8'))
        result['published_source'] = published['source']
        result['published_geometry'] = verify_geometry(published)
        result['published_comparison'] = compare(published, repository)
        result['published_sample_comparison'] = compare(published, sample)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(result, indent=2)+'\n', encoding='utf-8')
    print(f'Verified 60 nodes, 90 frames, and 32 convex face boundaries; details: {args.output}')
