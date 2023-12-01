// File for the parser class



using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Lox{



    /// <summary>
    /// Class for parser, section 6.2.1
    /// </summary>
    public class Parser{

        private class ParseError : Exception {}
        private readonly List<Token> tokens;
        private int current = 0;



        /// <summary>
        /// Constructor for parser, section 6.2.1
        /// </summary>
        /// <param name="tokens"></param>
        public Parser(List<Token> tokens){

            this.tokens = tokens;

        }



        /* Temporary Parse method for ch 6 + 7
        /// <summary>
        /// Parse method, 6.4
        /// </summary>
        /// <returns></returns>
        public Expr parse(){

            
            try{
                return expression();
            }catch(ParseError error){
                return null;
            }
            

        }
        */



        /// <summary>
        /// Method to parse through code, 8.1.2
        /// </summary>
        /// <returns></returns>
        public List<Stmt> parse(){

            // create list of statements
            List<Stmt> statements = new List<Stmt>();

            // iterate through until end
            while(!isAtEnd()){

                statements.Add(declaration());

            }

            return statements;

        }



        /// <summary>
        /// Helper method for expressions, goes to equality, section 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr expression(){

            // changed to assignment, 8.4.1
            return assignment();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Stmt declaration(){

            try{
                if(match(TokenType.VAR)) return varDeclaration();
                return statement();
            }catch (ParseError error){
                synchronize();
                return null;
            }

        }
    


        /// <summary>
        /// Determines proper statement and redirects, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Stmt statement(){

            if(match(TokenType.PRINT)) return printStatement();
            if(match(TokenType.LEFT_BRACE)) return new Stmt.Block(block());
            return expressionStatement();

        }



        /// <summary>
        /// Method for printing, checks for ;, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Stmt printStatement(){

            Expr value = expression();
            consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Stmt.Print(value);

        }



        /// <summary>
        /// Method for declaration statements, 8.2.2
        /// </summary>
        /// <returns></returns>
        private Stmt varDeclaration(){

            Token name = consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if(match(TokenType.EQUAL)){
                initializer = expression();
            }

            consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);

        }



        /// <summary>
        /// Method for statements, checks for;, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Stmt expressionStatement(){

            Expr expr = expression();
            consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new Stmt.Expression(expr);

        }



        /// <summary>
        /// Method for dealing with blocks in code, 8.5.2
        /// </summary>
        /// <returns></returns>
        private List<Stmt> block(){

            List<Stmt> statements = new List<Stmt>();

            while(!check(TokenType.RIGHT_BRACE) && !isAtEnd()){

                statements.Add(declaration());

            }

            consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;

        }



        /// <summary>
        /// Memthod for assignment, 8.4.1
        /// </summary>
        /// <returns></returns>
        private Expr assignment(){

            Expr expr = equality();

            if(match(TokenType.EQUAL)){

                Token equals = previous();
                Expr value = assignment();

                if(expr is Expr.Variable){

                    Token name = ((Expr.Variable) expr).name;
                    return new Expr.Assign(name, value);

                }

                error(equals, "Invalid assignment target.");

            }

            return expr;

        }



        /// <summary>
        /// Method for equality in grammar, section 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr equality(){

            Expr expr = comparison();

            // check according to grammar in book, 6.2.1
            while(match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)){
                Token oper = previous();
                Expr right = comparison();
                expr = new Expr.Binary(expr, oper, right);
            }

            return expr;

        }



        /// <summary>
        /// Method for comparison in grammar, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr comparison(){

            Expr expr = term();

            while(match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)){

                Token oper = previous();
                Expr right = term(); 
                expr = new Expr.Binary(expr, oper, right);

            }

            return expr;

        }



        /// <summary>
        /// Method for factor in grammar, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr term(){

            Expr expr = factor();

            while(match(TokenType.MINUS, TokenType.PLUS)){

                Token oper = previous();
                Expr right = factor();
                expr = new Expr.Binary(expr, oper, right);

            }

            return expr;

        }



        /// <summary>
        /// Method for factor in grammar, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr factor(){

            Expr expr = unary();

            while(match(TokenType.SLASH, TokenType.STAR)){

                Token oper = previous();
                Expr right = unary();
                expr = new Expr.Binary(expr, oper, right);

            }

            return expr;

        }



        /// <summary>
        /// Method for unary in grammar, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr unary(){

            if(match(TokenType.BANG, TokenType.MINUS)){

                Token oper = previous();
                Expr right = unary();
                return new Expr.Unary(oper, right);

            }

            return primary();

        }



        /// <summary>
        /// Method for primary in grammar, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Expr primary() {

            if (match(TokenType.FALSE)) return new Expr.Literal(false);
            if (match(TokenType.TRUE)) return new Expr.Literal(true);
            if (match(TokenType.NIL)) return new Expr.Literal(null);

            if (match(TokenType.NUMBER, TokenType.STRING)) {

                return new Expr.Literal(previous().literal);

            }

            // added support for variables 8.2.2
            if(match(TokenType.IDENTIFIER)){

                return new Expr.Variable(previous());

            }

            if (match(TokenType.LEFT_PAREN)) {

                Expr expr = expression();
                consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);

            }

            // throws error if can't start expression, 6.4
            throw error(peek(), "Expect expression.");

        }



        /// <summary>
        /// Method that matches with multiple inputs, section 6.2.1
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private bool match(params TokenType[] types){

            foreach(TokenType type in types){

                if(check(type)){
                    advance();
                    return true;
                }

            }

            return false;

        }



        /// <summary>
        /// Method for dealing with errors, 6.3.2
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Token consume(TokenType type, string message){

            if(check(type)) return advance();

            throw error(peek(), message);

        }



        /// <summary>
        /// Checks current token without consuming, 6.2.1
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool check(TokenType type){

            if(isAtEnd()) return false;
            return peek().type == type;

        }



        /// <summary>
        /// Takes current token and goes to next, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Token advance(){

            if(!isAtEnd()) current++;
            return previous();

        }



        /// <summary>
        /// Check if end is near, 6.2.1
        /// </summary>
        /// <returns></returns>
        private bool isAtEnd(){

            return peek().type == TokenType.EOF;

        }



        /// <summary>
        /// Show current, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Token peek(){

            return tokens[current];

        }



        /// <summary>
        /// Show last token, 6.2.1
        /// </summary>
        /// <returns></returns>
        private Token previous(){

            return tokens[current - 1];

        }



        /// <summary>
        /// Throws error, 6.3.2
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private ParseError error(Token token, string message){
            
            Lox.error(token, message);
            return new ParseError();

        }



        /// <summary>
        /// Synchronize on statements, 6.3.3
        /// </summary>
        private void synchronize(){

            advance();

            while(!isAtEnd()){

                if(previous().type == TokenType.SEMICOLON) return;

                switch(peek().type){
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                advance();

            }
        }
    }
}