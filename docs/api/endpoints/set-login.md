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

If [`Always allow updating entries`](../../configuration#-always-allow-updating-entries) option is disabled, updating existing entries will require user confirmation in KeePass.
Confirmation dialog will show the entry name and modified fields.


# Request
### Fields:

| Field          | Description / Value                                                            | Required |
| :------------- | :----------------------------------------------------------------------------- | :------- |
| `RequestType`  | "set-login"                                                                    | Yes      |
| `Id`           | Associated key `Id`                                                            | Yes      |
| `Nonce`        | 16-byte Base64 random                                                          | Yes      |
| `Verifier`     | `Nonce` encrypted with key                                                     | Yes      |
| `Url`          | Page URL (encrypted)                                                           | Optional |
| `Login`        | Username (encrypted)                                                           | Optional |
| `Password`     | Password (encrypted)                                                           | Optional |
| `Uuid`         | Existing entry UUID (encrypted)                                                | Optional |
| `SubmitUrl`    | Alternate submit host (encrypted)                                              | Optional |
| `Realm`        | Realm (encrypted)                                                              | Optional |
| `StringFields` | Dictionary of additional string fields. Both keys and values must be encrypted | Optional |

{: .warning }
**StringFields** keys must be unique per entry.
If a key already exists, its value will be updated.
Both keys and values must be encrypted using the same method as other fields.

{: .highlight }
To remove a string field from entry, set its value to `null`. Empty strings are considered valid values.
This rule applies only to string fields in `StringFields`. Other fields cannot be removed this way (`null` values for other fields will be ignored).

**Example**:
```json
{
  "RequestType": "set-login",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "Url": "EncryptedPageUrl==",
  "Login": "EncryptedUsername==",
  "Password": "EncryptedPassword==",
  "StringFields": {
    "EncryptedKey1==": "EncryptedValue1==",
    "EncryptedKey2==": "EncryptedValue2==",
    "EncryptedKeyToRemove==": null
  }
}
```

# Response

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful (HTTP 200):
```json
{
  "RequestType": "set-login",
  "Success": true,
  "Uuid": "entry-uuid-1234",
  "Id": "client1",
  "Nonce": "RespNonce==",
  "Verifier": "EncryptedRespNonce==",
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```
`Success` can be **false** when none of the fields were changed compared to the existing entry.

### Failure (HTTP 400):
```json
{
  "Error": "Error message describing the failure",
  "RequestType": "set-login",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```