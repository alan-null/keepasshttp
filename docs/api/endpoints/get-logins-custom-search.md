---
layout: default
title: get-logins-custom-search
parent: Endpoints
nav_order: 6
---

# get-logins-custom-search
{: .d-inline-block }
POST
{: .label .label-yellow }

Performs a KeePass "Find" using custom search options and returns matching entries. See [common-fields](../common-fields).

# Request

### Fields:

| Field                           | Description / Value                                           | Default                      | Required |
| :------------------------------ | :------------------------------------------------------------ | :--------------------------- | :------- |
| `RequestType`                   | "get-logins-custom-search"                                    | —                            | Yes      |
| `Id`                            | Associated key `Id`                                           | —                            | Yes      |
| `Nonce`                         | 16-byte Base64 random                                         | —                            | Yes      |
| `Verifier`                      | `Nonce` encrypted with key                                    | —                            | Yes      |
| `SearchString`                  | Search text (encrypted)                                       | —                            | Yes      |
| `SearchInTitles`                | Search in entry titles                                        | `true`                       | Optional |
| `SearchInUserNames`             | Search in usernames                                           | `true`                       | Optional |
| `SearchInPasswords`             | Search in passwords                                           | `false`                      | Optional |
| `SearchInUrls`                  | Search in URLs                                                | `true`                       | Optional |
| `SearchInNotes`                 | Search in notes / comments                                    | `true`                       | Optional |
| `SearchInOther`                 | Search other standard fields                                  | `true`                       | Optional |
| `SearchInStringNames`           | Search in custom string field names                           | `false`                      | Optional |
| `SearchInTags`                  | Search in entry tags                                          | `true`                       | Optional |
| `SearchInUuids`                 | Search in entry UUIDs                                         | `false`                      | Optional |
| `SearchInGroupPaths`            | Search in full group paths                                    | `false`                      | Optional |
| `SearchInGroupNames`            | Search in group names                                         | `false`                      | Optional |
| `SearchInHistory`               | Search in entry history items                                 | `false`                      | Optional |
| `SearchMode`                    | KeePass search mode (`Simple`, `RegularExpression`, `XPath`.) | `Simple`                     | Optional |
| `ExcludeExpired`                | Exclude expired entries from results                          | `false`                      | Optional |
| `RespectEntrySearchingDisabled` | Honor per-group “search entries in this group” option         | `true`                       | Optional |
| `ComparisonMode`                | String comparison mode (e.g. `InvariantCultureIgnoreCase`)    | `InvariantCultureIgnoreCase` | Optional |

**Example**:
```json
{
  "RequestType": "get-logins-custom-search",
  "Id": "client1",
  "Nonce": "ReqNonce==",
  "Verifier": "EncryptedReqNonce==",
  "SearchString": "EncryptedSearchText==",
  "SearchInTitles": true,
  "SearchInUserNames": true,
  "SearchInUrls": true,
  "SearchInNotes": true,
  "SearchInTags": true,
  "ExcludeExpired": false,
  "RespectEntrySearchingDisabled": true
}
```

# Response

`Success` may be true with empty `Entries`.

See [common-fields](../common-fields#response-envelope) for the common response envelope.

### Successful:
```json
{
  "RequestType": "get-logins-custom-search",
  "Success": true,
  "Id": "client1",
  "Count": 2,
  "Entries": [
    {
      "Name": "EncryptedTitle1==",
      "Login": "EncryptedUser1==",
      "Password": "EncryptedPass1==",
      "Uuid": "EncryptedUuidHex1=="
    },
    {
      "Name": "EncryptedTitle2==",
      "Login": "EncryptedUser2==",
      "Password": "EncryptedPass2==",
      "Uuid": "EncryptedUuidHex2=="
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
  "Error": "Exception message describing the failure",
  "RequestType": "get-logins-custom-search",
  "Success": false,
  "Version": "x.y.z",
  "Hash": "dbHashSha1"
}
```

## Notes
- Uses KeePass’ internal search engine with fine-grained flags.
- Empty result still returns `Success: true` if the request was valid.