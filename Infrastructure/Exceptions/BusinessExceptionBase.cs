namespace Infrastructure.Exceptions
{
    /// <summary>
    /// The base class for business exception
    /// </summary>
    public abstract class BusinessExceptionBase : System.Exception
    {
        /// <summary>
        /// Get the text for the link
        /// </summary>
        public string LinkText
        {
            get { return this.Data["LinkText"] != null ? (string)this.Data["LinkText"] : null; }
            set { this.Data["LinkText"] = value; }
        }

        /// <summary>
        /// Get the URL for the link
        /// </summary>
        public string LinkUrl
        {
            get { return this.HelpLink; }
            set { this.HelpLink = value; }
        }


        /// <summary>
        /// Show link or not
        /// </summary>
        public bool ShowLink
        {
            get { return (this.LinkUrl != null) && (this.LinkText != null); }
        }

        /// <summary>
        /// Error code
        /// </summary>
        public int ErrorCode
        {
            get { return this.HResult; }
            set { this.HResult = value; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message">the error msg</param>
        public BusinessExceptionBase(string message)
            : base(message)
        {
            this.HResult = 0;
            this.Data.Add("HResult", this.HResult);
            this.Data.Add("LinkText", string.Empty);
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="message">the error msg</param>
        public BusinessExceptionBase(int code, string message)
            : base(message)
        {
            this.HResult = code;
            this.Data.Add("HResult", this.HResult);
            this.Data.Add("LinkText", string.Empty);
        }
    }
}