import argparse
import json
import math
import pathlib

from blueprint_geometry import decode, dot, sub, unit, edges
from verify_geometry import DIRECTION_TOLERANCE, centroid, check, cross, verify_geometry


def derive(records):
    geometry = verify_geometry(records)
    pentagons = {i+1: tuple(f) for i, f in enumerate(geometry['pentagons'])}
    points = {i: unit(n['position']) for i, n in records['nodes'].items()}
    centers = {i: unit(centroid([points[n] for n in face])) for i, face in pentagons.items()}
    north = min(pentagons)
    up = centers[north]
    first = points[min(pentagons[north])]
    x = unit(sub(first, tuple(dot(first, up)*v for v in up)))
    z = cross(x, up)
    rotation = [x, up, z]
    rotated = {i: tuple(dot(p, axis) for axis in rotation) for i,p in points.items()}
    rotated_centers = {i: tuple(dot(p, axis) for axis in rotation) for i,p in centers.items()}
    south = min(centers, key=lambda i: dot(centers[i], up))
    upper = [i for i,p in rotated_centers.items() if p[1] > 0 and i != north]
    lower = [i for i,p in rotated_centers.items() if p[1] < 0 and i != south]
    check(len(upper) == len(lower) == 5, 'Polar ring sizes')
    check(math.dist(centers[south], tuple(-v for v in up)) <= DIRECTION_TOLERANCE, 'Opposite pole')
    for ring in (upper, lower):
        heights = [rotated_centers[i][1] for i in ring]
        check(max(heights)-min(heights) <= DIRECTION_TOLERANCE, 'Unequal ring latitude')
    check(abs(dot(x, up)) <= DIRECTION_TOLERANCE and abs(dot(cross(x, up), z)-1) <= DIRECTION_TOLERANCE, 'Rigid rotation')
    frame_ids = {tuple(sorted(f['ends'])): i for i,f in records['frames'].items()}
    node_pentagon = {n: i for i,face in pentagons.items() for n in face}
    links = {}
    for edge in frame_ids:
        a, b = (node_pentagon[n] for n in edge)
        if a != b:
            key = tuple(sorted((a, b)))
            check(key not in links, 'Repeated pentagon connection')
            links[key] = edge

    def ring_order(ring, first):
        order = sorted(ring, key=lambda i: math.atan2(rotated_centers[i][2], rotated_centers[i][0]))
        start = order.index(first)
        order = order[start:] + order[:start]
        check(all(e in links for e in edges(order)), 'Disconnected latitude ring')
        return order

    upper_order = ring_order(upper, min(upper))
    lower_start = min(i for i in lower if tuple(sorted((upper_order[-1], i))) in links)
    order = [north] + upper_order + ring_order(lower, lower_start) + [south]
    check(all(tuple(sorted(pair)) in links for pair in zip(order, order[1:])), 'Missing traversal spoke')
    painted_nodes, painted_frames, completed_nodes, closed_faces = set(), set(), set(), set()
    faces = sorted(geometry['pentagons'] + geometry['hexagons'])
    steps = []
    for index, pentagon in enumerate(order):
        face_nodes = set(pentagons[pentagon])
        completed_nodes.update(face_nodes)
        available_frames = {e for e in frame_ids if set(e) <= completed_nodes}
        leading = None
        next_endpoint = set()
        if index + 1 < len(order):
            leading = links[tuple(sorted((pentagon, order[index+1])))]
            next_endpoint = set(leading) - face_nodes
            available_frames.add(leading)
        available_nodes = completed_nodes | next_endpoint
        new_nodes = available_nodes - painted_nodes
        new_frames = available_frames - painted_frames
        newly_closed = {i+1 for i,f in enumerate(faces) if set(edges(f)) <= available_frames} - closed_faces
        check(painted_nodes <= available_nodes and painted_frames <= available_frames, 'Non-monotone patch')
        check(available_nodes - completed_nodes == next_endpoint, 'Unrequested future nodes')
        check({e for e in available_frames if not set(e) <= completed_nodes} == ({leading} if leading else set()), 'Unrequested future frames')
        visited, pending = set(), [min(available_nodes)]
        while pending:
            node = pending.pop()
            if node not in visited:
                visited.add(node)
                pending.extend(n for e in available_frames if node in e for n in e if n not in visited)
        check(visited == available_nodes, 'Disconnected patch')
        steps.append({'click': index+1, 'pentagon': pentagon, 'add_nodes': sorted(new_nodes),
                      'reuse_nodes': sorted(face_nodes & painted_nodes), 'add_frames': sorted(new_frames),
                      'reuse_frames': sorted(e for e in painted_frames if set(e) & face_nodes),
                      'leading': leading, 'close_faces': sorted(newly_closed),
                      'total_nodes': len(available_nodes), 'total_frames': len(available_frames)})
        painted_nodes, painted_frames = available_nodes, available_frames
        closed_faces |= newly_closed
    check(painted_nodes == set(points) and painted_frames == set(frame_ids) and len(closed_faces) == 32, 'Incomplete final union')
    return {'source': records['source'], 'rotation_rows': rotation, 'positions': rotated,
            'pentagons': pentagons, 'faces': faces, 'order': order, 'steps': steps,
            'center_latitudes': {i: math.degrees(math.asin(max(-1, min(1, p[1])))) for i,p in rotated_centers.items()},
            'maximum_node_latitude': max(math.degrees(math.asin(abs(p[1]))) for p in rotated.values())}


if __name__ == '__main__':
    parser = argparse.ArgumentParser()
    parser.add_argument('--output', type=pathlib.Path, required=True)
    args = parser.parse_args()
    records = decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8'))
    result = derive(records)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(json.dumps(result, indent=2)+'\n', encoding='utf-8')
    print('Pentagon order:', result['order'])
    for step in result['steps']:
        print(f"Click {step['click']}: P{step['pentagon']}, {step['total_nodes']} nodes, {step['total_frames']} frames, leading {step['leading']}")
