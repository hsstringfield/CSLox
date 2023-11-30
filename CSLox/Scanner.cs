// File for scanner class, as described in 4.4



using System;
using System.IO;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Scanner class, 4.4
    /// </summary>
    public class Scanner{

        private readonly string source;
        private readonly List<Token> tokens = new List<Token>(); 
        private int start = 0;
        private int current = 0;
        private int line =1; 



        /// <summary>
        /// Dictionary for determining keywords, 4.7
        /// </summary>
        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>{
            {"and",         TokenType.AND},
            {"class",       TokenType.CLASS},
            {"else",        TokenType.ELSE},
            {"false",       TokenType.FALSE},
            {"for",         TokenType.FOR},
            {"fun",         TokenType.FUN},
            {"if",          TokenType.IF},
            {"nil",         TokenType.NIL},
            {"or",          TokenType.OR},
            {"print",       TokenType.PRINT},
            {"return",      TokenType.RETURN},
            {"super",       TokenType.SUPER},
            {"this",        TokenType.THIS},
            {"true",        TokenType.TRUE},
            {"var",         TokenType.VAR},
            {"while",       TokenType.WHILE},
        };


        /// <summary>
        /// Constructor for scanner, as described in 4.4
        /// </summary>
        /// <param name="source"></param>
        public Scanner(string source){

            this.source = source;

        }



        /// <summary>
        /// Scan through tokens in input, as described in 4.4
        /// </summary>
        /// <returns></returns>
        public List<Token> scanTokens(){
            
            // goes through until end
            while(!isAtEnd()){
                start = current; 
                scanToken();
            }

            // add tokens, return
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens; 

        }



        /// <summary>
        /// Method to determine token, as described in 4.5
        /// </summary>
        private void scanToken(){
            
            char c = advance();
            
            switch(c){

                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.STAR); break;

                // operators, 4.5.2
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL    : TokenType.BANG); 
                    break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL   : TokenType.EQUAL); 
                    break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL    : TokenType.LESS); 
                    break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); 
                    break;

                // more detailed slash, 4.6
                case '/': 
                    if(match('/')){
                        while(peek() != '\n' && !isAtEnd()) advance();
                    }else{
                        addToken(TokenType.SLASH);
                    }
                    break;

                // whitespace and newlines, 4.6
                case ' ':
                case '\r':
                case '\t': break;
                case '\n': line++; break;

                // strings, 4.6.1
                case '"': String(); break;

                // default for unexpected, 4.5.1
                default:
                    // added support for num literals, 4.6.2
                    if(isDigit(c)){
                        number(); 
                    // added support for detecting alphabetic, 4.7
                    }else if(isAlpha(c)){
                        identifier();
                    }else{
                        Lox.error(line, "Unexpected character.");
                    }   
                    break;

            }
        }



        /// <summary>
        /// Identifier handler, 4.7
        /// </summary>
        private void identifier(){
            
            // get full identifier
            while(isAlphaNumeric(peek())) advance();

            // check for keyword
            string text = source.Substring(start, current - start);
            TokenType type; 
            if(!keywords.TryGetValue(text, out type)){
                type = TokenType.IDENTIFIER;
            }

            // add token
            addToken(type);

        }



        /// <summary>
        /// Method for dealing with numbers, 4.6.2
        /// </summary>
        private void number(){
            
            // loop to look for numbers longer than one char
            while(isDigit(peek())) advance();

            // check for decimal
            if(peek() == '.' && isDigit(peekNext())){

                advance();
                while(isDigit(peek())) advance();
            
            }

            // add token
            addToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start)));

        }



        /// <summary>
        /// Method for dealing with strings, 4.6.1
        /// </summary>
        private void String(){
            
            // peek until string ends or file ends
            while(peek() != '"' && !isAtEnd()){

                if(peek() == '\n') line++;
                advance();

            }

            // if file ends, unterminated string
            if(isAtEnd()){

                Lox.error(line, "Unterminated string.");
                return;

            }

            // continue onto next char
            advance();

            // put full string into literal and add token
            string value = source.Substring(start + 1, (current - start) - 2);
            addToken(TokenType.STRING, value);

        }



        /// <summary>
        /// Method for determining if is character match, 4.5.2
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        private bool match(char expected){

            // check if is at end or matches
            if(isAtEnd()) return false; 
            if(source[current] != expected) return false; 

            // returns if not
            current++; 
            return true;

        }



        /// <summary>
        /// Method to peek to next char, 4.6
        /// </summary>
        /// <returns></returns>
        private char peek(){
            
            if(isAtEnd()) return '\0';
            return source[current];

        }



        /// <summary>
        /// Method to peek to extra char, 4.6.2
        /// </summary>
        /// <returns></returns>
        private char peekNext(){

            if(current + 1 >= source.Length) return '\0';
            return source[current + 1];

        }



        /// <summary>
        /// Determines if is a valid input for an identifier, 4.7
        /// </summary>
        /// <returns></returns>
        private bool isAlpha(char c){

            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';

        }




        private bool isAlphaNumeric(char c){

            return isAlpha(c) || isDigit(c);

        }



        /// <summary>
        /// Determins if char is digit, 4.6.2
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool isDigit(char c){

            return c >= '0' && c <= '9';

        }



        /// <summary>
        /// Helper function that determines if is at end, as described in 4.4
        /// </summary>
        /// <returns></returns>
        private bool isAtEnd(){

            return current >= source.Length;

        }



        /// <summary>
        /// Advance through chars, as described in 4.5
        /// </summary>
        /// <returns></returns>
        private char advance(){

            return source[current++];

        }



        /// <summary>
        /// addToken helper method for those without literals, 4.5
        /// </summary>
        /// <param name="type"></param>
        private  void addToken(TokenType type){
            
            addToken(type, null);

        }



        /// <summary>
        /// Adds token for current text, 4.5
        /// </summary>
        /// <param name="type"></param>
        /// <param name="literal"></param>
        private void addToken(TokenType type, object literal){

            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));

        }





    }
}