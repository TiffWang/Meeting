using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Exceptions;

namespace Infrastructure
{
    /// <summary>
    /// EmailTemplateParseException
    /// </summary>
    public class EmailTemplateParseException : BusinessExceptionBase
    {
        public EmailTemplateParseException() : this(Language.English) { }
        public EmailTemplateParseException(Language lang) : base(-200, LocalizedString.Get("Fail to convert the email template to html.", lang)) { }
    }
    /// <summary>
    /// EmailTemplateEmptyException
    /// </summary>
    public class EmailTemplateEmptyException : BusinessExceptionBase
    {
        public EmailTemplateEmptyException() : this(Language.English) { }
        public EmailTemplateEmptyException(Language lang) : base(-201, LocalizedString.Get("Not found any email template.", lang)) { }
    }
    /// <summary>
    /// EmailServerEmptyException
    /// </summary>
    public class EmailServerEmptyException : BusinessExceptionBase
    {
        public EmailServerEmptyException() : this(Language.English) { }
        public EmailServerEmptyException(Language lang) : base(-202, LocalizedString.Get("Not found any email server.", lang)) { }
    }
    /// <summary>
    /// EmailFormatException
    /// </summary>
    public class EmailFormatException : BusinessExceptionBase
    {
        public EmailFormatException() : this(Language.English) { }
        public EmailFormatException(Language lang) : base(-203, LocalizedString.Get("Invalid format of the email address.", lang)) { }
    }
}
