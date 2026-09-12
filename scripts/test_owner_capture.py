import copy
import pathlib
import struct
import unittest
from blueprint_geometry import decode, edges
from check_owner_capture import compare
from derive_patches import derive


class CaptureChecks(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        source = decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8'))
        plan = derive(source)
        radius = 9700
        full = copy.deepcopy(source)
        for n, node in full['nodes'].items():
            node['position'] = tuple(struct.unpack('<f', struct.pack('<f', v * radius))[0] for v in plan['positions'][n])
        nodes = {n for step in plan['steps'][:3] for n in step['add_nodes']}
        frames = {tuple(e) for step in plan['steps'][:3] for e in step['add_frames']}
        shell_id, shell = next((i, s) for i, s in full['shells'].items() if len(s['boundary']) == 5 and set(edges(s['boundary'])) <= frames)
        full['shells'] = {shell_id: shell}
        prefix = copy.deepcopy(full)
        prefix['nodes'] = {i: n for i, n in prefix['nodes'].items() if i in nodes}
        prefix['frames'] = {i: f for i, f in prefix['frames'].items() if tuple(sorted(f['ends'])) in frames}
        cls.before, cls.after = prefix, full

    def test_valid_captures(self):
        result = compare(self.before, self.after, 9700)
        self.assertEqual((result['after']['nodes'], result['after']['frames']), (60, 90))

    def test_missing_final_frame(self):
        altered = copy.deepcopy(self.after)
        altered['frames'].pop(next(iter(altered['frames'])))
        with self.assertRaises(ValueError): compare(self.before, altered, 9700)

    def test_wrong_recorded_radius(self):
        with self.assertRaises(ValueError): compare(self.before, self.after, 9800)

    def test_lost_manual_shell(self):
        altered = copy.deepcopy(self.after); altered['shells'] = {}
        with self.assertRaises(ValueError): compare(self.before, altered, 9700)

    def test_changed_old_coordinate_inside_tolerance(self):
        altered = copy.deepcopy(self.after)
        identity = next(iter(self.before['nodes']))
        old = altered['nodes'][identity]['position']
        altered['nodes'][identity]['position'] = (old[0] + 1e-7, old[1], old[2])
        with self.assertRaises(ValueError): compare(self.before, altered, 9700)


if __name__ == '__main__': unittest.main()
