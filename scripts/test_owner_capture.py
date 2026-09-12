import copy
import base64
import gzip
import pathlib
import struct
import unittest
from blueprint_geometry import decode, edges
from check_owner_capture import compare, decode_capture
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

    def test_retained_hexagon(self):
        reference = decode(pathlib.Path('research/cosmin1490/60.txt').read_text())
        frames = {tuple(sorted(f['ends'])) for f in self.before['frames'].values()}
        identity, shell = next((i, s) for i, s in reference['shells'].items() if len(s['boundary']) == 6 and set(edges(s['boundary'])) <= frames)
        before, after = copy.deepcopy(self.before), copy.deepcopy(self.after)
        before['shells'] = after['shells'] = {identity: shell}
        result = compare(before, after, 9700)
        self.assertEqual(result['after']['shell_boundary_sizes'], [6])

    def test_whole_sphere_layer_selection_and_bounds(self):
        reference = pathlib.Path('research/cosmin1490/60.txt').read_text().strip()
        header, payload, signature = reference.split('"')
        layer = gzip.decompress(base64.b64decode(payload))[4:]
        orbit = struct.pack('<iifffff?', 0, 1, 36000, 0, 0, 0, 1, True)
        data = struct.pack('<iii', 0, 0, 0) + orbit * 20 + struct.pack('<iiii', 0, 0, 0, 1)
        data += b'\x01' + orbit + struct.pack('<i', 2) + b'\x00\x01' + layer
        fields = header.split(','); fields[3] = '4'
        def encoded(content):
            return '"'.join((','.join(fields), base64.b64encode(gzip.compress(content)).decode('ascii'), signature))
        selected = decode_capture(encoded(data), 1)
        original = decode(reference)
        for kind in ('nodes', 'frames', 'shells'):
            self.assertEqual(selected[kind], original[kind])
        self.assertEqual(selected['source']['radius'], 36000)
        with self.assertRaises(ValueError): decode_capture(encoded(data))
        with self.assertRaises(ValueError): decode_capture(encoded(data), 2)
        with self.assertRaises(ValueError): decode_capture(encoded(data + b'\x00'), 1)
        with self.assertRaises(struct.error): decode_capture(encoded(data[:-1]), 1)

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
