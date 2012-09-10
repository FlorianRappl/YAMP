using System;

namespace YAMP
{
	public class Context
	{
		string _original;
		string _input;
		Value _output;
		bool _isMuted;
		
		public Context (string input)
		{
			Input = input;
			Output = Value.Empty;
		}
		
		public string Input
		{
			get { return _input; }
			set 
			{
				_original = value;
				_input = string.IsNullOrEmpty(value) ? "0" : Sanitize(value.Replace(" ", string.Empty));

				if(_input.EndsWith(";"))
				{
					_input = _input.Remove(_input.Length - 1);
					_isMuted = true;
				}
			}
		}

		public bool IsMuted
		{
			get { return _isMuted; }
		}
		
		private string Sanitize(string input)
		{
			var check = input.Length;
			input = input.Replace("++", "+")
						.Replace("--", "+")
						.Replace("+-", "-")
						.Replace("-+", "-")
						.Replace("//", "*")
						.Replace("**", "*")
						.Replace("^*", "^")
						.Replace("*^", "^")
						.Replace("*/", "/")
						.Replace("/*", "/");
			
			if(check != input.Length)
				return Sanitize(input);
			
			return input;
		}
		
		public string Original
		{
			get { return _original; }
		}
		
		public Value Output
		{
			get { return _output; }
			set { _output = value; }
		}
	}
}