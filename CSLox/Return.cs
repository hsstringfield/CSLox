// file for return class, 8.5.1



using System;

namespace Lox{

    /// <summary>
    /// Class for return function wrap, 8.5.1
    /// </summary>
    public class Return : Exception{

        public readonly object value;



        /// <summary>
        /// Constructor for return class, 8.5.1
        /// </summary>
        /// <param name="value"></param>
        public Return(object value){

            this.value = value;

        }

    }
}