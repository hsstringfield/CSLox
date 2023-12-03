// file for lox callable class



using System.Collections.Generic;

namespace Lox{

    /// <summary>
    /// interface for callable functions, 10.1.2
    /// </summary>
    public interface LoxCallable{

        int arity();
        object call(Interpreter interpreter, List<Object> arguments);

    }

}