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

				if(string.IsNullOrEmpty(value))
				{
					_input = "0";
				}
				else
				{
					_input = value;

					if(_input.EndsWith(";"))
					{
						_input = _input.Remove(_input.Length - 1);
						_isMuted = true;
					}

					if(_input.Replace(" ", string.Empty).Length == 0)
						_input = "0";
				}
			}
		}

		public bool IsMuted
		{
			get { return _isMuted; }
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