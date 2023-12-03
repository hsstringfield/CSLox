// File for interpreter class as described in 7.2



using System;
using System.Collections.Generic;

namespace Lox{

    // added Stmt.Visitor, 8.1.3
    /// <summary>
    /// Interpreter class to interpret code, 7.2
    /// </summary>
    public class Interpreter : Expr.Visitor<object>, Stmt.Visitor<object>{

        public static readonly Environment globals = new Environment();

        private Environment environment = globals;
        private readonly Dictionary<Expr, int> locals = new Dictionary<Expr, int>();


        /// <summary>
        /// Interpreter constructor
        /// </summary>
        public Interpreter(){

            globals.define("clock", new Clock());
            
        }



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
        /// Behavior for visiting logical, 9.3
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitLogicalExpr(Expr.Logical expr){

            object left = evaluate(expr.left);

            if(expr.oper.type == TokenType.OR){
                if(isTruthy(left)) return left;
            } else{
                if(!isTruthy(left)) return left;
            }

            return evaluate(expr.right);

        }



        /// <summary>
        /// Behavior for visiting set expr, 10.4.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeError"></exception>
        public object visitSetExpr(Expr.Set expr) {

            object Object = evaluate(expr.Object);

            if (!(Object is LoxInstance)) { 

            throw new RuntimeError(expr.name, "Only instances have fields.");

            }

            object value = evaluate(expr.value);
            ((LoxInstance)Object).set(expr.name, value);
            return value;

        }



        /// <summary>
        /// Behavior for visiting super expr, 13.3.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitSuperExpr(Expr.Super expr) {

            int distance = locals[expr];
            LoxClass superclass = (LoxClass)environment.getAt(distance, "super");

            LoxInstance Object = (LoxInstance)environment.getAt(distance - 1, "this");

            LoxFunction method = superclass.findMethod(expr.method.lexeme);

            if (method == null) {

                throw new RuntimeError(expr.method, "Undefined property '" + expr.method.lexeme + "'.");
                
            }

            return method.bind(Object);

        }



        /// <summary>
        /// Behavior for visiting this expr, 12.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitThisExpr(Expr.This expr) {

            return lookUpVariable(expr.keyword, expr);

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

            // Changed to utilize resolver, 11.4.1
            return lookUpVariable(expr.name, expr);
        
        }



        /// <summary>
        /// Method to look up variable in locals, 11.4.1
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        private object lookUpVariable(Token name, Expr expr){

            if(locals.ContainsKey(expr)){

                int distance = locals[expr];
                return environment.getAt(distance, name.lexeme);

            } else{

                return globals.get(name);

            }

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
        /// Method to resolve in interpreter, adds to locals, 11.4
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="depth"></param>
        public void resolve(Expr expr, int depth) {

            locals.Add(expr, depth);
            
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
        /// Behavior for visiting class statements, 12.2
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitClassStmt(Stmt.Class stmt) {

            // added for checking validity of superclass, 13.1
            object superclass = null;
            if (stmt.superclass != null) {

                superclass = evaluate(stmt.superclass);
                if (!(superclass is LoxClass)) {

                    throw new RuntimeError(stmt.superclass.name, "Superclass must be a class.");

                }

            }

            environment.define(stmt.name.lexeme, null);
            
            // added for super support, 13.3.2
            if (stmt.superclass != null) {

                environment = new Environment(environment);
                environment.define("super", superclass);

            }

            // added for bound methods, 12.5
            Dictionary<string, LoxFunction> methods = new Dictionary<string, LoxFunction>();
            foreach(Stmt.Function method in stmt.methods){

                LoxFunction function = new LoxFunction(method, environment, method.name.lexeme.Equals("init"));
                methods.Add(method.name.lexeme, function);

            }

            LoxClass klass = new LoxClass(stmt.name.lexeme, (LoxClass)superclass, methods);

            if (superclass != null) environment = environment.enclosing;

            environment.assign(stmt.name, klass);
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
        /// Behavior for visiting function statement, 10.4.1
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitFunctionStmt(Stmt.Function stmt){

            LoxFunction function = new LoxFunction(stmt, environment, false);
            environment.define(stmt.name.lexeme, function);
            return null;

        }



        /// <summary>
        /// Behavior for visiting if statement, 9.2
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitIfStmt(Stmt.If stmt){

            if(isTruthy(evaluate(stmt.condition))){
                execute(stmt.thenBranch);
            } else if(stmt.elseBranch != null){

                execute(stmt.elseBranch);

            }

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
        /// Behavior for visiting return statement, 10.5.1
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        /// <exception cref="Return"></exception>
        public object visitReturnStmt(Stmt.Return stmt){

            object value = null;
            if(stmt.value != null) value = evaluate(stmt.value);

            throw new Return(value);

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
        /// Behavior for visiting while statements, 9.4
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitWhileStmt(Stmt.While stmt){

            while(isTruthy(evaluate(stmt.condition))){
                execute(stmt.body);
            }

            return null;

        }



        /// <summary>
        /// Behavior for visiting assignment expressions, 8.4.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitAssignExpr(Expr.Assign expr){

            object value = evaluate(expr.value);
            
            if(locals.ContainsKey(expr)){

                int distance = locals[expr];
                environment.assignAt(distance, expr.name, value);

            }else{

                globals.assign(expr.name, value);

            }

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



        /// <summary>
        /// Behavior for visiting call expressions, 10.1.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitCallExpr(Expr.Call expr){

            object callee = evaluate(expr.callee);

            List<object> arguments = new List<object>();
            foreach(Expr argument in expr.arguments){

                arguments.Add(evaluate(argument));

            }

            if(!(callee is LoxCallable)){

                throw new RuntimeError(expr.paren, "Can only call functions and classes");

            }

            LoxCallable function = (LoxCallable)callee;
            if(arguments.Count != function.arity()){

                throw new RuntimeError(expr.paren, "Expected " + function.arity() + " arguments but got " + arguments.Count + ".");

            }
            return function.call(this, arguments);

        }



        /// <summary>
        /// Behavior for visitng get expr, 12.4.1
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeError"></exception>
        public Object visitGetExpr(Expr.Get expr) {

            object Object = evaluate(expr.Object);
            if (Object is LoxInstance) {

                return ((LoxInstance)Object).get(expr.name);

            }

            throw new RuntimeError(expr.name, "Only instances have properties.");

        }
    }
}
