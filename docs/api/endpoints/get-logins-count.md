---
layout: default
title: get-logins-count
parent: Endpoints
nav_order: 4
---

# get-logins-count
{: .d-inline-block }
POST
{: .label .label-yellow }

Returns only a match count. See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value        | Required |
| :------------ | :------------------------- | :------- |
| `RequestType` | "get-logins-count"         | Yes      |
| `Id`          | Associated key `Id`        | Yes      |
| `Nonce`       | 16-byte Base64 random      | Yes      |
| `Verifier`    | `Nonce` encrypted with key | Yes      |
| `Url`         | Target URL (encrypted)     | Yes      |

**Example**:
```json
{
  "RequestType": "get-logins-count",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "Url": "EncryptedFormUrl=="
}
```

# Response

Only `Count` is populated (no `Entries`).

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful:
```json
{
    "RequestType": "get-logins-count",
    "Success": true,
    "Id": "client1",
    "Count": 2,
    "Nonce": "RespNonce==",
    "Verifier": "EncryptedRespNonce==",
    "Version": "x.y.z",
    "Hash": "dbHashSha1"
}
```

### Failure:
```json
{
    "Error": "Exception message describing the failure",
    "RequestType": "get-logins-count",
    "Success": false,
    "Version": "x.y.z",
    "Hash": "dbHashSha1"
}```
