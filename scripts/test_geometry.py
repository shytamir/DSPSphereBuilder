import copy
import pathlib
import unittest

from blueprint_geometry import decode
from verify_geometry import compare, verify_geometry


class GeometryChecks(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.reference = decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8'))

    def test_missing_frame_is_rejected(self):
        altered = copy.deepcopy(self.reference)
        altered['frames'].pop(next(iter(altered['frames'])))
        with self.assertRaises(ValueError):
            verify_geometry(altered)

    def test_displaced_node_is_rejected(self):
        altered = copy.deepcopy(self.reference)
        node = altered['nodes'][1]
        x, y, z = node['position']
        node['position'] = (x + 0.001, y, z)
        with self.assertRaises(ValueError):
            compare(self.reference, altered)

    def test_radial_scaling_preserves_geometry(self):
        altered = copy.deepcopy(self.reference)
        for node in altered['nodes'].values():
            node['position'] = tuple(x * 8192 for x in node['position'])
        verify_geometry(altered)
        compare(self.reference, altered)

    def test_non_boundary_cycle_is_rejected(self):
        altered = copy.deepcopy(self.reference)
        face = altered['shells'][next(iter(altered['shells']))]['boundary']
        face[1], face[2] = face[2], face[1]
        with self.assertRaises(ValueError):
            verify_geometry(altered)

    def test_truncated_payload_is_rejected(self):
        import base64
        import gzip
        text = pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8').strip()
        header, payload, signature = text.split('"')
        data = gzip.decompress(base64.b64decode(payload))
        shortened = base64.b64encode(gzip.compress(data[:-1])).decode('ascii')
        import struct
        with self.assertRaises(struct.error):
            decode('"'.join((header, shortened, signature)))


if __name__ == '__main__':
    unittest.main()
