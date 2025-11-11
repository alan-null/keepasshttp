---
layout: default
title: Troubleshooting
nav_order: 4
has_toc: false
---

# Troubleshooting

__First:__ If an error occures it will be shown as notification in system tray or as message box in KeePass.

Otherwise please check if it could be an error of the client you are using. For passIFox and chromeIPass you can [report an error here](https://github.com/alan-null/chromeIPass/issues/).


If you are having problems with KeePassHttp, please tell us at least the following information:
* version of KeePass
* version of KeePassHttp
* error message (if available)
* used clients and their versions
* URLs on which the problem occur (if available)

### HTTP Listener error message

Maybe you get the following error message:
[<img src="https://raw.github.com/alan-null/keepasshttp/master/documentation/images/http-listener-error.png" alt="http listener error" width="300px" />](https://raw.github.com/alan-null/keepasshttp/master/documentation/images/http-listener-error.png)

In old versions the explaining first part of the message does not exist!

This error occurs because you have multiple copies of KeePassHttp in your KeePass directory! Please check __all__ PLGX- and DLL-files in your _KeePass directory and all sub-directories_ whether they are a copy of KeePassHttp.
__Note:__ KeePass does _not_ detect plugins by filename but by extension! If you rename _KeePassHttp.plgx_ to _HelloWorld.plgx_ it is still a valid copy of KeePassHttp.

If you _really_ have only one copy of KeePassHttp in your KeePass directory another application seems to use port 19455 to wait for signals. In this case try to stop all applications and restart everyone again while checking if the error still occurs.

## URL matching: How does it work?

KeePassHttp can receive 2 different URLs, called URL and SubmitURL.

CompareToUrl = SubmitURL if set, URL otherwise

For every entry, the [Levenshtein Distance](http://en.wikipedia.org/wiki/Levenshtein_distance) of his Entry-URL (or Title, if Entry-URL is not set) to the CompareToURL is calculated.

Only the Entries with the minimal distance are returned.

###Example:
Submit-Url: http://www.host.com/subdomain1/login

| Entry-URL                      | Distance |
| ------------------------------ | -------- |
| http://www.host.com/           | 16       |
| http://www.host.com/subdomain1 | 6        |
| http://www.host.com/subdomain2 | 7        |

__Result:__ second entry is returned