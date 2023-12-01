// file for Environment class



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for coding environment, 8.3
    /// </summary>
    public class Environment{

        private readonly Environment enclosing;
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();



        /// <summary>
        /// Constructor for enclosing, 8.5.1
        /// </summary>
        public Environment(){
            enclosing = null;
        }


        
        /// <summary>
        /// Constructor for nesting, 8.5.1
        /// </summary>
        /// <param name="enclosing"></param>
        public Environment(Environment enclosing){

            this.enclosing = enclosing;

        }



        /// <summary>
        /// Method for returning value of variable, 8.3
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeError"></exception>
        public object get(Token name){

            if(values.ContainsKey(name.lexeme)){

                return values[name.lexeme];

            }

            // Added for variable scope, 8.5.1
            if(enclosing != null) return enclosing.get(name);
            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");

        }



        /// <summary>
        /// Method to assign variable, 8.4.2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <exception cref="RuntimeError"></exception>
        public void assign(Token name, object value){

            if(values.ContainsKey(name.lexeme)){
                values[name.lexeme] = value;
                return;
            }

            throw new RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");

        }



        /// <summary>
        /// Method for defining variables, 8.3
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void define(string name, object value){

            values[name] = value;

        }



        


    }
}