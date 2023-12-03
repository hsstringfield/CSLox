// file for LoxInstance class, 12.3



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for instance of classes in Lox, 12.3
    /// </summary>
    public class LoxInstance{

        private LoxClass klass;
        private Dictionary<string, object> fields = new Dictionary<string, object>();



        /// <summary>
        /// Constructor for instance of class, 12.3
        /// </summary>
        /// <param name="klass"></param>
        public LoxInstance(LoxClass klass){

            this.klass = klass;

        }



        /// <summary>
        /// Method for getting property of instance, 12.4.1
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeError"></exception>
        public object get(Token name){

            if(fields.ContainsKey(name.lexeme)){

                return fields[name.lexeme];

            }

            LoxFunction method = klass.findMethod(name.lexeme);
            if(method!= null) return method.bind(this);

            throw new RuntimeError(name, "Undefined property '" + name.lexeme + "'.");

        }



        /// <summary>
        /// Method for setting property of instance, 12.4.2
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void set(Token name, object value){

            fields.Add(name.lexeme, value);

        }



        /// <summary>
        /// ToString method for instance of class, 12.3
        /// </summary>
        /// <returns></returns>
        public override string ToString(){

            return klass.name + " instance";

        }


    }


}