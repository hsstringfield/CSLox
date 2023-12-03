using Lox;
using System;
using System.IO;
using System.Collections.Generic;

public abstract class Expr {
	public interface Visitor<R> {
		R visitAssignExpr(Assign expr);
		R visitBinaryExpr(Binary expr);
		R visitCallExpr(Call expr);
		R visitGetExpr(Get expr);
		R visitGroupingExpr(Grouping expr);
		R visitLiteralExpr(Literal expr);
		R visitLogicalExpr(Logical expr);
		R visitSetExpr(Set expr);
		R visitThisExpr(This expr);
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
	public class Call : Expr{

		public Call(Expr callee, Token paren, List<Expr> arguments) {
			this.callee = callee;
			this.paren = paren;
			this.arguments = arguments;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitCallExpr(this);
		}

		public Expr callee;
		public Token paren;
		public List<Expr> arguments;
	}
	public class Get : Expr{

		public Get(Expr Object, Token name) {
			this.Object = Object;
			this.name = name;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitGetExpr(this);
		}

		public Expr Object;
		public Token name;
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

		public Literal(object value) {
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitLiteralExpr(this);
		}

		public object value;
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
	public class Set : Expr{

		public Set(Expr Object, Token name, Expr value) {
			this.Object = Object;
			this.name = name;
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitSetExpr(this);
		}

		public Expr Object;
		public Token name;
		public Expr value;
	}
	public class This : Expr{

		public This(Token keyword) {
			this.keyword = keyword;
		}

		public override R accept<R>(Visitor<R> visitor){
			return visitor.visitThisExpr(this);
		}

		public Token keyword;
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
