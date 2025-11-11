---
layout: default
title: get-logins-by-names
parent: Endpoints
nav_order: 5
---

# get-logins-by-names
Returns entries whose **Title** matches any encrypted name. See [common-fields](../common-fields).

# Request
### Fields:

| Field         | Description / Value                         | Required |
| :------------ | :------------------------------------------ | :------- |
| `RequestType` | "get-logins-by-names"                       | Yes      |
| `Id`          | Associated key `Id`                         | Yes      |
| `Nonce`       | 16-byte Base64 random                       | Yes      |
| `Verifier`    | `Nonce` encrypted with key                  | Yes      |
| `Names`       | Array of encrypted entry titles (non-empty) | Yes      |

**Example**:
```json
{
  "RequestType": "get-logins-by-names",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "Names": [
    "EncryptedTitle1==",
    "EncryptedTitle2=="
  ]
}
```

# Response

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful Response:
```json
{
  "RequestType": "get-logins-by-names",
  "Success": true,
  "Id": "client1",
  "Count": 2,
  "Entries": [
    {
      "Name": "EncryptedTitle1==",
      "Login": "EncryptedUser1==",
      "Password": "EncryptedPass1==",
      "Uuid": "EncryptedUuidHex1==",
      "Group": { "Name": "Encrypted/Group/Path1==", "Uuid": "EncryptedGroupUuid1==" },
      "StringFields": [
        { "Key": "EncryptedNoteKey==", "Value": "EncryptedNoteValue==" }
      ]
    },
    {
      "Name": "EncryptedTitle2==",
      "Login": "EncryptedUser2==",
      "Password": "EncryptedPass2==",
      "Uuid": "EncryptedUuidHex2==",
      "Group": { "Name": "Encrypted/Group/Path2==", "Uuid": "EncryptedGroupUuid2==" },
      "StringFields": null
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
  "RequestType": "get-logins-by-names",
  "Success": false,
  "Count": 0,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```

## Notes
Useful when client already has entry titles cached and wants direct retrieval. Filtering by Allow/Deny still applies.