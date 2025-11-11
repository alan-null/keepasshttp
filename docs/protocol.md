---
layout: default
title: Protocol
nav_order: 6
has_toc: false
---

# Protocol

## A. New or stale client (key absent)

This is the only phase where a network observer could capture the raw AES key (no prior shared secret). Minimize exposure by performing association locally.

1. client sends "test-associate" with payload to server
2. server sends fail response to client (cannot decrypt)
3. client sends "associate" with 256bit AES key and payload to server
4. server decrypts payload with provided key and prompts user to save
5. server saves key into "KeePassHttpSettings":"AES key: label"
6. client saves label/key into local password storage

(1) can be skipped if client does not have a key configured

### B. Client with key stored in server
1. client sends "test-associate" with label+encrypted payload to server
2. server verifies payload and responds with success to client
3. client sends any of "get-logins-count", "get-logins", "set-login" using the previously negotiated key in (A)
4. if any subsequent request fails, it is necessary to "test-associate" again
