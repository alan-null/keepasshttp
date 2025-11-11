---
layout: default
title: associate
parent: API Examples
nav_order: 1
---

# associate

Request:
```javascript
{
    "RequestType":"associate",
    "Key":"CRyXRbH9vBkdPrkdm52S3bTG2rGtnYuyJttk/mlJ15g=", // Base64 encoded 256 bit key
    "Nonce":"epIt2nuAZbHt5JgEsxolWg==",
    "Verifier":"Lj+3N58jkjoxS2zNRmTpeQ4g065OlFfJsHNQWYaOJto="
}
```

Response:
```javascript
{
    "Count":null,
    "Entries":null,
    "Error":"",
    "Hash":"d8312a59523d3c37d6a5401d3cfddd077e194680",
    "Id":"PHP", // You need to save this - to use in future
    "Nonce":"cJUFe18NSThQ/0yAqZMaDA==",
    "RequestType":"associate",
    "Success":true,
    "Verifier":"ChH0PtuQWP4UKTPhdP3XSgwFyVdekHmHT7YdL1EKA+A=",
    "Version":"1.8.4.1",
    "objectName":""
}
```