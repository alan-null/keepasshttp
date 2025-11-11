---
layout: default
title: Overview
nav_order: 0
---

# KeePassHttp

is a plugin for KeePass 2.x and provides a secure means of exposing KeePass entries via HTTP for clients to
consume.

This plugin is primarily intended for use with [chromeIPass for Google Chrome](https://github.com/alan-null/chromeIPass).

## Features
 * returns all matching entries for a given URL
 * updates entries
 * secure exchange of entries
 * notifies user if entries are delivered
 * user can allow or deny access to single entries
 * works only if the database is unlocked
 * request for unlocking the database if it is locked while connecting
 * searches in all opened databases (if user activates this feature)
 * Whenever events occur, the user is prompted either by tray notification or requesting interaction (allow/deny/remember).

## System requirements
 * KeePass 2.17 or higher
 * For Windows: Windows XP SP3 or higher
 * For Linux: installed mono
 * For Mac: installed mono - it seems to fully support KeePassHttp, but we cannot test it

## Documentation & Versioning
Latest release documentation is published here. Versioned snapshots reside in branches named `docs/{version}` (e.g. `docs/2.0.0.0`). The `master` branch reflects unreleased development changes.

## Quick Navigation
- Association flow: API > associate / test-associate
- Retrieve entries: API > get-logins / get-logins-by-names / get-all-logins
- Count matches: API > get-logins-count
- Create or update: API > set-login
- Generate passwords: API > generate-password
- Field schema: API > Common Fields (see configuration notes)