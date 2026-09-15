"""Compare the compiled production plan with the independent reference derivation."""
import json
import math
import pathlib
import struct
import sys
from blueprint_geometry import decode, unit
from derive_patches import derive

actual = json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8'))
records = decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8'))
expected = derive(records, grid_aligned=True)
legacy = derive(records)
native = json.loads(pathlib.Path('checks/fixtures/native-grid-centers.json').read_text(encoding='utf-8'))
published = json.loads(pathlib.Path('checks/fixtures/published-directions.json').read_text(encoding='utf-8'))
assert actual['reference'] == expected['source']['sha256']
assert len(actual['patches']) == 12
assert actual['faces'] == [list(face) for face in expected['faces']]
for patch, step in zip(actual['patches'], expected['steps']):
    assert patch['nodes'] == step['add_nodes']
    assert {tuple(e) for e in patch['frames']} == set(step['add_frames'])
assert expected['steps'] == legacy['steps']

direction_bound = 4 / (16777216 - 1)
radius_bound = 10 / (16777216 - 10)
max_direction = max_radius = max_center = 0.0
plans = {p['orientation']: p for p in actual['plans']}
assert len(plans) == len(actual['plans']) == 2
for orientation, reference in (('Grid', expected), ('Legacy', legacy)):
    compiled = plans[orientation]
    assert len(compiled['directions']) == 60
    for canonical_id, direction in enumerate(compiled['directions'], 1):
        assert math.dist(unit(direction), reference['positions'][canonical_id]) <= direction_bound
        if orientation == 'Legacy':
            # Compare emitted float bits with the published table, independently of derivation.
            assert struct.pack('<3f', *direction) == struct.pack('<3f', *published['directions'][canonical_id - 1])
    for sample in compiled['scales']:
        radius = sample['radius']
        assert len(sample['positions']) == 60
        for canonical_id, position in enumerate(sample['positions'], 1):
            assert all(math.isfinite(v) for v in position)
            direction_error = math.dist(unit(position), unit(reference['positions'][canonical_id]))
            radius_error = abs(math.sqrt(sum(v*v for v in position)) - radius) / radius
            assert direction_error <= direction_bound, (canonical_id, direction_error)
            assert radius_error <= radius_bound, (canonical_id, radius_error)
            max_direction, max_radius = max(max_direction, direction_error), max(max_radius, radius_error)
        if orientation == 'Grid':
            matched = set()
            for face in reference['pentagons'].values():
                center = unit(tuple(sum(unit(sample['positions'][n - 1])[axis] for n in face) / 5 for axis in range(3)))
                errors = [math.dist(center, unit(p)) for p in native['centers']]
                nearest = min(range(len(errors)), key=errors.__getitem__)
                assert errors[nearest] <= direction_bound and nearest not in matched
                matched.add(nearest)
                max_center = max(max_center, errors[nearest])
            assert len(matched) == 12

new_directions = [unit(p) for p in plans['Grid']['directions']]
old_directions = [unit(p) for p in plans['Legacy']['directions']]
for a, b in zip(new_directions, old_directions):
    assert abs(a[1] - b[1]) <= direction_bound
for frame in records['frames'].values():
    a, b = (n - 1 for n in frame['ends'])
    assert abs(math.dist(new_directions[a], new_directions[b]) - math.dist(old_directions[a], old_directions[b])) <= 2 * direction_bound
print(f'PASS: both orientations, published float bits, 12 deltas, 32 faces, 60/90 topology and preserved geometry; '
      f'max direction error {max_direction:.9g}; max relative radius error {max_radius:.9g}; '
      f'max native center error {max_center:.9g}.')
