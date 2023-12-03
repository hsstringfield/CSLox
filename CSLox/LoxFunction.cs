// file for LoxFunction class, 10.4



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for Lox Functions wrapper, 10.4
    /// </summary>
    public class LoxFunction : LoxCallable{

        private readonly Stmt.Function declaration;
        // closure variable added, 10.6
        private readonly Environment closure;
        private readonly bool isInitializer;



        /// <summary>
        /// Lox function constructor, 10.4, added initializer 12.7
        /// </summary>
        /// <param name="declaration"></param>
        public LoxFunction(Stmt.Function declaration, Environment closure, bool isInitializer){

            this.isInitializer = isInitializer;
            this.closure = closure;
            this.declaration = declaration;

        }



        /// <summary>
        /// Method for binding functions, 12.6
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public LoxFunction bind(LoxInstance instance) {

            Environment environment = new Environment(closure);
            environment.define("this", instance);
            return new LoxFunction(declaration, environment, isInitializer);

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

                // added for returning in initializer, 12.7.2
                if(isInitializer) return closure.getAt(0, "this");

                return returnValue.value;

            }

            return null;

        }

        

        /// <summary>
        /// Return string of function, 10.4
        /// </summary>
        /// <returns></returns>
        public override string ToString(){

            return "<fn " + declaration.name.lexeme + ">";

        }
    }

}