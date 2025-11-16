---
layout: default
title: get-login-by-uuid
parent: Endpoints
nav_order: 6
---

# get-login-by-uuid
{: .d-inline-block }
POST
{: .label .label-yellow }

Returns single entry whose **Uuid** matches the provided encrypted UUID. See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value                      | Required |
| :------------ | :--------------------------------------- | :------- |
| `RequestType` | "get-login-by-uuid"                      | Yes      |
| `Id`          | Associated key `Id`                      | Yes      |
| `Nonce`       | 16-byte Base64 random                    | Yes      |
| `Verifier`    | `Nonce` encrypted with key               | Yes      |
| `Uuid`        | The unique identifier of a KeePass entry | Yes      |

**Example**:
```json
{
  "RequestType": "get-login-by-uuid",
  "Id": "client1",
  "Uuid": "EncryptedReqUuid==",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce=="
}
```

# Response

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful Response:
```json
{
  "RequestType": "get-login-by-uuid",
  "Success": true,
  "Id": "client1",
  "Count": 1,
  "Entries": [
    {
      "Name": "EncryptedTitle1==",
      "Login": "EncryptedUser1==",
      "Password": "EncryptedPass1==",
      "Uuid": "EncryptedUuidHex1==",
      "Group": { "Name": "Encrypted/Group/Path1==", "Uuid": "EncryptedGroupUuid1==" }
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
  "Error": "Exception message describing the failure",
  "RequestType": "get-login-by-uuid",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```