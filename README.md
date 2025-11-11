# KeePassHttp

KeePassHttp is a plugin for KeePass 2.x that exposes selected KeePass entries over a local HTTP endpoint for trusted clients (e.g. [chromeIPass](https://github.com/alan-null/chromeIPass)) after a user-approved AES key association.

## Features
- Return all matching entries for a given URL (or only the best matches when configured).
- Update existing entries (with optional confirmation).
- Secure per-client AES key exchange and encrypted field transport.
- Notify the user when credentials are delivered.
- Allow or deny access per entry (with “remember” option).
- Operate only while the database is unlocked (with optional unlock prompt on demand).
- Optionally search across all opened databases.
- Provide optional extra string fields (prefixed with `KPH: `).

## System requirements
- KeePass 2.17 or higher
- Windows XP SP3 or later
- Linux: recent Mono installation
- macOS: Mono (appears fully supported, but not continuously tested)

## Documentation

Full documentation for the latest released version is hosted at:
[**alan-null.github.io/keepasshttp**](https://alan-null.github.io/keepasshttp)

Each published version has its documentation in a branch named `docs/{version}` (e.g. `docs/2.0.0.0`).

The `master` branch contains the **development (unreleased)** version and its in-progress documentation.