---
layout: default
title: Security
nav_order: 5
has_toc: false
---

# Security

For security reasons KeePassHttp communicates only with the symmetric-key algorithm AES.
The entries are crypted with a 256bit AES key.

There is one single point where someone else will be able to steal the encryption keys.
If a new client has to connect to KeePassHttp, the encryption key is generated and send to KeyPassHttp via an unencrypted connection.