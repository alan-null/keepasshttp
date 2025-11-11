---
layout: default
title: Configuration
nav_order: 3
has_toc: false
---

# Configuration and Options

KeePassHttp works out-of-the-box. You don't have to explicitely configure it.

 * KeePassHttp stores shared AES encryption keys in "KeePassHttp Settings" in the root group of a password database.
 * Password entries saved by KeePassHttp are stored in a new group named "KeePassHttp Passwords" within the password database.
 * Remembered Allow/Deny settings are stored as JSON in custom string fields within the individual password entry in the database.

### Settings in KeePassHttp options.

You can open the options dialog with menu: Tools > KeePassHttp Options

[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/menu.jpg" alt="menu" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/menu.jpg)

The options dialog will appear:

[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/options-general.png" alt="options-general" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/options-general.png)

General tab

1. show a notification balloon whenever entries are delivered to the inquirer.
2. returns only the best matching entries for the given url, otherwise all entries for a domain are send.
  - e.g. of two entries with the URLs http://example.org and http://example.org/, only the second one will returned if the requested URL is http://example.org/index.html
3. if the active database in KeePass is locked, KeePassHttp sends a request to unlock the database. Now KeePass opens and the user has to enter the master password to unlock the database. Otherwise KeePassHttp tells the inquirer that the database is closed.
4. KeePassHttp returns only these entries which match the scheme of the given URL.
  - given URL: https://example.org --> scheme: https:// --> only entries whose URL starts with https://
5. sort found entries by username or title.
6. removes all shared encryption-keys which are stored in the currently selected database. Every inquirer has to reauthenticate.
7. removes all stored permissions in the entries of the currently selected database.

[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/options-advanced.png" alt="options-advanced" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/options-advanced.png)

Advanced tab

8. KeePassHttp no longer asks for permissions to retrieve entries, it always allows access.
9. KeePassHttp no longer asks for permission to update an entry, it always allows updating them.
10. Searching for entries is no longer restricted to the current active database in KeePass but is extended to all opened databases!
  - __Important:__ Even if another database is not connected with the inquirer, KeePassHttp will search and retrieve entries of all opened databases if the active one is connected to KeePassHttp!
11. if activated KeePassHttp also search for string fields which are defined in the found entries and start with "KPH: " (note the space after colon). __The string fields will be transfered to the client in alphabetical order__. You can set string fields in the tab _Advanced_ of an entry.
[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/advanced-string-fields.png" alt="advanced tab of an entry" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/advanced-string-fields.png)
