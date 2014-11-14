using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public class CustomerException : Exception
    {
        public string ErrorCode { get; set; }

        /// <summary>
        /// 自定义异常
        /// </summary>
        /// <param name="errorCode"></param>
        public CustomerException(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
