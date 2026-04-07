# How to Use the New Scripts

This guide explains how to set up and use the new/refactored scripts introduced in the recent update.

## 1) `CorePrefsKeys`

**What it is:** centralized PlayerPrefs key names and helper builders.

**How to use:**

```csharp
// Set/read idle timer key consistently
PlayerPrefs.SetFloat(CorePrefsKeys.IdleTimer, 60f);
float seconds = PlayerPrefs.GetFloat(CorePrefsKeys.IdleTimer, 60f);

// Set/read can-proceed gate
PlayerPrefs.SetInt(CorePrefsKeys.CanProceed, 1);
int canProceed = PlayerPrefs.GetInt(CorePrefsKeys.CanProceed, 0);

// Build dynamic keys
string keyBinding = CorePrefsKeys.BindingKey("Play");        // KEY_Play
string dayCount  = CorePrefsKeys.ActivityCountKey("7_4_2026"); // activities_7_4_2026
```

## 2) `DateManager` (replacement for `DateMananger`)

**What it does:** locks/unlocks your `GameManager.lockScreen` based on date range and `CorePrefsKeys.CanProceed`.

### Setup in Unity

1. Add **DateManager** to a scene object.
2. In Inspector, set:
   - `Start Date String` (format `dd/MM/yyyy`)
   - `End Date String` (format `dd/MM/yyyy`)
3. Ensure a `GameManager` exists in scene and `lockScreen` is assigned.
4. Run scene:
   - inside range: lock screen is hidden
   - outside range: lock screen is shown

### Force-unlock for testing

You can toggle from `GameManager` using **P + L** (sets `CorePrefsKeys.CanProceed`), or set manually:

```csharp
PlayerPrefs.SetInt(CorePrefsKeys.CanProceed, 1);
PlayerPrefs.Save();
```

### Backward compatibility

`DateMananger` still exists as an `[Obsolete]` wrapper, so old references compile, but use `DateManager` for all new work.

## 3) `SubScreenManager`

**What changed:** switching subscreens now deactivates previous/all subscreens before enabling the target.

### Setup

1. Add `SubScreenManager` to a parent screen object.
2. Populate `subScreens` array in Inspector in desired order.
3. Call:

```csharp
subScreenManager.ChangeSubScreen(index);
```

It now validates index and logs a warning for invalid values.

## 4) `SFXManager`

**What changed:** safer null checks and dictionary guards.

### Setup

1. Place `SFXManager` in scene.
2. Assign:
   - `sfxSource`
   - `uiSource`
3. Fill `sounds` list (`id`, `clip`).
4. Play by id:

```csharp
SFXManager.instance.Play("button");
SFXManager.instance.PlayUI("hover");
```

If an id/source is missing, calls fail safely.

## 5) `KeyManager`

**What changed:** duplicate instance guard + centralized pref key generation.

### Setup

1. Add `KeyManager` to scene.
2. Populate `bindings` in Inspector:
   - `actionName`
   - `defaultKey`
   - optional `targetButton`
3. On run, saved keys are loaded from `CorePrefsKeys.BindingKey(actionName)`.

To rebind at runtime:

```csharp
KeyManager.instance.SetKey("OpenMenu", KeyCode.M);
```

## 6) `ActivityRegister`

**What changed:**
- clears cached `knownTimes` before reading,
- uses `Path.Combine`,
- uses shared `CorePrefsKeys.ActivityCountKey(...)`.

### Usage

Call when an activity completes:

```csharp
ActivityRegister.RegisterActivity();
```

Output file path:
`<Desktop>/Dados/<ProductName>/<day_month_year>.txt`

Enable hourly logging by setting:

```csharp
ActivityRegister.registerHours = true;
```

## 7) `PhoneNumberFormatUtility` + `PhoneNumberFormatter`

### `PhoneNumberFormatUtility`
Use this in pure code/tests:

```csharp
string formatted = PhoneNumberFormatUtility.FormatPhoneNumber("5511998765432");
// +55(11)99876-5432
```

### `PhoneNumberFormatter` component
1. Add `TMP_InputField` to UI.
2. Add `PhoneNumberFormatter` to same object.
3. At runtime, typing is auto-formatted while preserving unformatted digits in `UnformattedPhoneNumber`.

## 8) Editor Tests

### What exists
- `CorePrefsKeysTests`
- `PhoneNumberFormatUtilityTests`

### How to run in Unity
1. Open **Window > General > Test Runner**.
2. Select **EditMode**.
3. Run all tests in `Anubis.Core.EditorTests`.

> In this environment, Unity CLI is not installed, so tests must be run from Unity Editor locally/CI with Unity available.
