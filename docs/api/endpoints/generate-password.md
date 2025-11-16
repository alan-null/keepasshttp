---
layout: default
title: generate-password
parent: Endpoints
nav_order: 9
---

# generate-password
{: .d-inline-block }
POST
{: .label .label-yellow }

Generates a password (not persisted). See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value        | Required |
| :------------ | :------------------------- | :------- |
| `RequestType` | "generate-password"        | Yes      |
| `Id`          | Associated key `Id`        | Yes      |
| `Nonce`       | 16-byte Base64 random      | Yes      |
| `Verifier`    | `Nonce` encrypted with key | Yes      |

**Example**:
```json
{
  "RequestType": "generate-password",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce=="
}
```

# Response

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful:
```json
{
  "RequestType": "generate-password",
  "Success": true,
  "Id": "client1",
  "Count": 1,
  "Entries": [
    {
      "Name": "EncryptedText==",
      "Login": "EncryptedBits==",
      "Password": "EncryptedPasswordValue==",
      "Uuid": "EncryptedText=="
    }
  ],
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
  "RequestType": "generate-password",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```