---
layout: default
title: Tips and Tricks
nav_order: 4
has_toc: false
---

# Tips and Tricks

### Support multiple URLs for one username + password
This is already implemented directly in KeePass.

1. Open the context menu of an entry by clicking right on it and select _Duplicate entry_:
[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/keepass-context-menu.png" alt="context-menu-entry" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/keepass-context-menu.png)

2. Check the option to use references for username and password:
[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/keepass-duplicate-entry-references.png" alt="mark checkbox references" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/keepass-duplicate-entry-references.png)

3. You can change the title, URL and everything of the copied entry, but not the username and password. These fields contain a _Reference Key_ which refers to the _master entry_ you copied from.
