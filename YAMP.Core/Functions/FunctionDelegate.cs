namespace YAMP
{
    /// <summary>
    /// This function delegate could be used by any developer to set simply
    /// functions without writing a special class for them.
    /// </summary>
    /// <param name="value">The value to pass as parameter.</param>
    /// <returns>The result of the function call.</returns>
	public delegate Value FunctionDelegate(Value value);
}