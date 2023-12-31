// C Sharp Lox by Holden Stringfield



using System;
using System.IO;
using System.Collections.Generic;

namespace Lox{

    public class Lox{

        // instance of interpreter, 7.4.2
        private static readonly Interpreter interpreter = new Interpreter();
        // true if detects errors, section 4.1.1
        private static bool hadError = false; 
        // true if detects runtime errors, section 7.4.1
        private static bool hadRuntimeError = false;



        /// <summary>
        ///  Takes input and determines proper behavior, as outlined in 4.1
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args){

            if(args.Length > 1){
                Console.Write("Usage: csLox [script]");
            } else if (args.Length == 1){
                runFile(args[0]);
            }else{
                runPrompt();
            }

        }

    

        /// <summary>
        /// Runs file if directed, as outlined in 4.1
        /// </summary>
        /// <param name="path"></param>
        private static void runFile(string path){

            run(File.ReadAllText(path));

            // exit if hadError true, section 4.1.1
            if(hadError) System.Environment.Exit(65);
            // exit if hadRuntimeError ture, section 7.4.1
            if(hadRuntimeError) System.Environment.Exit(70);

        }


        /// <summary>
        /// Runs prompt if directed, as outlined in 4.1
        /// </summary>
        private static void runPrompt(){

            // loop
            for(;;){
                
                // show where to input, take input and run, if empty break, added newline for better spacing
                Console.Write("\n> ");
                string line = Console.ReadLine();
                if(line == null) break;
                run(line);

                // reset hadError, section 4.1.1
                hadError = false;

            }
        }



        /// <summary>
        /// Runs code when called, as outlined in 4.1
        /// </summary>
        /// <param name="source"></param>
        private static void run(string source){
            
            // create scanner and scan tokens
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            /* Print tokens test, Chapter 4
            Console.Write("\n");
            foreach(Token token in tokens){
                Console.Write(token.toString() + "\n");
            }*/

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.parse();

            if(hadError) return;

            Resolver resolver = new Resolver(interpreter);
            resolver.resolve(statements);

            if (hadError) return;

            /* Print expressions test, Chapter 6
            AstPrinter example = new AstPrinter();
            Console.Write(example.print(expression));
            */

            interpreter.interpret(statements);

        }



        /// <summary>
        /// Throws error, as outlined in 4.1.1
        /// </summary>
        /// <param name="line"></param>
        /// <param name="message"></param>
        public static void error(int line, string message){

            report(line, "", message);

        }



        /// <summary>
        /// Reports error from error method, as outlined in 4.1.1
        /// </summary>
        /// <param name="line"></param>
        /// <param name="where"></param>
        /// <param name="message"></param>
        private static void report(int line, string where, string message){
            
            Console.Error.Write("[line " + line.ToString() + "] Error " + where + ": " + message);
            hadError = true; 

        }



        /// <summary>
        /// Method for throwing errors from parser, 6.3.2
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public static void error(Token token, string message){

            if(token.type ==TokenType.EOF){
                report(token.line, "at end", message);
            }else{
                report(token.line, "at '" + token.lexeme + "'", message);
            }

        }



        /// <summary>
        /// Method for throwing runtime errors from interpreter, 7.4.1
        /// </summary>
        /// <param name="error"></param>
        public static void runtimeError(RuntimeError error){

            Console.Error.Write(error.Message + " [ line " + error.token.line + "]");
            hadRuntimeError = true;

        }


    }
}