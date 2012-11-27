using System;

namespace YAMP
{
	public class NonconvergenceException : YAMPException
	{
		public NonconvergenceException() : base("Unfortunately no convergence could be reached. The process has been terminated.")
		{
		}
	}
}
