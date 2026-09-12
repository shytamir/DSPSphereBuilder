"""Compare the compiled production plan with the independent reference derivation."""
import json
import math
import pathlib
import sys
from blueprint_geometry import decode, unit
from derive_patches import derive

actual = json.loads(pathlib.Path(sys.argv[1]).read_text(encoding='utf-8'))
expected = derive(decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8')))
assert actual['reference'] == expected['source']['sha256']
assert len(actual['directions']) == 60 and len(actual['patches']) == 12
assert actual['faces'] == [list(face) for face in expected['faces']]
for patch, step in zip(actual['patches'], expected['steps']):
    assert patch['nodes'] == step['add_nodes']
    assert {tuple(e) for e in patch['frames']} == set(step['add_frames'])

direction_bound = 4 / (16777216 - 1)
radius_bound = 10 / (16777216 - 10)
max_direction = max_radius = 0.0
for sample in actual['scales']:
    radius = sample['radius']
    assert len(sample['positions']) == 60
    for canonical_id, position in enumerate(sample['positions'], 1):
        assert all(math.isfinite(v) for v in position)
        direction_error = math.dist(unit(position), unit(expected['positions'][canonical_id]))
        radius_error = abs(math.sqrt(sum(v*v for v in position)) - radius) / radius
        assert direction_error <= direction_bound, (canonical_id, direction_error)
        assert radius_error <= radius_bound, (canonical_id, radius_error)
        max_direction, max_radius = max(max_direction, direction_error), max(max_radius, radius_error)
print(f'PASS: compiled plan matches all 12 deltas and 32 faces; final 60/90; max direction error {max_direction:.9g}; max relative radius error {max_radius:.9g}.')
