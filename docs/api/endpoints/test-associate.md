---
layout: default
title: test-associate
parent: Endpoints
nav_order: 2
---

# test-associate
{: .d-inline-block }
POST
{: .label .label-yellow }

Verifies an existing association. See [common-fields](../common-fields).

# Request

### Fields:

| Field         | Description / Value        | Required |
| :------------ | :------------------------- | :------- |
| `RequestType` | "test-associate"           | Yes      |
| `Id`          | Associated key `Id`        | Yes      |
| `Nonce`       | 16-byte Base64 random      | Yes      |
| `Verifier`    | `Nonce` encrypted with key | Yes      |

**Example**:
```json
{
  "RequestType": "test-associate",
  "Id": "client1",
  "Nonce": "Base64Nonce==",
  "Verifier": "EncryptedNonce=="
}
```

# Response

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful:
```json
{
  "RequestType": "test-associate",
  "Success": true,
  "Id": "client1",
  "Version": "x.y.z",
  "Hash": "dbHashSha1",
  "Nonce": "RespNonce==",
  "Verifier": "EncryptedRespNonce=="
}
```

### Failure:
```json
{
  "Error": "Exception message describing the failure",
  "RequestType": "test-associate",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```