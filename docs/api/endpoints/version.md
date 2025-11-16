---
layout: default
title: version
parent: Endpoints
nav_order: 9
---

# version
{: .d-inline-block }
GET
{: .label .label-green }

## Overview
Returns the **KeePassHttp** plugin identifier and its current version.

# Request

Path: `/version`

Optional query: `format=`
- `keepass` returns the colon-delimited 3-line payload - KeePass Plugin format.
- `empty` or missing returns plain version text only.

{: .highlight }
No body

Examples:
```bash
# Plain text (default)
curl -s http://localhost:19455/version

# KeePass plugin format
curl -s http://localhost:19455/version?format=keepass
```

# Response

### Successful (HTTP 200)

KeePass plugin format (`format=keepass`):
```text
:
KeePassHttp:2.0.0.0
:
```

Plain (default / `format=empty` / missing):
```text
2.0.0.0
```