---
layout: default
title: set-login
parent: API Examples
nav_order: 5
---

# set-login

Request:
```javascript
{
    "RequestType":"set-login",
    "Id":"PHP",
    "Nonce":"VBrPACEOQGxIBkq58/5Xig==",
    "Verifier":"1dT0gnw6I1emxDzhtYn1Ecn1sobLG98GfTf7Z/Ma0R0=",
    "Login":"lm9qo5HcAYEIaHsCdSsYHQ==", // encrypted username
    "Password":"EZLtRxFgZVqIwv5xI9tfvA==", // encrypted password
    "Url":"<snip>",
    "SubmitUrl":"<snip>"
}
```

Response:
```javascript
{
    "Count":null,
    "Entries":null,
    "Error":"",
    "Hash":"d8312a59523d3c37d6a5401d3cfddd077e194680",
    "Id":"PHP",
    "Nonce":"uofAcMtnPQo5TOdI21VjBw==",
    "RequestType":"set-login",
    "Success":true,
    "Verifier":"4u8OINVGBtlCCPY7OnW5T616iPlzvf56LzPtPAwZIs0=",
    "Version":"1.8.4.1",
    "objectName":""
}
```
