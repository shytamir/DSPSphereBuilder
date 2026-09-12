# Agent working practices

## Before changing files

Read this file, `docs/PROJECT.md`, and the files directly relevant to the request.
Check Git status and preserve unrelated or unexplained changes. Current user
instructions take precedence over repository guidance; ask only when a missing
decision or conflicting edit prevents a sound result.

`docs/PROJECT.md` owns current scope, phase, tracked work status, readiness, and
acceptance. Other documents define behavior, work, procedures, or evidence and
link there for state. Keep this file about agent conduct, without product
requirements, backlog items, release status, or session history.

## Keep the change bounded

Make the smallest coherent change that fulfills the request. Reference
repositories are read-only examples of conventions, not sources of architecture,
identities, project decisions, or code to transplant.

Prefer direct code and native mechanisms. Add abstractions, dependencies,
compatibility gates, recovery systems, or tooling only for a demonstrated need.
Inspect the relevant native behavior before inventing a substitute. Do not turn
uncertainty into speculative frameworks or unrelated cleanup.

Keep tools, caches, generated files, and temporary diagnostics under ignored
`artifacts/`. Do not commit credentials, machine-specific paths, dependency
binaries, game assemblies, or decompiled game code. Treat installed software
and other repositories as external inputs; change them only within explicit
authorization.

## Investigate and validate proportionately

Use targeted searches and batch independent reads. Prefer existing tools and
commands; avoid adding a bootstrap system for tools already available.

Run the narrowest check that exercises the changed contract. Add tests for
meaningful failure cases, not assertions that merely repeat the implementation.
Distinguish inspection, package validation, compilation, runtime observation,
owner acceptance, and publication. Report skipped checks and uncertainty plainly.

After a failure, identify the cause before repeating the command. Retry only
after a relevant change or evidence of a transient condition, with at most one
unchanged retry. If the same failure persists, stop that path, report the exact
blocker, and continue independent work. Do not reinstall tools, widen permissions,
change dependencies, or rerun full suites in a loop. Access failures are not
source failures; use the supported permission route when needed.

Once relevant checks pass, repeat or broaden them only for new changes, new
failures, or a concrete unresolved concern. Stop when the requested result is
complete.

## Documentation and Git

Update documentation when its contract or authoritative state changes. Link to
existing information instead of maintaining parallel status tables. Do not add
registers, reports, or placeholder documents solely to record activity.

Review the final diff and stage named, intended paths only. Commit and push when
authorized by the user; that authorization persists for the requested work.
Do not rewrite history or force-push. Check remote state before pushing and
preserve concurrent changes.

For Git ownership errors, use `git -c safe.directory=<repository-path>` per
command instead of changing global configuration.

Finish with the result, relevant validation, commit/push outcome when requested,
and any remaining blocker. Keep the report concise and evidence-based.
