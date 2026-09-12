import json
import pathlib
import sys

from blueprint_geometry import decode
from derive_patches import derive


plan = derive(decode(pathlib.Path('research/cosmin1490/60.txt').read_text(encoding='utf-8')))
steps = plan['steps']
used_nodes = sorted({n for step in steps for n in step['add_nodes']})
output = {
    'referenceSha256': plan['source']['sha256'],
    'nodes': [{'id': i, 'direction': dict(zip(('x', 'y', 'z'), plan['positions'][i]))} for i in used_nodes],
    'patches': [{'nodes': step['add_nodes'], 'frames': [{'a': a, 'b': b} for a,b in step['add_frames']]} for step in steps],
    'faces': plan['faces'],
}
path = pathlib.Path(sys.argv[1])
path.parent.mkdir(parents=True, exist_ok=True)
path.write_text(json.dumps(output, indent=2) + '\n', encoding='utf-8')
