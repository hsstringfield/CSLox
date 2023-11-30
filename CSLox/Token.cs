// File for Token class as described in 4.2.3



namespace Lox{

    /// <summary>
    /// Class for tokens, as described in 4.2.3
    /// </summary>
    public class Token{

        public readonly TokenType type; 
        public readonly string lexeme;
        public readonly object literal;
        public readonly int line; 



        /// <summary>
        /// Constructor for tokens, as described in 4.2.3
        /// </summary>
        /// <param name="token"></param>
        /// <param name="lexeme"></param>
        /// <param name="literal"></param>
        /// <param name="line"></param>
        public Token(TokenType type, string lexeme, object literal, int line){

            this.type = type;
            this.lexeme = lexeme; 
            this.literal = literal;
            this.line = line;

        }



        /// <summary>
        /// Method that will return values of Token, as described in 4.2.3
        /// </summary>
        /// <returns></returns>
        public string toString(){
            
            return type + " " + lexeme + " " + literal;

        }

    }

}