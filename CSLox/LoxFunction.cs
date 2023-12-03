// file for LoxFunction class, 10.4



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for Lox Functions wrapper, 10.4
    /// </summary>
    class LoxFunction : LoxCallable{

        private readonly Stmt.Function declaration;
        // closure variable added, 10.6
        private readonly Environment closure;



        /// <summary>
        /// Lox function constructor, 10.4
        /// </summary>
        /// <param name="declaration"></param>
        public LoxFunction(Stmt.Function declaration, Environment closure){

            this.closure = closure;
            this.declaration = declaration;

        }



        /// <summary>
        /// Method to return arity, 10.4
        /// </summary>
        /// <returns></returns>
        public int arity(){

            return declaration.prms.Count;

        }



        /// <summary>
        /// Method for call
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object call(Interpreter interpreter, List<object> arguments){

            Environment environment = new Environment(closure);
            for(int i = 0; i < declaration.prms.Count; i++){

                environment.define(declaration.prms[i].lexeme, arguments[i]);

            }

            // changed for return, 10.5.1
            try{

                interpreter.executeBlock(declaration.body, environment);

            } catch (Return returnValue){

                return returnValue.value;

            }

            return null;

        }

        

        /// <summary>
        /// Return string of function, 10.4
        /// </summary>
        /// <returns></returns>
        public string toString(){

            return "<fn " + declaration.name.lexeme + ">";

        }
    }

}