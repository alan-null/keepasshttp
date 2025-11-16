---
layout: default
title: set-login
parent: Endpoints
nav_order: 8
---

# set-login
{: .d-inline-block }
POST
{: .label .label-yellow }

Creates or updates an entry. See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value               | Required |
| :------------ | :-------------------------------- | :------- |
| `RequestType` | "set-login"                       | Yes      |
| `Id`          | Associated key `Id`               | Yes      |
| `Nonce`       | 16-byte Base64 random             | Yes      |
| `Verifier`    | `Nonce` encrypted with key        | Yes      |
| `Url`         | Page URL (encrypted)              | Yes      |
| `Login`       | Username (encrypted)              | Yes      |
| `Password`    | Password (encrypted)              | Yes      |
| `Uuid`        | Existing entry UUID (encrypted)   | Optional |
| `SubmitUrl`   | Alternate submit host (encrypted) | Optional |
| `Realm`       | Realm (encrypted)                 | Optional |

**Example**:
```json
{
  "RequestType": "set-login",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "Url": "EncryptedPageUrl==",
  "Login": "EncryptedUsername==",
  "Password": "EncryptedPassword=="
}
```

# Response

No `Entries` / `Count`.

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful:
```json
{
  "RequestType": "set-login",
  "Success": true,
  "Id": "client1",
  "Nonce": "RespNonce==",
  "Verifier": "EncryptedRespNonce==",
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```

### Failure:
```json
{
  "Error": "Error message describing the failure",
  "RequestType": "set-login",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```