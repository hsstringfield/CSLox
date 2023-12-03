// file for Environment class



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for coding environment, 8.3
    /// </summary>
    public class Environment{

        public readonly Environment enclosing;
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

            if(enclosing != null){
                enclosing.assign(name, value);
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



        /// <summary>
        /// Method for finding ancestor Env, 11.4.1
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Environment ancestor(int distance){

            Environment environment = this;
            for(int i = 0; i < distance; i++){

                environment = environment.enclosing;

            }

            return environment;

        }



        /// <summary>
        /// Method for finding variable in particular environment, 11.4.1
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object getAt(int distance, string name){

            return ancestor(distance).values[name];
            
        }



        /// <summary>
        /// Method for assigning variable in particular environemtn, 11.4.2
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void assignAt(int distance, Token name, object value){

            ancestor(distance).values[name.lexeme] = value;

        }



        
    }
}