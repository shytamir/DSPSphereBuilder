# Compile reference map

These assemblies provide type declarations for CI compilation. They are never
installed, shipped, or used to simulate game behavior. The local assemblies remain
the target authority. [PROJECT.md](../docs/PROJECT.md) owns decisions and state.

[Map.json](Map.json) maps each declared public/protected surface to the actual
assembly identity, hash/MVID, native type/base, member signature, and metadata
token. Signatures distinguish fields from accessors and instance from static
members through the checked declaration. Only required members are declared;
unreferenced interfaces and native implementation bodies are not reproduced.

For each commit adding/changing a reference:

1. Inspect the actual target type/member. Add only the needed declaration.
2. Build the shims and run `scripts/Test-ReferenceMap.ps1` with the real managed
   and BepInEx core paths. `-UpdateMap` writes the successfully checked inventory;
   review its diff and commit it with the declarations and production usage.
3. Compile production code in both reference modes and inspect any changed
   emitted member/assembly references. Source compilation alone can miss a field
   versus property or declaring-assembly difference.

The checker reads metadata with the loader's existing Mono.Cecil library. It
does not load or invoke native game types. Generated outputs stay in `artifacts/`.
The map's equality check detects a changed recorded API inventory, not player UI
wording or an implementation-specific behavior assertion.
