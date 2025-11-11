---
layout: default
title: test-associate
parent: API Examples
nav_order: 1
---

# test-associate
Request, without key, seems like initialization of every key assignation session:
```javascript
{
    "RequestType":"test-associate",
    "TriggerUnlock":false
}
```

Response: (without success)
```javascript
{
    "Count":null,
    "Entries":null,
    "Error":"",
    "Hash":"d8312a59523d3c37d6a5401d3cfddd077e194680",
    "Id":"",
    "Nonce":"",
    "RequestType":"test-associate",
    "Success":false,
    "Verifier":"",
    "Version":"1.8.4.1",
    "objectName":""
}
```

If you have key, you can test with request like this:
```javascript
{
    "Nonce":"+bG+EpbCR4jSnjROKAAw4A==", // random 128bit vector, base64 encoded
    "Verifier":"2nVUxyddGpe62WGx5cm3hcb604Xn8AXrYxUK2WP9dU0=", // Nonce in base64 form, encoded with aes
    "RequestType":"test-associate",
    "TriggerUnlock":false,
    "Id":"PHP"
}
```