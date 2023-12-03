// file for clock class, 10.2.1



using System;
using System.Collections.Generic;

namespace Lox{


    public class Clock : LoxCallable{

        /// <summary>
        /// Set arity, section 10.2.1
        /// </summary>
        /// <returns></returns>
        public int arity() { return 0; }



        /// <summary>
        /// Call method, 10.2.1
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object call(Interpreter interpreter, List<object> arguments){

            return (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) / 1000.0;

        }



        /// <summary>
        /// Method to return string, 10.2.1
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return "<native fn"; }

    }

}