// File for interpreter class as described in 7.2



using System;
using System.Collections.Generic;

namespace Lox{

    // added Stmt.Visitor, 8.1.3
    /// <summary>
    /// Interpreter class to interpret code, 7.2
    /// </summary>
    public class Interpreter : Expr.Visitor<object>, Stmt.Visitor<object>{

        private Environment environment = new Environment();

        /* interpret for Ch 7
        /// <summary>
        /// API for interpreter, 7.4
        /// </summary>
        /// <param name="expression"></param>
        public void interpret(Expr expression){

            try{
                object value = evaluate(expression);
                Console.Write(stringify(value));
            }catch(RuntimeError error){
                Lox.runtimeError(error);
            }

        }
        */



        /// <summary>
        /// API for interpreter, changed to execute list of statements, 8.1.3
        /// </summary>
        /// <param name="expression"></param>
        public void interpret(List<Stmt> statements){

            try{
                foreach(Stmt statement in statements){
                    execute(statement);
                }
            }catch(RuntimeError error){
                Lox.runtimeError(error);
            }

        }



        /// <summary>
        /// Behavior for visiting literal, return literal, 7.2.1
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitLiteralExpr(Expr.Literal expr){

            return expr.value;

        }



        /// <summary>
        /// Behavior for visiting parenthetical, evaluate inside, 7.2.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitGroupingExpr(Expr.Grouping expr){

            return evaluate(expr.expression);

        }



        /// <summary>
        /// Behavior for unary, 7.2.3
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitUnaryExpr(Expr.Unary expr){

            // set value
            object right = evaluate(expr.right);

            // determine oper and apply
            switch(expr.oper.type){
                case TokenType.MINUS:
                    // check operand for error, 7.3.1
                    checkNumberOperand(expr.oper, right);
                    return -(double)right;
                case TokenType.BANG:
                    return !isTruthy(right);
            }

            // unreachable
            return null;

        }



        /// <summary>
        /// Behavior for variable expr, 8.3.1
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitVariableExpr(Expr.Variable expr){

            return environment.get(expr.name);
        
        }



        /// <summary>
        /// Method to check operand for error, 7.3.1
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="operand"></param>
        private static void checkNumberOperand(Token oper, object operand){

            if(operand is double) return;
            throw new RuntimeError(oper, "Operand must be a number.");

        }



        /// <summary>
        /// Method to check multiple multiple operands for 
        /// </summary>
        /// <param name="oepr"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void checkNumberOperands(Token oper, object left, object right){

            if(left is double && right is double) return;
            throw new RuntimeError(oper, "Operands must be numbers.");

        }



        /// <summary>
        /// Method to evaluate if something is true or false, 7.2.4
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        private bool isTruthy(object Object){

            if(Object == null) return false;
            if(Object is bool) return (bool)Object;
            return true;

        }



        /// <summary>
        /// Method for determening equality, 7.2.5
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool isEqual(object a, object b){

            if(a == null && b == null) return true;
            if(a == null) return false;

            return a.Equals(b);

        }



        /// <summary>
        /// Method that turns expression into string, 7.4
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        private string stringify(object Object){
            
            // check for null
            if(Object == null) return "nil";

            if(Object is double){

                string text = Object.ToString();
                if(text.EndsWith(".0")){

                    text = text.Substring(0, text.Length - 2);

                }

                return text;

            }

            return Object.ToString();

        }



        /// <summary>
        /// Method to evaluate arguments, 7.2.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private object evaluate(Expr expr){

            return expr.accept(this);

        }



        /// <summary>
        /// Method to execute stmts, 8.1.3
        /// </summary>
        /// <param name="stmt"></param>
        private void execute(Stmt stmt){

            stmt.accept(this);

        }



        /// <summary>
        /// Method to execute a block, 8.5.2
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="environment"></param>
        public void executeBlock(List<Stmt> statements, Environment environment){
        
            Environment previous = this.environment;
            try{
                this.environment = environment;

                foreach(Stmt statement in statements){

                    execute(statement);

                }
            } finally{

                this.environment = previous;

            }
        }



        /// <summary>
        /// Behavior for block statements, 8.5.2
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitBlockStmt(Stmt.Block stmt){

            executeBlock(stmt.statements, new Environment(environment));
            return null;

        }
    


        /// <summary>
        /// Behavior for visiting expression statement, 8.1.3
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitExpressionStmt(Stmt.Expression stmt){

            evaluate(stmt.expression);
            return null;

        }



        /// <summary>
        /// Behavior for visiting print statement, 8.1.3
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitPrintStmt(Stmt.Print stmt){

            object value = evaluate(stmt.expression);
            Console.Write(stringify(value) + "\n");
            return null;

        }



        /// <summary>
        /// Behavior for visiting variable statement, 8.3.1
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitVarStmt(Stmt.Var stmt){

            // initialize to null
            object value = null;

            // check if initializor changes
            if(stmt.initializer != null){

                value = evaluate(stmt.initializer);

            }

            // define variable and return
            environment.define(stmt.name.lexeme, value);
            return null;

        }



        /// <summary>
        /// Behavior for visiting assignment expressions, 8.4.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitAssignExpr(Expr.Assign expr){

            object value = evaluate(expr.value);
            environment.assign(expr.name, value);
            return value;

        }



        /// <summary>
        /// Behavior for visiting binary expr, 7.2.5
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitBinaryExpr(Expr.Binary expr){

            // set values
            object left = evaluate(expr.left);
            object right = evaluate(expr.right);

            // determine oper and do appropriate behavior
            switch(expr.oper.type){
                case TokenType.GREATER:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL: return !isEqual(left, right);
                case TokenType.EQUAL_EQUAL: return isEqual(left, right);
                case TokenType.MINUS:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left - (double)right;
                case TokenType.SLASH:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    // check operands for error, 7.3.1
                    checkNumberOperands(expr.oper, left, right);
                    return (double)left * (double)right;
                case TokenType.PLUS:
                    if(left is double && right is double){
                        return (double)left + (double)right;
                    }
                    if(left is string && right is string){
                        return (string)left + (string)right;
                    }
                    // error if operands don't match, 7.3.1
                    throw new RuntimeError(expr.oper, 
                    "Operands must be two numbers or two strings.");
            }

            // unreachable
            return null;

        }





    }
}