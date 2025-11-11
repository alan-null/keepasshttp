---
layout: default
title: Installation
nav_order: 2
has_toc: false
---

# Installation

## Windows installation using Chocolatey

 1. Install using [Chocolatey](https://chocolatey.org/) with `choco install keepass-keepasshttp`
 2. Restart KeePass if it is currently running to load the plugin

## Non-Windows / Manual Windows installation

 1. Download [KeePassHttp](https://github.com/alan-null/keepasshttp/releases)
 2. Copy it into the KeePass directory
	* default directory in Ubuntu14.04: /usr/lib/keepass2/
	* default directory in Arch: /usr/share/keepass
 3. Set chmod 644 on file `KeePassHttp.plgx`
 4. On linux systems you maybe need to install mono-complete: `$ apt-get install mono-complete` (in Debian it should be enough to install the packages libmono-system-runtime-serialization4.0-cil and libmono-posix2.0-cil)
 * Tips to run KeePassHttp on lastest KeePass 2.31: install packages
 	`sudo apt-get install libmono-system-xml-linq4.0-cil libmono-system-data-datasetextensions4.0-cil libmono-system-runtime-serialization4.0-cil mono-mcs`
 5. Restart KeePass

### KeePassHttp on Linux and Mac

KeePass needs Mono. You can find detailed [installation instructions on the official page of KeePass](http://keepass.info/help/v2/setup.html#mono).

Perry has tested KeePassHttp with Mono 2.6.7 and it appears to work well.
With Mono 2.6.7 and a version of KeePass lower than 2.20 he could not get the plgx file to work on linux.
If the plgx file does also not work for you, you can try the two DLL files KeePassHttp.dll and Newtonsoft.Json.dll from directory [mono](https://github.com/alan-null/keepasshttp/tree/master/mono) which should work for you.

With newer versions of Mono and KeePass it seems that the plgx file works pretty fine.
More information about it are contained in the following experience report.