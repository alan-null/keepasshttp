---
layout: default
title: get-all-logins
parent: Endpoints
nav_order: 6
---

# get-all-logins
Returns every non-expired entry. See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value        | Required |
| :------------ | :------------------------- | :------- |
| `RequestType` | "get-all-logins"           | Yes      |
| `Id`          | Associated key `Id`        | Yes      |
| `Nonce`       | 16-byte Base64 random      | Yes      |
| `Verifier`    | `Nonce` encrypted with key | Yes      |

**Example**:
```json
{
  "RequestType": "get-all-logins",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce=="
}
```

# Response

`Entries` contain all visible credentials (may be large). Entry object fields per schema.

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful Response (sample truncated):
```json
{
    "RequestType": "get-all-logins",
    "Success": true,
    "Id": "client1",
    "Count": 1,
    "Entries": [
        {
            "Name": "EncryptedTitleExample==",
            "Login": "EncryptedUserOne==",
            "Password": "EncryptedPassOne==",
            "Uuid": "EncryptedUuidHexOne==",
            "Group": { "Name": "Encrypted/Full/GroupPath==", "Uuid": "EncryptedGroupUuidOne==" }
        }
    ],
    "Nonce": "RespNonce==",
    "Verifier": "EncryptedRespNonce==",
    "Version": "x.y.z",
    "Hash": "dbHashSha1"
}
```

### Failure Response:
```json
{
  "RequestType": "get-all-logins",
  "Success": false,
  "Count": 0,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```