# Codebase Analysis: `com.anubis.core`

## Overview

This repository is a Unity package (`com.anubis.core`) with reusable runtime utilities and managers focused on kiosk/interactive-app behavior:

- Screen and state flow (`GameManager`, `SubScreenManager`)
- Keyboard binding and debug controls (`KeyManager`)
- Audio playback (`SFXManager`)
- Date-gated lock behavior (`DateMananger`)
- Utility behaviors (animation events, gizmo drawing, activity logging, multi-display activation, phone formatting)

## Architecture Snapshot

- **Package metadata** in `package.json` (Unity 6000.2, UGUI dependency).
- **Managers** under `Runtime/Managers` handle app-level orchestration.
- **Utilities** under `Runtime/Utilities` are mostly standalone MonoBehaviours.
- No automated test project is currently present in the package.

## Strengths

1. **Clear responsibility split** between screen, input, and audio manager components.
2. **Inspector-friendly setup** (serializable classes, headers, tooltips) for designer workflows.
3. **Reusable utility scripts** for common production needs (gizmos, animation callbacks, display activation).

## High-Impact Issues

1. **PlayerPrefs key mismatch in date gating**
   - `DateMananger` initializes `CanProceed` but checks `canProceed`.
   - This causes date-lock bypass state to be inconsistent.
   - Recommendation: standardize to one key constant used everywhere.

2. **Potential static list growth / duplication in activity logs**
   - `ActivityRegister.knownTimes` is static and appended every read/write cycle.
   - `ReadActivityFile()` does not clear existing entries before adding lines, which may duplicate timestamps across calls.
   - Recommendation: clear or deduplicate before refill.

3. **Singleton robustness gaps**
   - `GameManager.instance` and `KeyManager.instance` are assigned without duplicate protection.
   - Multiple scene instances can lead to non-deterministic references.
   - Recommendation: align singleton handling with `SFXManager` pattern or remove static singleton usage.

4. **Subscreen activation may leave prior screen active**
   - `SubScreenManager.ChangeSubScreen` only activates target index without disabling previous one.
   - If called repeatedly, multiple subscreens can stay active.
   - Recommendation: deactivate all subscreens (or previous index) before enabling target.

## Medium-Priority Improvements

1. **Input handling coupling**
   - `GameManager` and `KeyManager` both watch direct key inputs (`P`, `R`, `Escape`, etc.).
   - Recommendation: centralize key dispatch via `KeyManager` bindings for consistency.

2. **Validation and defensive checks**
   - Several scripts assume serialized references are assigned (`mainScreens`, `lockScreen`, audio sources).
   - Recommendation: add null/index checks with clear error logs to prevent runtime exceptions.

3. **Naming and API consistency**
   - `DateMananger` appears misspelled; method/field naming style varies.
   - Recommendation: rename with compatibility strategy (class/file rename + migration note).

4. **Cross-platform path handling**
   - `ActivityRegister` manually builds Windows-style paths with `\\`.
   - Recommendation: use `Path.Combine` for portability and readability.

## Suggested Refactor Roadmap

1. **Stability Pass (short-term)**
   - Fix preference key consistency.
   - Add null/index guards.
   - Prevent duplicate singleton instances.

2. **Behavioral Correctness Pass**
   - Normalize sub-screen switching logic.
   - Harden activity log read/write cycle.

3. **Maintainability Pass**
   - Introduce shared constants for PlayerPrefs keys.
   - Standardize naming conventions.
   - Add edit-mode tests for pure utility logic (e.g., phone formatting).

## Risk Assessment

- **Runtime Risk**: Moderate (state bugs likely under edge flows, e.g., multiple managers, repeated subscreen changes).
- **Data Integrity Risk**: Low/Moderate (activity logs may become noisy/duplicated).
- **Maintenance Risk**: Moderate (inconsistent naming and tightly coupled input pathways).

## Quick Wins

- Create `CorePrefsKeys` constants file.
- Add `OnValidate` checks for required references in key managers.
- Update `SubScreenManager` to activate exactly one sub-screen.
- Switch file path composition to `Path.Combine` in `ActivityRegister`.


## Recommended Next Actions (Practical Sequence)

If you want the **fastest path to stability**, I recommend this exact sequence:

1. **PR 1: Correctness hotfixes (small, safe)**
   - Unify `PlayerPrefs` key usage for proceed/lock behavior.
   - Ensure `SubScreenManager` disables previous/all subscreens before enabling next.
   - Add null/index guard rails in `GameManager` and `SFXManager` entry points.

2. **PR 2: Reliability hardening**
   - Add duplicate-instance protection to `GameManager` and `KeyManager` singletons.
   - Fix `ActivityRegister` timestamp duplication (`knownTimes.Clear()` before repopulating).
   - Replace manual path concatenation with `Path.Combine`.

3. **PR 3: Maintainability + tests**
   - Introduce shared constants for all `PlayerPrefs` keys.
   - Rename `DateMananger` to `DateManager` (with migration note).
   - Add edit-mode tests for pure/mostly pure logic (phone formatter + key mapping helpers).

## What I Would Do First (if we only do one PR now)

Start with **PR 1 (Correctness hotfixes)** because it reduces immediate runtime defects with minimal refactor risk.

### Success Criteria for PR 1

- Date lock behavior is deterministic across sessions.
- Switching sub-screens never leaves old sub-screens active.
- No null-reference exceptions when references are missing in manager inspectors.

