---
layout: default
title: Listener Configuration
parent: Configuration
nav_order: 1
has_toc: false
---

# Listener Configuration

This page explains how the KeePassHttp listener endpoints are configured, the secure defaults, and safe ways to enable encryption or limited remote access. It is intended for local workstation, host↔VM, or tightly controlled lab use—not public exposure.

If you cannot meet the constraints described here, do not enable broader access. Proceed only after reviewing the security recommendations below.

## Default Settings

By default, the application runs with the following configuration:

- **HTTP listener** bound to `localhost` on port `19455`
- **HTTPS listener** disabled

No additional setup is required for local usage.

## Local HTTPS (Trusted Self-Signed Certificate)

If you want encrypted communication while keeping usage strictly local (e.g., browser extension → **KeePassHttp** on the same machine), you can enable HTTPS with a locally trusted self-signed certificate. This improves security (no plaintext) without exposing anything publicly.

Use this only for:
- Local machine access (same OS user or same host).
- Host ↔ VM scenarios on a controlled network segment.
- Development/testing where certificate warnings must be eliminated.

Do not treat this as production hardening for public exposure; for anything remote prefer tunneling (SSH / VPN).

### High-Level Steps

1. Run the provided setup script as Administrator: [**Local HTTPS setup script**](https://raw.githubusercontent.com/alan-null/keepasshttp/refs/heads/master/scripts/enable-local-https.ps1).
2. The script will:
   - Create (or reuse) a self-signed cert for `localhost`.
   - Add it to **LocalMachine\My** and trust it (**Roots**).
   - Bind the cert to the chosen HTTPS port (default: 19456).
3. In your KeePassHttp configuration, enable the HTTPS listener on that port (keeping HTTP on localhost only or disabling it if not needed). See [`Activate HTTPS Listener`](./../configuration.md#-activate-https-listener) for details.
4. Update any client/extension settings to use: `https://localhost:19456/`
5. Test in a browser to confirm no certificate warning appears.
6. Keep firewall rules restricted (loopback / host-only). Do not open this port publicly.

### Validation Checklist

- Certificate shows as trusted (no browser warning).
- HTTP (plaintext) either disabled or not exposed beyond localhost.
- No external interface binding (avoid 0.0.0.0 unless strictly required internally).
- Remote access still governed by earlier tunneling guidance.

### When Not to Proceed

If you cannot keep usage local, or must expose beyond trusted boundaries, prefer SSH/VPN tunneling instead of relying solely on this HTTPS setup.


## Considerations for Remote Access

Enabling remote access requires careful planning and a clear understanding of the risks. Before making any changes, review these recommendations:

- **Avoid exposing HTTP over public networks.** The HTTP listener does not provide encryption and should never be used over the internet or any untrusted connection.
- **Virtual machine use cases.** A common scenario is running KeePass inside a VM (VirtualBox, Parallels, etc.) and accessing it from the host system. This can be acceptable, but you should:
  - Limit firewall rules to the minimum necessary (e.g., allow only the host machine).
  - Prefer using HTTPS even in this environment.
- **Public access is strongly discouraged.** Running the service over an open or public connection significantly increases your exposure to attacks.
- **Preferred approach: use a secure tunnel.** If remote access is required, an SSH tunnel with TCP port forwarding is the recommended solution. With a tunnel, the default listener configuration is usually sufficient.
- **Alternative: VPN tunneling.** If SSH tunneling is not possible, use a trusted VPN connection to secure the traffic.
- **Last resort: HTTPS with client certificate authentication.** This provides strong security but may not be fully supported in all setups.
- If none of the above options are feasible, **remote access should not be enabled.**

Once you fully understand the implications and have chosen an appropriate approach, continue with the configuration steps below.