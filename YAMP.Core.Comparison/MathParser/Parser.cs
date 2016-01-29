/* Copyright (C) 2005 <Paratrooper> paratrooper666@gmx.net
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  any later version.
 */

using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace MathParser
{
	public enum Mode { RAD, DEG, GRAD };

	public class Parser
	{
		private ArrayList FunctionList = new ArrayList( new string[] { "abs", "acos", "asin", "atan", "ceil", "cos", "cosh", "exp", "floor", "ln", "log", "sign", "sin", "sinh", "sqrt", "tan", "tanh" } );
		private double Value;
		private double Factor;
		private Mode mode;

		// Constructor
		public Parser()
		{
			this.Mode = Mode.RAD;
		}
		public Parser( Mode mode )
		{
			this.Mode  = mode;
		}

		// Getter & Setter
		public double Result
		{
			get{ return this.Value; }
		}
		public Mode Mode
		{
			get{ return this.mode; }
			set
			{
				this.mode = value;
				switch( value )
				{
					case Mode.RAD :
						this.Factor = 1.0;
						break;
					case Mode.DEG :
						this.Factor = 2.0 * Math.PI / 360.0;
						break;
					case Mode.GRAD :
						this.Factor = 2.0 * Math.PI / 400.0;
						break;
				}
			}
		}

		public bool Evaluate( string Expression ) 
		{
			try 
			{
				// ****************************************************************************************
				// ** MathParser in action:                                                              **
				// ** Expression = "-(5 - 10)^(-1)  ( 3 + 2(    cos( 3 Pi )+( 2+ ln( exp(1) ) )    ^3))" **
				// ****************************************************************************************
				//
				//
				// ----------
				// - Step 1 -
				// ----------
				// Remove blank.
				//
				// -(5 - 10)^(-1)  ( 3 + 2(    cos( 3 Pi )+( 2+ ln( exp(1) ) )    ^3)) -> -(5-10)^(-1)(3+2(cos(3Pi)+(2+ln(exp(1)))^3))
				//
				Expression = Expression.Replace( " ", "" );
				//
				// ----------
				// - Step 2 -
				// ----------
				// Insert '*' if necessary.
				//
				//                                                             _    _      _
				// -(5-10)^(-1)(3+2(cos(3Pi)+(2+ln(exp(1)))^3)) -> -(5-10)^(-1)*(3+2*(cos(3*Pi)+(2+ln(exp(1)))^3))
				//             |   |     |
				//
				Regex regEx = new Regex( @"(?<=[\d\)])(?=[a-df-z\(])|(?<=pi)(?=[^\+\-\*\/\\^!)])|(?<=\))(?=\d)|(?<=[^\/\*\+\-])(?=exp)", RegexOptions.IgnoreCase );
				Expression = regEx.Replace( Expression, "*" );
				//
				// ----------
				// - Step 3 -
				// ----------
				// Replace constants: Pi -> 3,14...
				//
				//                                                                        ____
				// -(5-10)^(-1)*(3+2*(cos(3*Pi)+(2+ln(e))^3)) -> -(5-10)^(-1)*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3))
				//                          --
				//
				regEx = new Regex( "pi", RegexOptions.IgnoreCase );
				Expression = regEx.Replace( Expression, Math.PI.ToString() );
				//
				// ----------
				// - Step 4 -
				// ----------
				// Search for parentheses an solve the expression between it.
				//
				//                                                       _____
				// -(5-10)^(-1)*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3)) -> -{-5}^(-1)*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3))
				//  |_____|
				//                                                          __
				// -{-5}^(-1)*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3)) -> -{-5}^-1*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3))
				//       |__|
				//                                                                    ____
				// -{-5}^-1*(3+2*(cos(3*3,14)+(2+ln(exp(1)))^3)) -> -{-5}^-1*(3+2*(cos9,42+(2+ln(exp(1)))^3))
				//                   |______|
				//                                                                              _
				// -{-5}^-1*(3+2*(cos9,72+(2+ln(exp(1)))^3)) -> -{-5}^-1*(3+2*(cos9,72+(2+ln(exp1))^3))
				//                                 |_|
				//                                                                        ____
				// -{-5}^-1*(3+2*(cos9,72+(2+ln(exp1))^3)) -> -{-5}^-1*(3+2*(cos9,72+(2+ln2,71)^3))
				//                             |____|
				//                                                                 ____
				// -{-5}^-1*(3+2*(cos9,72+(2+ln2,71)^3)) -> -{-5}^-1*(3+2*(cos9,72+{3}^3))
				//                        |_________|
				//                                                 __
				// -{-5}^-1*(3+2*(cos9,72+{3}^3)) -> -{-5}^-1*(3+2*26)
				//               |_____________|
				//                               __
				// -{-5}^-1*(3+2*26) -> -{-5}^-1*55
				//          |______|
				//
				regEx = new Regex( @"([a-z]*)\(([^\(\)]+)\)(\^|!?)", RegexOptions.IgnoreCase );
				Match m = regEx.Match( Expression );
				while( m.Success )
				{
					if( m.Groups[3].Value.Length > 0 ) Expression = Expression.Replace( m.Value, "{" + m.Groups[1].Value + this.Solve( m.Groups[2].Value ) + "}" + m.Groups[3].Value );
					else Expression = Expression.Replace( m.Value, m.Groups[1].Value + this.Solve( m.Groups[2].Value ) );
					m = regEx.Match( Expression );
				}
				//
				// ----------
				// - Step 5 -
				// ----------
				// There are no more parentheses. Solve the expression and convert it to double.
				//                __
				// -{-5}^-1*55 => 11
				// |_________|
				//
				this.Value = Convert.ToDouble( this.Solve( Expression ) );
				return true;
			}
			catch
			{
				// Shit!
				return false;
			}
		}

		private string Solve( string Expression )
		{
			// Solve Sin, Cos, Tan...
			Regex regEx = new Regex( @"([a-z]{2,})([\+-]?\d+,*\d*[eE][\+-]*\d+|[\+-]?\d+,*\d*)", RegexOptions.IgnoreCase );
			Match m = regEx.Match( Expression );
			while( m.Success && this.FunctionList.IndexOf(m.Groups[1].Value.ToLower() ) > -1  )
			{
				switch( m.Groups[1].Value.ToLower() )
				{
					case "abs" :
						Expression = Expression.Replace( m.Groups[0].Value, Math.Abs( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "acos" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Acos( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "asin" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Asin( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "atan" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Atan( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "cos" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Cos( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "ceil" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Ceiling( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "cosh" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Cosh( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "exp" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Exp( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "floor" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Floor( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "ln" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Log( Convert.ToDouble( m.Groups[2].Value ), Math.Exp(1.0) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "log" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Log10( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "sign" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Sign( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "sin" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Sin( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "sinh" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Sinh( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "sqrt" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Sqrt( Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "tan" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Tan( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
					case "tanh" :
						Expression = Expression.Replace( m.Groups[0].Value , Math.Tanh( this.Factor * Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
						m = regEx.Match( Expression );
						continue;
				}
			}
			// Solve Factorial.
			regEx = new Regex( @"\{(.+)\}!" ); // Search for patterns like {5}!
			m = regEx.Match( Expression );
			while( m.Success )
			{
				double n = Convert.ToDouble( m.Groups[1].Value );
				if( (n < 0) && (n != Math.Round(n)) ) throw new Exception(); // Value negative or not integer -> throw exception
				Expression = regEx.Replace( Expression, this.Fact( Convert.ToDouble( m.Groups[1].Value ) ).ToString(), 1 );
				m = regEx.Match( Expression );
			}
			regEx = new Regex( @"(\d+,*\d*[eE][\+-]?\d+|\d+,*\d*)!" ); // Search for patterns like 5!
			m = regEx.Match( Expression );
			while( m.Success )
			{
				double n = Convert.ToDouble( m.Groups[1].Value );
				if( (n < 0) && (n != Math.Round(n)) ) throw new Exception(); // Value negative or not integer -> throw exception
				Expression = regEx.Replace( Expression, this.Fact( Convert.ToDouble( m.Groups[1].Value ) ).ToString(), 1 );
				m = regEx.Match( Expression ); 
			}
			// Solve power.
			regEx = new Regex( @"\{(.+)\}\^(-?\d+,*\d*[eE][\+-]?\d+|-?\d+,*\d*)" ); // Search for patterns like {-5}^-1
			m = regEx.Match( Expression, 0 );
			while( m.Success )
			{
				Expression = Expression.Replace( m.Value, Math.Pow( Convert.ToDouble( m.Groups[1].Value ), Convert.ToDouble( m.Groups[2].Value ) ).ToString() );
				m = regEx.Match( Expression );
			}
			regEx = new Regex( @"(\d+,*\d*e[\+-]?\d+|\d+,*\d*)\^(-?\d+,*\d*[eE][\+-]?\d+|-?\d+,*\d*)" ); // Search for patterns like 5^-1
			m = regEx.Match( Expression, 0 );
			while( m.Success )
			{
				Expression = regEx.Replace( Expression, Math.Pow( Convert.ToDouble( m.Groups[1].Value ), Convert.ToDouble( m.Groups[2].Value ) ).ToString(), 1 );
				m = regEx.Match( Expression );
			}
			// Solve multiplication / division.
			regEx = new Regex( @"([\+-]?\d+,*\d*[eE][\+-]?\d+|[\-\+]?\d+,*\d*)([\/\*])(-?\d+,*\d*[eE][\+-]?\d+|-?\d+,*\d*)" );
			m = regEx.Match( Expression, 0 );
			while( m.Success )
			{
				double result;
				switch( m.Groups[2].Value )
				{
					case "*" :
						result = Convert.ToDouble( m.Groups[1].Value ) * Convert.ToDouble( m.Groups[3].Value );
						if( (result < 0) || (m.Index == 0) ) Expression = regEx.Replace( Expression, result.ToString(), 1 );
						else Expression = Expression.Replace( m.Value, "+" + result );
						m = regEx.Match( Expression );
						continue;
					case "/" :
						result = Convert.ToDouble( m.Groups[1].Value ) / Convert.ToDouble( m.Groups[3].Value );
						if( (result < 0) || (m.Index == 0) ) Expression = regEx.Replace( Expression, result.ToString(), 1 );
						else Expression = regEx.Replace( Expression, "+" + result, 1 );
						m = regEx.Match( Expression );
						continue;
				}
			}
			// Solve addition / subtraction.
			regEx = new Regex( @"([\+-]?\d+,*\d*[eE][\+-]?\d+|[\+-]?\d+,*\d*)([\+-])(-?\d+,*\d*[eE][\+-]?\d+|-?\d+,*\d*)" );
			m = regEx.Match( Expression, 0 );
			while( m.Success )
			{
				double result;
				switch( m.Groups[2].Value )
				{
					case "+" :
						result = Convert.ToDouble( m.Groups[1].Value ) + Convert.ToDouble( m.Groups[3].Value );
						if( (result < 0) || (m.Index == 0) ) Expression = regEx.Replace( Expression, result.ToString(), 1 );
						else Expression = regEx.Replace( Expression, "+" + result, 1 );
						m = regEx.Match( Expression );
						continue;
					case "-" :
						result = Convert.ToDouble( m.Groups[1].Value ) - Convert.ToDouble( m.Groups[3].Value );
						if( (result < 0) || (m.Index == 0) ) Expression = regEx.Replace( Expression, result.ToString(), 1 );
						else Expression = regEx.Replace( Expression, "+" + result, 1 );
						m = regEx.Match( Expression );
						continue;
				}
			}
			if( Expression.StartsWith( "--" ) ) Expression = Expression.Substring(2);
			return Expression;
		}

		// Calculate Factorial.
		private double Fact( double n )
		{
			return n == 0.0 ? 1.0 : n * Fact( n - 1.0 );
		}
	}
}