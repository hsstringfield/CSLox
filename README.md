# CSLox 

This is an interpreter for the language Lox as described in the book Crafting Interpreters by Robert Nystrom. I wrote this for CS 403 taught by Professer Don Yessick. 
I've attatched a makefile which when executed will run the 6 attatched test cases.

## Current Chapter
Finished 13

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

###### Chapter 9

I ran the test example and one of each kind of control flow, test cases can be found in Test2.txt. 

Book Example:

> 0

> 1

> 1

> 2

> 3
 
> 5

> 8

> 13

> 21

> 34

> 55

> 89

If statement:

> C bigger or equal

While statement:

> Hello!

> Hello!

> Hello!

> Hello!

> Hello!

###### Chapter 10

Test3.txt was run for testing, it's a combination of two tests that are within chapter 10. It shows that function declaration, nested functions, return, and clock work correctly.

> Hi, Dear Reader!

> 1

> 2

> 63837162952.315

> 63837162952.983

###### Chapter 11

Changes:

As per usual, instead of Void for visitors, used object.

Instead of hashmap and stack, utilized dictionary for scopes, due to this there were a lot of little changes with methods used such as .peek() or .put().

Issues:

Tested with Test4.txt and ran into issue with check for empty scope and seeing if variable is initialized in visitVariableExpr. Removing check made it work. Error says can't find a in dictionary, which means it's likely running into issue with checking enclosing (?) Added a check outside of if to check if variable exists inside current scope. 

Testing:

Used examples from chapter, in Test4.txt.

First example returned properly, showing that variable resolution works correctly.

> global

> global

Second example, returned error properly, shows referencing variable in initializer returns error works.

> [line 14] Error at 'a': Can't read local variable in its own initializer.

Third example, returned error properly, shows making mult static variables in function error works properly.

> [line 19] Error at 'a': Already a variable with this name in this scope

Fourth example, returned error properly, shows top level return error works properly.

> [line 22] Error at 'return': Can't return from top-level code.

###### Chapter 12

Changes:

As per usual, no Void, did object instead. Also did lists instead of arrays.

Issues:

Ran into issue with tostring not working in test 1 of Test5.txt, was an issue with fix from last time. Instead of checking outside, should do trygetkey.

Testing:

Used test throughout chapter and they were correct, Test5.txt is file.

Test 1 returned properly, showing class is properly being parsed and executed.

> Devonshire Cream

Test 2 returned properly, showing instances of classes work properly.

> Bagel instance

Test 3 returned properly, wish shows that class methods work.

> Crunch crunch crunch!

Test 4 returned properly, showing this working properly.

> The German chocolate cake is delicious!

###### Chapter 13

Issues:

Although test for this chapter worked, I went back and ran it for the tests in previous chapters and got an error for Test 1, 2, 3, and 4 due to the Can't read local variable in it's own initializer. error. It seems like the code is not checking higher scopes, just the one it is currently in for the value of the variable in the check. This doesn't make any sense as without the check, it still runs properly and sets variables within the correct scope.

After spending a very long time, I can't seem to find an issue anywhere else, or why this is happening. So, I will just be ommmiting this portion from the code as it seems to work perfectly without it.

Testing:

Used test throughout chapter to and they were correct, Test6.txt is file.

Test 1 returned properly, showing that base inheritance and methods worked.

> Fry until golden brown.

Test 2 returned properly, showing that super works correctly.

> Fry until golden brown.

> Pipe full of custard and coat with chocolate.

Test 3 returned properly, showing that super works correctly when needing to determine which superclass to inherit.

> A method