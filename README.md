# Crisp
_Purity in Coding_

What is Crisp?
Crisp is a dialect of Lisp with absolutely minimal syntax designed and optimised for use in creating web applications. A purely functional language, it's extremely easy to write efficient, clean and maintainable programs and Crisp's interpreted nature means changing your code is a simple as changing a source file.

What's more, Crisp is easy to understand. Take a look at the following program:

```lisp
;; Crisp example program.
;; Author: Saul Johnson
;; Notes: Replaces every occurrence of 'foo' in the document in the file 
;; specified by the 'input' parameter and writes the result to the file 
;; specified by the 'output' parameter.

;; Every program is a single lambda (i.e. function).
(lambda (input output) 
	;; The let block allows you to bind symbols to expressions (i.e. declare variables).
	(let 
		;; Here we actually write the file contents.
		(file-set-text output (replace contents "foo" "bar")) 
		(contents . (file-get-text path))))
From taking a glance over that source code, we can see that:
```

Crisp is purely functional - unlike programming languages such as C# and PHP, Crisp programs are composed from function applications rather than classes and methods.

A Crisp program is a "lambda" (i.e. a function) that can take parameters. These parameters can be passed to the program via the command line: 

```
crisp.exe -f example.csp -a "\"foo.txt\" \"bar.txt\""
```

**Please remember:** This project is far from complete. Both the collection of special forms and the standard library are very minimal and nothing has been tested for potential security vulnerabilities. Any special forms you create yourself would be extremely welcome as part of this project if you'd like to contribute them!
