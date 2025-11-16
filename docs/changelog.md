---
layout: default
title: Changelog
nav_order: 8
toc_title: "Versions:"
---

# Changelog
All notable changes to this project will be documented in this file.

## [Unreleased]
{: .d-inline-block }
Coming soon
{: .label .label-yellow }

### Added

- New handler `get-login-by-uuid` ([#30])
- Extended `set-login` to include extra fields ([#26])
    - Possibility to set "Name" along with "Login" and "Password" for `set-login` API ([#25])
    - Possibility to set and update string fields ([#5])

### Fixed

- URL trimmed incorrectly when creating a new entry ([code][base_url_trim])


## [v2.1.0.0]
{: .d-inline-block }
Latest
{: .label .label-green }

### Added

- Integration tests for all API endpoints ([#9])
- Abstraction layer for configuration testing ([#10] [#12] [#13])
- Possibility to get "group" name when listing credentials ([#7])
- Add issue templates for bug reports and feature requests ([#16])
- Add new handler `get-logins-by-names` ([#18])
- New endpoint `GET /version` to retrieve current plugin version ([api/version](../api/endpoints/version))
- Create documentation page ([#23])
- UI label and tooltip for Options window
- CheckUpdates configuration option ([#28])
- Ability to define key **Id** for `associate` requests ([#29])

### Fixed

- The `get-all-logins` function does not return all available logins ([#11])
- The `get-all-logins` function does not sort results ([#14])
- Exception for unknown handler ([#15])
- set-login return incorrect status ([#17])
- Fix inconsistency in `get*logins` handlers ([#20])

### Changed

- Refactor code for improved readability and maintainability ([#21])
- Refactor and enhance Request/Response models ([#27])

### Removed

- `Count` property from numerous response models where it was redundant ([#27])

## [v2.0.0.0]

### Fixed
- Update check failed: version information file could not be downloaded ([#1])


## [v1.8.4.2]

### Notes
- Legacy release of **KeePassHttp** by [Perry Nguyen].

<!-- versions -->
[unreleased]: https://github.com/alan-null/keepasshttp/compare/v2.1.0.0...HEAD
[v2.1.0.0]: https://github.com/alan-null/keepasshttp/compare/v2.0.0.0...v2.1.0.0
[v2.0.0.0]: https://github.com/alan-null/keepasshttp/compare/v1.8.4.2...v2.0.0.0
[v1.8.4.2]: https://github.com/alan-null/keepasshttp/compare/v1.8.4.2...v1.8.4.2

<!-- issues -->
[#30]: https://github.com/alan-null/keepasshttp/issues/30
[#29]: https://github.com/alan-null/keepasshttp/issues/29
[#28]: https://github.com/alan-null/keepasshttp/issues/28
[#27]: https://github.com/alan-null/keepasshttp/issues/27
[#26]: https://github.com/alan-null/keepasshttp/issues/26
[#25]: https://github.com/alan-null/keepasshttp/issues/25
[#23]: https://github.com/alan-null/keepasshttp/issues/23
[#21]: https://github.com/alan-null/keepasshttp/issues/21
[#20]: https://github.com/alan-null/keepasshttp/issues/20
[#18]: https://github.com/alan-null/keepasshttp/issues/18
[#17]: https://github.com/alan-null/keepasshttp/issues/17
[#16]: https://github.com/alan-null/keepasshttp/issues/16
[#15]: https://github.com/alan-null/keepasshttp/issues/15
[#14]: https://github.com/alan-null/keepasshttp/issues/14
[#13]: https://github.com/alan-null/keepasshttp/issues/13
[#12]: https://github.com/alan-null/keepasshttp/issues/12
[#11]: https://github.com/alan-null/keepasshttp/issues/11
[#10]: https://github.com/alan-null/keepasshttp/issues/10
[#9]: https://github.com/alan-null/keepasshttp/issues/9
[#7]: https://github.com/alan-null/keepasshttp/issues/7
[#5]: https://github.com/alan-null/keepasshttp/issues/5
[#1]: https://github.com/alan-null/keepasshttp/issues/1

<!-- commits -->
[base_url_trim]: https://github.com/alan-null/keepasshttp/blob/ca00b13d244adefbdb0e81ec8b12b036bb7b26f6/KeePassHttp/Handlers.cs#L999-L1003