namespace Infrastructure
{
    /// <summary>
    /// 正则表达式
    /// </summary>
    public static class RegexLibrary
    {
        /// <summary>非负整数</summary>
        public const string NonNegativeInteger = @"^\d+$";

        /// <summary>整数</summary>
        public const string Integer = @"^-?\d+$";

        /// <summary>布尔</summary>
        public const string Boolean = @"^0|1|False|True|false|true$";

        /// <summary>带小数的十进制数</summary>
        public const string Decimal = @"^-?\d+(\.\d+)?$";

        /// <summary>金钱</summary>
        public const string Currency = @"^[-+]?\[1-9]\d+(?:,\d{3})*(?:\.\d{1,2})?$";

        /// <summary>日期</summary>
        public const string Date = @"^\d{2,4}[-\/]\d{1,2}[-\/]\d{1,4}$";

        /// <summary>日期 时间</summary>
        public const string DateTime = @"^\d{2,4}[-\/]\d{1,2}[-\/]\d{1,4}(?:\d{1,2}:\d{1,2}:\d{1,2})?$";

        /// <summary>IP</summary>
        public const string IP = @"^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$";

        /// <summary></summary>
        public const string REPip = @"^\d{1,3}(\.(\d{1,3}|\*)){3}(;\d{1,3}(\.(\d{1,3}|\*)){3})*$";

        /// <summary>Email</summary>
        public const string Email = @"^[0-9a-zA-Z]+([\-\\_\.][0-9a-zA-Z]+)*\@[0-9a-zA-Z]+(\-[0-9a-zA-Z]+)*(\.[0-9a-zA-Z]+(\-[0-9a-zA-Z]+)*)+$";

        /// <summary>Url</summary>
        public const string Url = @"^[a-zA-Z]+:\/\/[0-9a-zA-Z]+(\-[0-9a-zA-Z]+)*(\.[0-9a-zA-Z]+(\-[0-9a-zA-Z]+)*)+(\:\d+)?(\S+)?$";

        /// <summary>ID列表</summary>
        public const string IDList = @"^\d+(?:,\d+)*$";

        /// <summary>注册名称</summary>
        public const string SafeRegisteredName = @"^[^'""<>,&\n\r\f\t]+$";

        /// <summary>图像类型</summary>
        public const string SafeWebImageType    = @"^(?:(?:image/gif)|(?:image/pjpeg)|(?:image/png)|(?:image/bmp))$";

        /// <summary>图像扩展名称</summary>
        public const string SafeImageExtension  = @"\.?(gif|jpeg|jpg|png|bmp)$";

        /// <summary>字符[a-z][A-Z]和下划线</summary>
        public const string Alpha       = @"^[a-zA-Z\_])+$";

        /// <summary>字符[a-z][A-Z][0-9]和下划线</summary>
        public const string Alphanum    = @"^\w+$";

        /// <summary>电话号码 </summary>
        public const string Tel = @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)";

    }
}
