namespace YAMP
{
	/// <summary>
	/// Abstract base class of scripting keywords.
	/// </summary>
	public abstract class Keyword : Expression
    {
        #region ctor

        /// <summary>
        /// Creates a new keyword instance.
        /// </summary>
        /// <param name="keyword">The keyword to use (token).</param>
        public Keyword(string keyword)
		{
            Token = keyword;
            IsSingleStatement = true;
		}

		#endregion

        #region Properties

        /// <summary>
        /// Gets the token (pattern or keyword) that represents the current keyword.
        /// </summary>
		public string Token { get; private set; }

		#endregion

        #region Methods

        /// <summary>
        /// Registers the element at the factory.
        /// </summary>
        public override void RegisterElement(Elements elements)
		{
			elements.AddKeyword(Token, this);
		}

        #endregion
    }
}
