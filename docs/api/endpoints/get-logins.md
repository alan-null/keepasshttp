---
layout: default
title: get-logins
parent: Endpoints
nav_order: 3
---

# get-logins

Returns matching credentials for a form (optional submit URL / realm). See [common-fields](../common-fields).

# Request

### Fields:

| Field         | Description / Value        | Required |
| :------------ | :------------------------- | :------- |
| `RequestType` | "get-logins"               | Yes      |
| `Id`          | Associated key `Id`        | Yes      |
| `Nonce`       | 16-byte Base64 random      | Yes      |
| `Verifier`    | `Nonce` encrypted with key | Yes      |
| `Url`         | Page/form URL (encrypted)  | Yes      |
| `SubmitUrl`   | Submission URL (encrypted) | Optional |
| `Realm`       | Realm string (encrypted)   | Optional |

**Example**:
```json
{
  "RequestType": "get-logins",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "Url": "EncryptedFormUrl=="
}
```

# Response

`Success` may be true with empty `Entries`.

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful Response (sample):
```json
{
  "RequestType": "get-logins",
  "Success": true,
  "Id": "client1",
  "Count": 1,
  "Entries": [
    {
      "Name": "EncryptedHostOrTitle==",
      "Login": "EncryptedUsername==",
      "Password": "EncryptedPassword==",
      "Uuid": "EncryptedEntryUuidHex==",
      "Group": { "Name": "EncryptedFullPath==", "Uuid": "EncryptedGroupUuid==" },
      "StringFields": [
        { "Key": "EncryptedFieldName==", "Value": "EncryptedFieldValue==" }
      ]
    }
  ],
  "Nonce": "RespNonce==",
  "Verifier": "EncryptedRespNonce==",
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```

### Failure Response (verifier / id issue):
```json
{
  "RequestType": "get-logins",
  "Success": false,
  "Count": 0,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```

## Matching Logic Highlights
- Host decomposition (progressive trimming by dots).
- Expired entries hidden if configured.
- Allow/Deny lists override host checks.
- Optional scheme match.
- Levenshtein distance used for selection & sorting; may collapse to minimal distance if SpecificMatchingOnly enabled.

## Notes
Empty result still returns Success=true. Use get-logins-count for a lightweight existence check.