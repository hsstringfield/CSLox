// File to generate AST from 5.2.2



using System;
using System.IO;
using System.Collections.Generic;

namespace GenerateAST{
    


    /// <summary>
    /// Class for generate AST, as described in 5.2.2
    /// </summary>
    public class GenerateAst{



        /// <summary>
        /// Main function, control improper use and delegate, 5.2.2
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args){

            // control input
            if(args.Length != 1){

                Console.Error.WriteLine("Usage: generate_ast <output directory>");
                System.Environment.Exit(64);

            }

            // create output and populate
            string outputDir = args[0];
            defineAst(outputDir, "Expr", new List<string>{
                "Binary   : Expr left, Token oper, Expr right",
                "Grouping : Expr expression",
                "Literal  : Object value",
                "Unary    : Token oper, Expr right"
            });
        }



        /// <summary>
        /// Method for writing files, 5.2.2
        /// </summary>
        /// <param name="outputDir"></param>
        /// <param name="baseName"></param>
        /// <param name="types"></param>
        private static void defineAst(string outputDir, string baseName, List<string> types){

            // set up stream writer
            string path = outputDir + "/" + baseName + ".cs";
            StreamWriter writer = new StreamWriter(path);

            // write to file
            writer.WriteLine ("using Lox;");
            writer.WriteLine ("using System;");
            writer.WriteLine ("using System.IO;");
            writer.WriteLine ("using System.Collections.Generic;");
            writer.WriteLine("");
            writer.WriteLine("public abstract class " + baseName + " {");

            defineVisitor(writer, baseName, types);

            // iterate through types and write
            foreach(string type in types){

                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(writer, baseName, className, fields);

            }

            writer.WriteLine("");
            writer.WriteLine("\tpublic abstract R accept<R>(Visitor<R> visitor);");

            writer.WriteLine("}");
            writer.Close();

        }



        /// <summary>
        /// Sets up the visitor interface, section 5.3.3
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="baseName"></param>
        /// <param name="types"></param>
        private static void defineVisitor(StreamWriter writer, string baseName, List<string> types){

            // header 
            writer.WriteLine("\tpublic interface Visitor<R> {");

            // iterate and visit for each type
            foreach(string type in types){
                string typeName = type.Split(":")[0].Trim();
                writer.WriteLine("\t\tR visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
            }

            // end Visitor
            writer.WriteLine("\t}");

        }



        /// <summary>
        /// Method that declares class, 5.2.2
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="baseName"></param>
        /// <param name="className"></param>
        /// <param name="fieldList"></param>
        private static void defineType(StreamWriter writer, string baseName, string className, string fieldList){
            
            // create class
            writer.WriteLine("\tpublic class " + className + " : " + baseName + "{\n");

            // constructor
            writer.WriteLine("\t\tpublic " + className + "(" + fieldList + ") {");
            string[] fields = fieldList.Split(", ");
            foreach(string field in fields){

                string name = field.Split(" ")[1];
                writer.WriteLine("\t\t\tthis." + name + " = " + name + ";");

            }
            writer.WriteLine("\t\t}");

            // visitor pattern
            writer.WriteLine("");
            writer.WriteLine("\t\tpublic override R accept<R>(Visitor<R> visitor){");
            writer.WriteLine("\t\t\treturn visitor.visit" + className + baseName + "(this);");
            writer.WriteLine("\t\t}");
            

            // fields
            writer.WriteLine("");
            foreach(string field in fields){
                writer.WriteLine("\t\tpublic " + field + ";");
            }

            // close class
            writer.WriteLine("\t}");
        }







    }



}
