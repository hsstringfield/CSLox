// File for Runtime Error Class, section 7.3.1



using System;
using Microsoft.CSharp.RuntimeBinder;

namespace Lox{

    /// <summary>
    /// RuntimeError class, 7.3.1
    /// </summary>
    public class RuntimeError : Exception{

        public readonly Token token;



        /// <summary>
        /// Constructor for RuntimeError class, 7.3.1
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public RuntimeError(Token token, string message) : base(message){

            this.token = token;

        }

    }

}