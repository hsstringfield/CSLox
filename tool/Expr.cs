using Lox;
using System;
using System.IO;
using System.Collections.Generic;

public abstract class Expr {
	public interface Visitor<R> {
		R visitAssignExpr(Assign expr);
		R visitBinaryExpr(Binary expr);
		R visitGroupingExpr(Grouping expr);
		R visitLiteralExpr(Literal expr);
		R visitLogicalExpr(Logical expr);
		R visitUnaryExpr(Unary expr);
		R visitVariableExpr(Variable expr);
	}
	public class Assign : Expr{

		public Assign(Token name, Expr value) {
			this.name = name;
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitAssignExpr(this);
		}

		public Token name;
		public Expr value;
	}
	public class Binary : Expr{

		public Binary(Expr left, Token oper, Expr right) {
			this.left = left;
			this.oper = oper;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitBinaryExpr(this);
		}

		public Expr left;
		public Token oper;
		public Expr right;
	}
	public class Grouping : Expr{

		public Grouping(Expr expression) {
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitGroupingExpr(this);
		}

		public Expr expression;
	}
	public class Literal : Expr{

		public Literal(Object value) {
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitLiteralExpr(this);
		}

		public Object value;
	}
	public class Logical : Expr{

		public Logical(Expr left, Token oper, Expr right) {
			this.left = left;
			this.oper = oper;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitLogicalExpr(this);
		}

		public Expr left;
		public Token oper;
		public Expr right;
	}
	public class Unary : Expr{

		public Unary(Token oper, Expr right) {
			this.oper = oper;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitUnaryExpr(this);
		}

		public Token oper;
		public Expr right;
	}
	public class Variable : Expr{

		public Variable(Token name) {
			this.name = name;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitVariableExpr(this);
		}

		public Token name;
	}

	public abstract R accept<R>(Visitor<R> visitor);
}
