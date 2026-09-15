import copy
import pathlib
import unittest

from blueprint_geometry import decode
from verify_geometry import compare, verify_geometry
from derive_patches import derive
from check_envelope import point_segment_squared, scaled_directions


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

    def test_pool_enumeration_does_not_change_patches(self):
        altered = copy.deepcopy(self.reference)
        for kind in ('nodes', 'frames', 'shells'):
            altered[kind] = dict(reversed(list(altered[kind].items())))
        self.assertEqual(derive(self.reference), derive(altered))

    def test_rotation_preserves_all_pairwise_distances(self):
        import itertools
        import math
        from blueprint_geometry import unit
        for grid_aligned in (False, True):
            with self.subTest(grid_aligned=grid_aligned):
                rotated = derive(self.reference, grid_aligned=grid_aligned)['positions']
                for a, b in itertools.combinations(rotated, 2):
                    original_distance = math.dist(unit(self.reference['nodes'][a]['position']), unit(self.reference['nodes'][b]['position']))
                    self.assertAlmostEqual(math.dist(rotated[a], rotated[b]), original_distance, places=14)

    def test_segment_projection_clamps_beyond_endpoint(self):
        self.assertAlmostEqual(point_segment_squared((0, -1, 0), (1, 0, 0), (0, 1, 0)), 2)

    def test_segment_projection_returns_to_sphere(self):
        import math
        middle = (math.sqrt(0.5), math.sqrt(0.5), 0)
        self.assertAlmostEqual(point_segment_squared(middle, (1, 0, 0), (0, 1, 0)), 0)

    def test_invalid_radius_is_rejected(self):
        for radius in (0, -1, float('inf'), float('nan')):
            with self.subTest(radius=radius), self.assertRaises(ValueError):
                scaled_directions({1: (1, 0, 0)}, radius)


if __name__ == '__main__':
    unittest.main()
