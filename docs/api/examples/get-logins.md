---
layout: default
title: get-logins
parent: API Examples
nav_order: 2
---

# get-logins

Request:
```javascript
{
    "RequestType":"get-logins",
    "SortSelection":"true",
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
    "Entries":[
        {
            "Login":"{encrypted login base64}",
            "Name":"{encrypted item name}",
            "Password":"{encrypted Password}",
            "StringFields":null,
            "Uuid":"{encrypted UUID}"
        },
        {
            <snip>
        },
        {
            <snip>
        }
    ],
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