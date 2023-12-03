using Lox;
using System;
using System.IO;
using System.Collections.Generic;

public abstract class Stmt {
	public interface Visitor<R> {
		R visitBlockStmt(Block stmt);
		R visitClassStmt(Class stmt);
		R visitExpressionStmt(Expression stmt);
		R visitIfStmt(If stmt);
		R visitFunctionStmt(Function stmt);
		R visitPrintStmt(Print stmt);
		R visitReturnStmt(Return stmt);
		R visitVarStmt(Var stmt);
		R visitWhileStmt(While stmt);
	}
	public class Block : Stmt{

		public Block(List<Stmt> statements) {
			this.statements = statements;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitBlockStmt(this);
		}

		public List<Stmt> statements;
	}
	public class Class : Stmt{

		public Class(Token name, List<Stmt.Function> methods) {
			this.name = name;
			this.methods = methods;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitClassStmt(this);
		}

		public Token name;
		public List<Stmt.Function> methods;
	}
	public class Expression : Stmt{

		public Expression(Expr expression) {
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitExpressionStmt(this);
		}

		public Expr expression;
	}
	public class If : Stmt{

		public If(Expr condition, Stmt thenBranch, Stmt elseBranch) {
			this.condition = condition;
			this.thenBranch = thenBranch;
			this.elseBranch = elseBranch;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitIfStmt(this);
		}

		public Expr condition;
		public Stmt thenBranch;
		public Stmt elseBranch;
	}
	public class Function : Stmt{

		public Function(Token name, List<Token> prms, List<Stmt> body) {
			this.name = name;
			this.prms = prms;
			this.body = body;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitFunctionStmt(this);
		}

		public Token name;
		public List<Token> prms;
		public List<Stmt> body;
	}
	public class Print : Stmt{

		public Print(Expr expression) {
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitPrintStmt(this);
		}

		public Expr expression;
	}
	public class Return : Stmt{

		public Return(Token keyword, Expr value) {
			this.keyword = keyword;
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitReturnStmt(this);
		}

		public Token keyword;
		public Expr value;
	}
	public class Var : Stmt{

		public Var(Token name, Expr initializer) {
			this.name = name;
			this.initializer = initializer;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitVarStmt(this);
		}

		public Token name;
		public Expr initializer;
	}
	public class While : Stmt{

		public While(Expr condition, Stmt body) {
			this.condition = condition;
			this.body = body;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitWhileStmt(this);
		}

		public Expr condition;
		public Stmt body;
	}

	public abstract R accept<R>(Visitor<R> visitor);
}
