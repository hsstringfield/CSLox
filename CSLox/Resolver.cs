// File for resolver class



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for resolver, solves leakage issue, 11.3
    /// </summary>
    public class Resolver : Expr.Visitor<object>, Stmt.Visitor<object>{

        private readonly Interpreter interpreter;
        private readonly List<Dictionary<string, bool>> scopes = new List<Dictionary<string, bool>>();
        // added to check if in function, 11.5.1
        private FunctionType currentFunction = FunctionType.NONE;



        /// <summary>
        /// Constructor for resolver class, 11.3
        /// </summary>
        /// <param name="interpreter"></param>
        public Resolver(Interpreter interpreter){

            this.interpreter = interpreter;

        }



        /// <summary>
        /// Enum type for functions, determine if is one, 11.5.1
        /// </summary>
        private enum FunctionType{

            NONE, 
            FUNCTION,
            INITIALIZER,
            METHOD

        }



        /// <summary>
        /// Enum type for classes, determine if is one, 12.6
        /// </summary>
        private enum ClassType {

            NONE,
            CLASS

        }

        private ClassType currentClass = ClassType.NONE;



        /// <summary>
        /// Method for resolving statements in scope, 11.3.1
        /// </summary>
        /// <param name="statements"></param>
        public void resolve(List<Stmt> statements){

            foreach(Stmt statement in statements){

                resolve(statement);

            }

        }



        /// <summary>
        /// Begin scope by making entry in scopes for variable, 11.3.1
        /// </summary>
        private void beginScope(){

            scopes.Add(new Dictionary<string, bool>());

        }



        /// <summary>
        /// Removes scope from the dictionary of scopes, 11.3.2
        /// </summary>
        private void endScope(){

            scopes.RemoveAt(scopes.Count - 1);

        }



        /// <summary>
        /// Declare variable in scope, return if empty, 11.3.2
        /// </summary>
        /// <param name="name"></param>
        private void declare(Token name){

            if(scopes.Count < 1) return;

            var scope = scopes[scopes.Count - 1];

            // added to check if already exists, return error, 11.5
            if(scope.ContainsKey(name.lexeme)){

                Lox.error(name, "Already a variable with this name in this scope");

            }

            scope.Add(name.lexeme, false);

        }



        /// <summary>
        /// Define variable in scope, return if empty, 11.3.2
        /// </summary>
        /// <param name="name"></param>
        private void define(Token name){

            if(scopes.Count < 1) return;
            scopes[scopes.Count - 1][name.lexeme] = true;

        }



        /// <summary>
        /// Helper method for visitVariableExpr and visitAssignExpr, 11.3.3
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="name"></param>
        private void resolveLocal(Expr expr, Token name){

            for(int i = scopes.Count - 1 ; i >= 0; i--){

                if(scopes[i].ContainsKey(name.lexeme)){

                    interpreter.resolve(expr, scopes.Count - 1 - i);
                    return;

                }

            }

        }



        /// <summary>
        /// Behavior for visiting block stmts w proper scope, 11.3.1
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitBlockStmt(Stmt.Block stmt){

            beginScope();
            resolve(stmt.statements);
            endScope();
            return null;

        }



        /// <summary>
        /// Behavior for visiting class stmts w proper scope, 12.2
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitClassStmt(Stmt.Class stmt) {

            ClassType enclosingClass = currentClass;
            currentClass = ClassType.CLASS;

            declare(stmt.name);
            define(stmt.name);

            // added for this, 12.6
            beginScope();
            scopes[scopes.Count - 1].Add("this", true);

            foreach(Stmt.Function method in stmt.methods){

                FunctionType declaration = FunctionType.METHOD;
                
                // sets to initializer so will not return, 12.7.2
                if (method.name.lexeme.Equals("init")) {
                    declaration = FunctionType.INITIALIZER;
                }

                resolveFunction(method, declaration);

            }

            endScope();

            currentClass = enclosingClass;
            return null;

        }


        
        /// <summary>
        /// Behavior for visiting expr stmts w proper scope, 11.3.6
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitExpressionStmt(Stmt.Expression stmt){

            resolve(stmt.expression);
            return null;

        }



        /// <summary>
        /// Behavior for visitng func stmts w proper scope, 11.3.5
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitFunctionStmt(Stmt.Function stmt){

            declare(stmt.name);
            define(stmt.name);

            resolveFunction(stmt, FunctionType.FUNCTION);
            return null;

        }



        /// <summary>
        /// Behavior for visiting if stmts w proper scope, 11.3.6
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitIfStmt(Stmt.If stmt) {

            resolve(stmt.condition);
            resolve(stmt.thenBranch);
            if (stmt.elseBranch != null) resolve(stmt.elseBranch);
            return null;

        }



        /// <summary>
        /// Behavior for visiting print stmts w proper scope, 11.3.6
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitPrintStmt(Stmt.Print stmt) {

            resolve(stmt.expression);
            return null;

        }



        /// <summary>
        /// Behavior for visitng return stmts w proper scope, 11.3.6
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitReturnStmt(Stmt.Return stmt) {

            // Added to check if in function decl, 11.5.1
            if(currentFunction == FunctionType.NONE){

                Lox.error(stmt.keyword, "Can't return from top-level code.");

            }

            // added for returning value from initializer, 12.7.2
            if (stmt.value != null) {
                
                if (currentFunction == FunctionType.INITIALIZER) {
                    Lox.error(stmt.keyword, "Can't return a value from an initializer.");
                }

                resolve(stmt.value);

            }
            

            return null;

        }



        /// <summary>
        /// Behavior for visiting var stmts w proper scope, 11.3.2
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitVarStmt(Stmt.Var stmt){

            declare(stmt.name);
            if(stmt.initializer != null) resolve(stmt.initializer);
            define(stmt.name);
            return null;

        }



        /// <summary>
        /// Behavior for visitng while stmts w proper scope, 11.3.6
        /// </summary>
        /// <param name="stmt"></param>
        /// <returns></returns>
        public object visitWhileStmt(Stmt.While stmt) {

            resolve(stmt.condition);
            resolve(stmt.body);
            return null;

        }



        /// <summary>
        /// Behavior for visiting assign expr w proper scope, 11.3.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitAssignExpr(Expr.Assign expr){

            resolve(expr.value);
            resolveLocal(expr, expr.name);
            return null;

        }



        /// <summary>
        /// Behavior for visitng bin expr w proper scope, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitBinaryExpr(Expr.Binary expr) {

            resolve(expr.left);
            resolve(expr.right);
            return null;

        }



        /// <summary>
        /// Behavior for visiting call expr w proper scope, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitCallExpr(Expr.Call expr){

            resolve(expr.callee);

            foreach(Expr argument in expr.arguments){

                resolve(argument);

            }

            return null;

        }



        /// <summary>
        /// Behavior for visiting get expr w proper scope, 12.4.1
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitGetExpr(Expr.Get expr){

            resolve(expr.Object);
            return null;

        }



        /// <summary>
        /// Behavior for visiting group expr w proper scope, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitGroupingExpr(Expr.Grouping expr) {

            resolve(expr.expression);
            return null;

        }



        /// <summary>
        /// Behavior for visiting literal expr w proper scope, just returns null, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitLiteralExpr(Expr.Literal expr) {

            return null;

        }



        /// <summary>
        /// Behavior for visiting logical expr w proper scope, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitLogicalExpr(Expr.Logical expr) {

            resolve(expr.left);
            resolve(expr.right);
            return null;

        }



        /// <summary>
        /// Behavior for visiting set expr w proper scope, 12.4.2
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitSetExpr(Expr.Set expr) {

            resolve(expr.value);
            resolve(expr.Object);
            return null;

        }



        /// <summary>
        /// Behavior for visiting this expr w proper scope, 12.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitThisExpr(Expr.This expr) {

            // added to check for this outside class, 12.6
            if (currentClass == ClassType.NONE) {

                Lox.error(expr.keyword, "Can't use 'this' outside of a class.");
                return null;

            }

            resolveLocal(expr, expr.keyword);
            return null;

        }



        /// <summary>
        /// Behavior for unary expr w proper scope, 11.3.6
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitUnaryExpr(Expr.Unary expr) {

            resolve(expr.right);
            return null;

        }



        /// <summary>
        /// Behavior for visiting variable expr w proper scope, 11.3.3
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object visitVariableExpr(Expr.Variable expr){

                if(!(scopes.Count < 1) && scopes[scopes.Count - 1].TryGetValue(expr.name.lexeme, out bool value) == false){

                    Lox.error(expr.name, "Can't read local variable in its own initializer.");

                }

            resolveLocal(expr, expr.name);
            return null;

        }



        /// <summary>
        /// Resolve statements, 11.3.1
        /// </summary>
        /// <param name="expr"></param>
        private void resolve(Stmt stmt){

            stmt.accept(this);

        }



        /// <summary>
        /// Resolve expressions, 11.3.1
        /// </summary>
        /// <param name="expr"></param>
        private void resolve(Expr expr){

            expr.accept(this);

        }



        /// <summary>
        /// Helper method for visiting func stmts, 11.3.5
        /// </summary>
        /// <param name="function"></param>
        private void resolveFunction(Stmt.Function function, FunctionType type){

            // added for function check, 11.5.1
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;

            beginScope();
            foreach(Token prm in function.prms){

                declare(prm);
                define(prm);

            }

            resolve(function.body);
            endScope();
            currentFunction = enclosingFunction;

        }


    }


}