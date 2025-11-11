---
layout: default
title: get-logins-count
parent: API Examples
nav_order: 4
---

# get-logins-count

Request:
```javascript
{
    "RequestType":"get-logins-count",
    "TriggerUnlock":"false",
    "Id":"PHP",
    "Nonce":"vCysO8UwsWyE2b+nMzE3/Q==",
    "Verifier":"5Nyi5973GawqdP3qF9QlAF/KlZAyvb6c5Smhun8n9wA=",
    "Url":"Gz+ZCSjHAGmeYdrtS78hSxH3yD5LiYidSq9n+8TdQXc=", // Encrypted URL
    "SubmitUrl":"<snip>" // Encrypted submit URL
}
```

Response:
```javascript
{
    "Count":3,
    "Entries":null,
    "Error":"",
    "Hash":"d8312a59523d3c37d6a5401d3cfddd077e194680",
    "Id":"PHP",
    "Nonce":"Aeh9maerCjE5v5V8Tz2YxA==",
    "RequestType":"get-logins",
    "Success":true,
    "Verifier":"F87c4ggkMTSEptJT8/FypBH491kRexTAiEZxovLMvD8=",
    "Version":"1.8.4.1",
    "objectName":""
}
```