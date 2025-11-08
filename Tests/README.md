# KeePassHttp Test Suite

This document describes the methodology and structure of the automated **Pester** tests for the **KeePassHttp** plugin.

## Test Configuration Activation

The plugin selects its configuration provider at startup:

**Test mode** triggers when:
  - KeePass is launched with the command line flag: `--kph-ev-config`, OR
  - Environment variable `KPH_EV_CONFIG` is set to `1` or `true` (case-insensitive).

- In test mode: `EnvironmentConfigProvider` is used.

Otherwise: `DefaultConfigProvider` is used.

Reference: selection logic in `KeePassHttp.cs` (`CreateDefaultFactory()`).

## Running the Tests

Prerequisites:
- Plugin installed

Example invocation (PowerShell):

```powershell
pwsh -File .\Tests\KeePassHttp.Tests.ps1
```
