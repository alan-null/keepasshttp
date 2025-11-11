---
layout: default
title: associate
parent: Endpoints
nav_order: 1
---

# associate

Establishes a trust relationship by registering a client-supplied **AES** key under a user-chosen `Id`.
The server (plugin) validates that the client controls the key by checking the `Verifier` (`Nonce` encrypted with the provided `Key`).
After success, subsequent requests use the stored key via `Id`.
See [common-fields](../common-fields).

# Request

### Fields:

| Field         | Description                                        | Required |
| :------------ | :------------------------------------------------- | :------- |
| `RequestType` | "associate"                                        | Yes      |
| `Key`         | Base64 AES key (32 bytes recommended)              | Yes      |
| `Nonce`       | 16-byte Base64 random (IV + challenge)             | Yes      |
| `Verifier`    | `Nonce` encrypted with `Key` (AES-CBC, IV=`Nonce`) | Yes      |

**Example**:
```json
{
  "RequestType": "associate",
  "Key": "ksP1sw/6Bgx4CpqCO3JpI1+5vtSO8/aCcqxDjXTTYvw=",
  "Nonce": "2WQ4Hm9lUjRU0CXje3CeZA==",
  "Verifier": "idtdIoq7mHVwTGiFS0fvJR3PFNWqKmssEjOO9un2L+k="
}
```

# Response

`Entries` unused; `Count` always 0.

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful Response:
```json
{
  "RequestType": "associate",
  "Success": true,
  "Id": "client1",
  "Count": 0,
  "Version": "2.0.0.0",
  "Hash": "000c8edde13701752405676e684b7570c13a9291",
  "Nonce": "g4fYGDSufIbtEUGsuHvFcA==",
  "Verifier": "ii652Bj5kRBVkxDtmr3T8rIm72r7dj4zo3IiVflvPNk="
}
```

### Failure Response:
```json
{
  "RequestType": "associate",
  "Success": false,
  "Count": 0,
  "Version": "2.0.0.0",
  "Hash": "000c8edde13701752405676e684b7570c13a9291"
}
```

## Security Notes
- AES mode: CBC with PKCS7 padding; IV = Nonce (16 bytes).
- Verifier proves caller holds Key (decrypts to original Nonce).
- Key stored in a hidden configuration entry (KeePassHttp Settings) under field "AES Key: {Id}".
- Association requires explicit user confirmation; overwrite prompt appears if Id already exists.

## Next Steps
After association:
- Use test-associate to validate key: see [test-associate](test-associate.md)
- Retrieve logins: see get-logins.md
- Store/update login: see set-login.md
- Generate password: see generate-password.md