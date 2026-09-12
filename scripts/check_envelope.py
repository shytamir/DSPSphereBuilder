import argparse
import itertools
import json
import math
import pathlib
import struct

from blueprint_geometry import decode, dot, sub, unit, edges
from derive_patches import derive
from verify_geometry import centroid, check, cross


def distance_squared(a, b):
    difference = sub(a, b)
    return dot(difference, difference)


def point_segment_squared(point, start, end):
    direction = sub(end, start)
    t = max(0, min(1, dot(sub(point, start), direction) / dot(direction, direction)))
    projected = unit(tuple(a + t*d for a,d in zip(start, direction)))
    return distance_squared(point, projected)


def inside_face(point, boundary, points):
    center = unit(centroid([points[i] for i in boundary]))
    for a,b in edges(boundary):
        normal = cross(points[a], points[b])
        if dot(point, normal) * dot(center, normal) <= 0:
            return False
    return True


def evaluate(plan, points):
    frames = [tuple(e) for step in plan['steps'] for e in step['add_frames']]
    faces = plan['faces']
    node_spacing = min(distance_squared(points[a], points[b]) for a,b in itertools.combinations(points, 2))
    node_frame_spacing = min(point_segment_squared(p, points[a], points[b]) for i,p in points.items() for a,b in frames if i not in (a,b))
    maximum_frame_chord = max(math.dist(points[a], points[b]) for a,b in frames)
    check(node_spacing >= 0.0051122503, 'Node spacing')
    check(node_frame_spacing >= 0.0027562499, 'Node/frame spacing')
    check(maximum_frame_chord <= 0.518, 'Frame length')
    crossing_candidates = 0
    for first, second in itertools.combinations(frames, 2):
        if set(first) & set(second):
            continue
        a,b,c,d = (points[i] for i in first + second)
        if min(distance_squared(x,y) for x in (a,b) for y in (c,d)) >= 0.13443033:
            continue
        crossing_candidates += 1
        first_normal, second_normal = unit(cross(a,b)), unit(cross(c,d))
        check(not (dot(a,second_normal)*dot(b,second_normal) < -1e-9 and dot(c,first_normal)*dot(d,first_normal) < -1e-9), ('Crossing', first, second))
    face_distances = {5: [], 6: []}
    for boundary in faces:
        center = unit(centroid([points[i] for i in boundary]))
        distances = [distance_squared(center, points[i]) for i in boundary]
        face_distances[len(boundary)].extend(distances)
        check(max(distances) <= 0.26832402 * 0.6, ('Shell cycle too large', boundary))
        candidates = {i for i,p in points.items() if distance_squared(center,p) < 0.16099441}
        check(candidates == set(boundary), ('Shell center node candidates', boundary))
        check(not any(inside_face(p, boundary, points) for i,p in points.items() if i not in boundary), ('Interior node', boundary))
    closed_faces = set()
    for step in plan['steps']:
        for face_id in closed_faces:
            boundary = faces[face_id-1]
            check(not any(inside_face(points[i], boundary, points) for i in step['add_nodes']), ('New node in existing face', step['click'], face_id))
            check(not any(inside_face(unit(centroid([points[a],points[b]])), boundary, points) for a,b in step['add_frames']), ('New frame midpoint in existing face', step['click'], face_id))
        closed_faces.update(step['close_faces'])
    node_latitudes = [math.degrees(math.asin(max(-1, min(1, abs(p[1]))))) for p in points.values()]
    return {'minimum_node_distance_squared': node_spacing,
            'minimum_nonincident_node_frame_distance_squared': node_frame_spacing,
            'maximum_frame_chord': maximum_frame_chord, 'nonincident_crossing_candidates': crossing_candidates,
            'maximum_face_center_distance_squared': {size: max(values) for size,values in face_distances.items()},
            'maximum_node_latitude': max(node_latitudes), 'required_rounded_latitude': max(round(v) for v in node_latitudes)}


def f32(value):
    return struct.unpack('<f', struct.pack('<f', value))[0]


def scaled_directions(points, radius):
    check(math.isfinite(radius) and radius > 0, 'Positive finite radius required')
    result = {}
    for i,p in points.items():
        raw = tuple(f32(v*radius) for v in p)
        magnitude = f32(math.sqrt(f32(f32(f32(raw[0]*raw[0]) + f32(raw[1]*raw[1])) + f32(raw[2]*raw[2]))))
        check(math.isfinite(magnitude) and magnitude > 1e-5, 'Coordinate magnitude')
        result[i] = tuple(f32(v/magnitude) for v in raw)
    return result


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--output', type=pathlib.Path, required=True)
    parser.add_argument('--radii', type=float, nargs='+', default=[2400, 10000, 1000000])
    args = parser.parse_args()
    plan = derive(decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8')))
    result = {'unit_geometry': evaluate(plan, plan['positions']), 'float32_scale_checks': {r: evaluate(plan, scaled_directions(plan['positions'], r)) for r in args.radii}}
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(result, indent=2)+'\n', encoding='utf-8')
    print(json.dumps(result, indent=2))
