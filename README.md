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

## Crisp Is Lazy
Importantly, Crisp is lazily evaluated. Any values you bind in `let` or `letrec` blocks aren't actually evaluated until they're used in your executing code. Let's take a look back at our example:

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
```

The call to the file-get-text function only happens when we actually need its value, so nothing is read from disk until the evaluator hits line 12. If we comment out the line that actually requires the value of contents and replace it with a simple addition, for example:

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
		;; We've removed the part where we write the file contents!
		(add 1 2)
		(contents . (file-get-text path))))
```

Now, no data is actually read from disk because the expression bound to the contents symbol is never evaluated. This is extrememly important to bear in mind when your code is designed to produce side-effects (for example, writing a record to a database or a text file to disk).

## Binding Values & Applying Functions
To apply a function, we write a list with the symbol bound to the function as its first element, followed by any parameters we want to pass to the function. For example, in the following program we create a lambda that will increment any given number by 1, then we bind it to the symbol increment. In the body of the let block, we make a call to the lambda bound to increment, passing in the number that the user specified on the command line:

```lisp
;; Crisp example program.
;; Author: Saul Johnson
;; Notes: Increments a number by 1 using an 'increment' function.

(lambda (x) 
	(let 
		;; Call the increment function with the value passed to the program.
		(increment x) 
		;; Bind a lambda to the 'increment' symbol that will return the value
		;; of its paramater plus 1.
		(increment . (lambda (n) (add n 1))))) 
```

Now if we run this program on the command line using `crisp.exe -f "increment.csp" -a 5` we'll get the number 6 passed back as the result; we just incremented a number! Now head over to the getting started page to write your first Crisp program!

**Please remember:** This project is far from complete. Both the collection of special forms and the standard library are very minimal and nothing has been tested for potential security vulnerabilities. Any special forms you create yourself would be extremely welcome as part of this project if you'd like to contribute them!
