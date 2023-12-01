// File to print AST, section 5.4



using System.Text;
using System.IO;

namespace Lox{



    /// <summary>
    /// Class that will print AST, section 5.4
    /// </summary>
    public class AstPrinter : Expr.Visitor<string>{



        /// <summary>
        /// Visior implementation, section 5.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string print(Expr expr){

            return expr.accept(this);

        }



        /// <summary>
        /// Visitor for Binary Expr, section 5.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string visitBinaryExpr(Expr.Binary expr){

            return parenthesize(expr.oper.lexeme, expr.left, expr.right);

        }



        /// <summary>
        /// Visitor for Grouping Expr, section 5.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string visitGroupingExpr(Expr.Grouping expr){

            return parenthesize("group", expr.expression);

        }



        /// <summary>
        /// Visitor for Literal Expr, section 5.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string visitLiteralExpr(Expr.Literal expr){

            if(expr.value == null) return "nil";
            return expr.value.ToString();

        }



        /// <summary>
        /// Visitor for Unary Expr, section 5.4
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string visitUnaryExpr(Expr.Unary expr){

            return parenthesize(expr.oper.lexeme, expr.right);

        }



        /// <summary>
        /// Order arguments into parentheses, section 5.4
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exprs"></param>
        /// <returns></returns>
        public string parenthesize(string name, params Expr[] exprs){

            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach(Expr expr in exprs){

                builder.Append(" ");
                builder.Append(expr.accept(this));

            }
            builder.Append(")");

            return builder.ToString();

        }



        /// <summary>
        /// Test main, 5.4
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args){

            Expr expression = new Expr.Binary(
                new Expr.Unary(
                    new Token(TokenType.MINUS, "-", null, 1),
                    new Expr.Literal(123)),
                new Token(TokenType.STAR, "*", null, 1),
                new Expr.Grouping(new Expr.Literal(45.67)));

            Console.Write(new AstPrinter().print(expression));

        }
    }
}