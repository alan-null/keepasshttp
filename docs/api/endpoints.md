---
layout: default
title: Endpoints
nav_order: 1
parent: API
toc_title: "Endpoints:"
---

# Endpoints

Overview of available endpoints exposed by **KeePassHttp** over `HttpListener`.

### Common pattern
1. Associate once (user confirms).
2. For each request: generate `Nonce` (16 bytes), set `Verifier` = AES-CBC(Key, IV=Nonce, NoncePlaintext).
3. Encrypt all sensitive fields with same Key + Nonce (AES-CBC, PKCS7).