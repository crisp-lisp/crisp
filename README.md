# Crisp
_Purity in Coding_

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

## The Language
The Crisp language is extremely simple at its core, using the same representation for code and data. Just like in Common Lisp, Crisp programs are just lists that are evaluated according to a set of rules.

### Types of Expression
Symbolic expressions (or s-expressions) can be of one of several different types in Crisp. An expression is considered atomic if it is not a pair or a function.

| Name     | Atomic? | Examples                      | Notes                                                                                                                                                                                                                                |
|----------|---------|-------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Boolean  | Yes     | `true false`                  | The `true` or `false` keywords evaluate to boolean atoms in Crisp.                                                                                                                                                                   |
| Constant | Yes     | `(quote my-constant)`         | In Crisp, calling the `quote` special form on a symbol or list will return it as a data structure instead of evaluating it (that is, with symbols converted to constants).                                                           |
| Lambda   | No      | `(lambda (x) (add x 1))`      | The `lambda` function can be used to create a callable closure. For example, if the lambda on the left was bound to a symbol `increment` then calling `(increment 3)` would return `4`.                                              |
| Nil      | Yes     | `nil`                         | The `nil` keyword represents a null value in Crisp.                                                                                                                                                                                  |
| Numeric  | Yes     | `9 12.358 -5`                 | Numeric atoms are just numbers. They have the equivalent precision of a `double` in C#.                                                                                                                                              |
| Pair     | No      | `(1 . 2) ("hello" . "world")` | A pair is the basic building block of a list. It's the simplest data structure possible, consisting of a pair of expressions bound together in a _cons_ cell. Read more about it on [Wikipedia](https://en.wikipedia.org/wiki/Cons). |
| String   | Yes     | `"hello" "world"`             | A string represents a sequence of characters. Even though it is possible to manipulate strings in Crisp, they are immutable and considered to be atoms.                                                                              |

**Please remember:** This project is far from complete. Both the collection of special forms and the standard library are very minimal and nothing has been tested for potential security vulnerabilities. Any special forms you create yourself would be extremely welcome as part of this project if you'd like to contribute them!
