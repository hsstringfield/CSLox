// file for class loxclass wrapper, 12.2



using System;
using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// Class for lox classes wrapper, 12.2
    /// </summary>
    public class LoxClass : LoxCallable{

        public string name;
        private readonly Dictionary<string, LoxFunction> methods = new Dictionary<string, LoxFunction>();



        /// <summary>
        /// LoxClass constructor, 12.2, changed to have methods, 12.5
        /// </summary>
        /// <param name="name"></param>
        public LoxClass(string name, Dictionary<string, LoxFunction> methods) {

            this.name = name;
            this.methods = methods;

        }



        /// <summary>
        /// Method to find method in class, 12.5
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LoxFunction findMethod(string name) {

            if (methods.ContainsKey(name)) {

            return methods[name];

            }

            return null;

        }



        /// <summary>
        /// toString method returns name, 12.2
        /// </summary>
        /// <returns></returns>
        public override string ToString(){

            return name;

        }



        /// <summary>
        /// Call method for LoxClass, 12.3
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object call(Interpreter interpreter, List<object> arguments){

            LoxInstance instance = new LoxInstance(this);

            // added for constructor 12.7
            LoxFunction initializer = findMethod("init");
            if (initializer != null) {

                initializer.bind(instance).call(interpreter, arguments);

            }

            return instance;

        }



        /// <summary>
        /// Arity method for LoxClass, 12.3, changed for init, 12.7
        /// </summary>
        /// <returns></returns>
        public int arity(){

            LoxFunction initializer = findMethod("init");
            if (initializer == null) return 0;
            return initializer.arity();

        }
    }
}