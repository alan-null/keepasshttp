---
layout: default
title: Common Fields
parent: API
nav_order: 2
---

# Common Fields

## Overview
All API requests share a JSON envelope. Sensitive values are encrypted individually. This document unifies naming, required flags, and structural conventions.

## Request Envelope

| Field           | Type   | Required                         | Description                                                  |
| :-------------- | :----- | :------------------------------- | :----------------------------------------------------------- |
| `RequestType`   | string | Yes                              | Endpoint identifier string (e.g. `get-logins`).              |
| `Id`            | string | Yes (except initial `associate`) | Associated AES key identifier chosen by user.                |
| `Nonce`         | string | Yes                              | Base64 16-byte random value; AES IV and plaintext challenge. |
| `Verifier`      | string | Yes                              | `Nonce` encrypted with AES key (AES-CBC, IV=`Nonce`).        |
| `Key`           | string | Yes (`associate` only)           | Base64 AES key (recommend 32 bytes).                         |
| `TriggerUnlock` | string | Optional                         | "true" forces database unlock prompt.                        |
| `SortSelection` | string | Optional                         | Internal sorting hint.                                       |
| `Url`           | string | Optional (context lookups)       | Current page / target URL (encrypted).                       |
| `SubmitUrl`     | string | Optional                         | Form submission / alternate URL (encrypted).                 |
| `Realm`         | string | Optional                         | Realm string (encrypted).                                    |
| `Names[]`       | array  | Yes (`get-logins-by-names`)      | Entry titles (encrypted).                                    |
| `Login`         | string | Yes (`set-login`)                | Username (encrypted).                                        |
| `Password`      | string | Yes (`set-login`)                | Password (encrypted).                                        |
| `Uuid`          | string | Optional (`set-login`)           | Entry UUID (encrypted).                                      |

> Sensitive fields are individually AES-CBC encrypted using the same `Nonce` as IV.

## Response Envelope

Fields may be omitted if not applicable:

| Field         | Type   | Required        | Description                                   |
| :------------ | :----- | :-------------- | :-------------------------------------------- |
| `RequestType` | string | Yes             | Mirrors request.                              |
| `Success`     | bool   | Yes             | Operation result (true even with empty data). |
| `Id`          | string | Yes             | Associated key Id.                            |
| `Entries`     | array  | When applicable | Present for data/list endpoints.              |
| `Count`       | number | When applicable | Entry count.                                  |
| `Version`     | string | Always          | Plugin version.                               |
| `Hash`        | string | Always          | DB identity (SHA1 rootUuid + recycleBinUuid). |
| `Nonce`       | string | Always          | Response nonce (Base64 16 bytes).             |
| `Verifier`    | string | Always          | `Nonce` encrypted with key.                   |
| `Error`       | string | Optional        | Internal error only.                          |

### Entry Object (Credentials)
Used by: `get-logins`, `get-logins-by-names`, `get-all-logins`, `generate-password` (variation).

| Field                  | Type   | Notes                                                                |
| :--------------------- | :----- | :------------------------------------------------------------------- |
| `Name`                 | string | Entry title / host (encrypted).                                      |
| `Login`                | string | Username (encrypted) or strength bits (`generate-password`).         |
| `Password`             | string | Password (encrypted) or generated value.                             |
| `Uuid`                 | string | Entry UUID (hex, encrypted) or literal marker (`generate-password`). |
| `Group`                | object | Optional (absent for `generate-password`).                           |
| `Group.Name`           | string | Full path (encrypted).                                               |
| `Group.Uuid`           | string | Group UUID (encrypted).                                              |
| `StringFields`         | array  | Optional extra fields (encrypted).                                   |
| `StringFields[].Key`   | string | Field name.                                                          |
| `StringFields[].Value` | string | Field value.                                                         |

## Endpoint-Specific Differences

| Endpoint          | Differences                                                                   |
| :---------------- | :---------------------------------------------------------------------------- |
| associate         | No `Entries`; `Count` always 0.                                               |
| test-associate    | No `Entries`.                                                                 |
| set-login         | No `Entries`; success indicates create/update.                                |
| get-logins-count  | No `Entries`; only `Count`.                                                   |
| generate-password | Single synthetic entry; no `Group` / `StringFields`; `Login` = strength bits. |


## Encryption Rules
- AES-CBC, PKCS7; Key = associated key; IV = `Nonce`.
- Each sensitive field (`Url`, `SubmitUrl`, `Realm`, `Names[]`, `Login`, `Password`, `Uuid`) encrypted separately with same `Nonce`.
- Response uses fresh `Nonce`/`Verifier` pair.
