# Crisp
_Purity in Coding_

![Crisp logo](https://raw.githubusercontent.com/lambdacasserole/crisp/master/Assets/logo_128.png)

Crisp is a dialect of Lisp with absolutely minimal syntax designed and optimised for use in creating web applications. A purely functional language, it's extremely easy to write efficient, clean and maintainable programs and Crisp's interpreted nature means changing your code is a simple as changing a source file.

Run Crisp on Packet - a webserver optimised for serving dynamic webpages generated in Crisp. Add data persistence with the included Crisp.Data assembly which handily wraps a 100% managed port of SQLite.

This repository contains a Visual Studio 2015 solution containing both the Crisp interpreter and the Packet webserver, plus bundled special forms and a small standard library.

**Please remember:** This project is far from complete. Both the collection of special forms and the standard library are very minimal and nothing has been tested for potential security vulnerabilities. Any special forms you create yourself would be extremely welcome as part of this project if you'd like to contribute them!
