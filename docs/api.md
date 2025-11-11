---
layout: default
title: API
nav_order: 7
has_toc: false
---

# API

Requests are JSON objects sent via HTTP POST to `http://localhost:19455/` with `Content-Type: application/json`. The cipher used for request field encryption is AES‑256‑CBC (32‑byte key). Fields listed below are unencrypted at transport level (HTTP); specific values inside requests are encrypted using the negotiated key + per‑request nonce.

### Generic HTTP request
(based on packet sniffing and code analyssis)
Generic HTTP request is json sent in POST message. Cipher, by means of OpenSSL library is `AES-256-CBC`, so key is 32 byte long.

```bash
Host: localhost:19455
Connection: keep-alive
Content-Length: 54
Content-Type: application/json
Accept: */*
Accept-Encoding: gzip, deflate, br

{"RequestType":"test-associate","TriggerUnlock":false}
```

Also, minimal JSON request (except that one without key set up) consists of four main parameters:
 - RequestType - `test-associate`, `associate`, `get-logins`, `get-logins-count`, `set-login`, ...
 - TriggerUnlock - TODO: what is this good for? seems always false
 - Nonce - 128 bit (16 bytes) long random vector, base64 encoded, used as IV for aes encryption
 - Verifier - verifier, base64 encoded AES encrypted data: `encrypt(base64_encode($nonce), $key, $nonce);`
 - Id - Key id entered into KeePass GUI while  `associate`, not used during `associate`
