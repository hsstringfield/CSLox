# CSLox 

This is an interpreter for the language Lox as described in the book Crafting Interpreters by Robert Nystrom. I wrote this for CS 403 taught by Professer Don Yessick. 

## Current Chapter
Finished 5

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

> (){},.-+;*!=!<=<>=>identifier===/hello hello hello/\r\t\n"Hello"14&@$

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