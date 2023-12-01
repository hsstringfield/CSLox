# CSLox 

This is an interpreter for the language Lox as described in the book Crafting Interpreters by Robert Nystrom. I wrote this for CS 403 taught by Professer Don Yessick. 

## Current Chapter
Finished 8

## Notes

I commented the sections each bit of code was from as well as its general purpose. I didn't clarify how simple methods worked, but more complex methods should be well commented. 

## Testing

###### Chapter 4

I changed what was in the book to be :

console.Write("\n")

foreach(Token token in tokens){

    Console.Write(token.toString() + "\n");

}

I ran several test cases that all worked. For the sake of example here's one that includes all types and its output:

(){},.-+;*!=!<=<>=>identifier===/hello hello hello/\r\t\n"Hello"14&@$

>[line 1] Error : Unexpected character.[line 1] Error : Unexpected character.[line 1] Error : Unexpected character.[line 1] 

>Error : Unexpected character.[line 1] Error : Unexpected character.[line 1] Error : Unexpected character.

>LEFT_PAREN ( 

>RIGHT_PAREN ) 

>LEFT_BRACE { 

>RIGHT_BRACE } 

>COMMA , 

>DOT . 

>MINUS - 

>PLUS + 

>SEMICOLON ; 

>STAR * 

>BANG_EQUAL != 

>BANG ! 

>LESS_EQUAL <= 

>LESS < 

>GREATER_EQUAL >= 

>GREATER > 

>IDENTIFIER identifier 

>EQUAL_EQUAL == 

>EQUAL = 

>SLASH / 

>(3)IDENTIFIER hello 

>SLASH / 

>IDENTIFIER r 

>IDENTIFIER t 

>IDENTIFIER n 

>STRING "Hello" Hello

>NUMBER 14 14

>EOF  

###### Chapter 5

Using the main method provided (at the bottom of AstPrinter.cs), I can see that my code runs correctly. As it outputs:

>(* (- 123) (group 45.67))

###### Chapter 6

Tested operators with lengthy input and various operators with parse method as described at end of chapter 6.

1 != 2 == 3 > 4 >= 5 < 6 <= 7 - 8 + 9 / 10 * true ! false + nil

> (== (!= 1 2) (<= (< (>= (> 3 4) 5) 6)) (+ (- 7 8) (* (/ 9 10) True)))

! false

> (! False)

nil + 2

> (+ nil 2)

1 < 2 < 3 < 4

> (< (< (< 1 2) 3) 4)

###### Chapter 7

Tested operators properly showing values and grouping:

(1 + (2 * 3)) / (-1 - -2 * 4)
Order of operations, parenthetical grouping, addition, subtraction, multiplication, division, negative nums working

> 1

All comparisons tested.

1 == 1

> True

1 == 2

> False

2 > 1

> True

1 > 2

> False

2 >= 1

> True

2 >= 2

> True

1 >= 2

> False

1 < 2

> True

2 < 1

> False

1<=2

> True

1 <= 1

> True

2 <= 1

> False

1 != 2

> True

1 != 1

> False

String Concatenation:

"hello" + " world"

> hello world

Errors:

-

> [line 1] Error at end: Expect expression.

!

> [line 1] Error at end: Expect expression.

1 + 

> [line 1] Error at end: Expect expression.

1 -

> [line 1] Error at end: Expect expression.

1 *

> [line 1] Error at end: Expect expression.

1 /

> [line 1] Error at end: Expect expression.

1 <

> [line 1] Error at end: Expect expression.

1 <=

> [line 1] Error at end: Expect expression.

1 >

> [line 1] Error at end: Expect expression.

1 >=

> [line 1] Error at end: Expect expression.

(1 + 2

> [line 1] Error at end: Expect ')' after expression.

###### Chapter 8

I ran the test example that was in the book (titled Test1.txt) and it returned correctly:

> inner a

> outer b

> global c

> outer a

> outer b

> global c

> global a

> global b

> globabl c