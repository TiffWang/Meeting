using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Infrastructure
{
    /// <summary>
    /// The class extension of String.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Convert string to list by custom seperator.
        /// </summary>
        /// <typeparam name="T">Expected type T</typeparam>
        /// <param name="str">String</param>
        /// <param name="seperator">Seperator</param>
        /// <returns>List&lt;T&gt;</returns>
        /// <example>
        /// <code>
        /// string str = "12123,342432,5436546.321321,";
        /// long[] ary = str.ConvertToList&lt;long&gt;(',', '.');
        /// // So. the ary should be a long array mode up of 12123 / 342432 / 5436546 / 321321
        /// // Of cuase, you can write the codes below, But it is not recommand
        /// ary = StringExtension.ConvertToList&lt;long&gt;(str, ',', '.');
        /// </code>
        /// </example>
        public static List<T> ConvertToList<T>( this string str, params char [] seperator)
        {
            Debug.Assert(!string.IsNullOrEmpty(str));
            Debug.Assert(seperator != null && seperator.Length > 0);

            List<T> result = new List<T>();
            string[] ary = str.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in ary)
            {
                result.Add( (T)Convert.ChangeType( item, typeof(T)) );
            }

            return result;
        }
        /// <summary>
        /// Convert string to array by custom seperator.
        /// </summary>
        /// <typeparam name="T">Expected type T</typeparam>
        /// <param name="str">String</param>
        /// <param name="seperator">Seperator</param>
        /// <returns>Array: T[]</returns>
        /// <example>
        /// <code>
        /// string str = "12123,342432,5436546.321321,";
        /// long[] ary = str.ConvertToArray&lt;long&gt;(',', '.');
        /// // So. the ary should be a long array mode up of 12123 / 342432 / 5436546 / 321321
        /// // Of cuase, you can write the codes below, But it is not recommand
        /// ary = StringExtension.ConvertToArray&lt;long&gt;(str, ',', '.');
        /// </code>
        /// </example>
        public static T[] ConvertToArray<T>( this string str, params char [] seperator){
            return ConvertToList<T>(str, seperator).ToArray();
        }

        /// <summary>
        /// Convert string to list by comma.
        /// </summary>
        /// <typeparam name="T">Expected type T</typeparam>
        /// <param name="str">String</param>
        /// <returns>List&lt;T&gt;</returns>
        /// <example>
        /// <code>
        /// string str = "12123,342432,5436546.321321,";
        /// long[] ary = str.ConvertToList&lt;long&gt;();
        /// // So. the ary should be a long array mode up of 12123 / 342432 / 5436546 / 321321
        /// </code>
        /// </example>
        public static List<T> ConvertCommaSeperatedToList<T>(this string str)
        {
            return ConvertToList<T>(str, ',');
        }

        /// <summary>
        /// Convert the array to string by custom seperator
        /// </summary>
        /// <param name="ary">Any array type base upon IEnumerable</param>
        /// <param name="seperator">Seperator</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        /// IList&lt;long&gt; ary = StringExtension.ConvertCommaSeperatedToList&lt;long&gt;("12123,342432,5436546,321321");
        /// // The result is 12123 / 342432 / 5436546 / 321321 mode up of List&lt;long&gt;
        /// 
        /// string str = StringExtension.ConvertToSeperatedString(ary, '|');
        /// // The result is "12123|342432|5436546|321321"
        /// 
        /// str = ary.ConvertToSeperatedString('|');
        /// // The result is "12123|342432|5436546|321321"
        /// </code>
        /// </example>
        public static string ConvertToSeperatedString( this IEnumerable ary, char seperator)
        {
            Debug.Assert(ary != null);

            StringBuilder sb = new StringBuilder();

            foreach (var item in ary)
            {
                sb.Append(item.ToString());
                sb.Append(seperator);
            }

            return sb.ToString().Trim(seperator);
        }

        /// <summary>
        /// Convert the array to string by comma
        /// </summary>
        /// <param name="ary">Any array type base upon IEnumerable</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        /// IList&lt;long&gt; ary = StringExtension.ConvertCommaSeperatedToList&lt;long&gt;("12123,342432,5436546,321321");
        /// // The result is 12123 / 342432 / 5436546 / 321321 mode up of List&lt;long&gt;
        /// 
        /// string str = StringExtension.ConvertToSeperatedString(ary);
        /// // The result is "12123,342432,5436546,321321"
        /// </code>
        /// </example>
        public static string ConvertToCommaSeperatedString(this IEnumerable ary)
        {
            return ConvertToSeperatedString(ary, ',');
        }


        /// <summary>
        /// Convert the array list to text string, and insert a custom seperator between each item.
        /// </summary>
        /// <typeparam name="T">Expected T type</typeparam>
        /// <param name="seperator">Seperator</param>
        /// <param name="items">Array list</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        /// public enum HHH
        /// {
        ///     AAA = 1,
        ///     BBB = 2,
        ///     CCC = 3,
        /// }
        /// 
        /// string temp = StringExtension.ConvertToSeperatedString&lt;int&gt;(',', HHH.AAA, HHH.BBB, HHH.CCC);
        /// // temp = "1,2,3"
        /// 
        /// temp = StringExtension.ConvertToSeperatedString&lt;string&gt;(',', HHH.AAA, HHH.BBB, HHH.CCC);
        /// // temp = "AAA,BBB,CCC"
        /// </code>
        /// </example>
        public static string ConvertToSeperatedString<T>( char seperator, params object [] items)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in items)
            {
                sb.Append( typeof(T) == typeof(string) ? item.ToString() : ((T)item).ToString());
                sb.Append(seperator);
            }

            return sb.ToString().Trim(seperator);
        }



        /// <summary>
        /// Convert the array list to string by comma.
        /// </summary>
        /// <typeparam name="T">Expected T type</typeparam>
        /// <param name="items">Array list</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        /// public enum HHH
        /// {
        ///     AAA = 1,
        ///     BBB = 2,
        ///     CCC = 3,
        /// }
        /// 
        /// string temp = StringExtension.ConvertToCommaSeperatedString&lt;int&gt;(HHH.AAA, HHH.BBB, HHH.CCC);
        /// // temp = "1,2,3"
        /// 
        /// temp = StringExtension.ConvertToCommaSeperatedString&lt;string&gt;( HHH.AAA, HHH.BBB, HHH.CCC);
        /// // temp = "AAA,BBB,CCC"
        /// </code>
        /// </example>
        public static string ConvertToCommaSeperatedString<T>(params object[] items)
        {
            return ConvertToSeperatedString<T>(',', items);
        }

        /// <summary>
        /// Convert the array to string by comma.
        /// </summary>
        /// <param name="items">Array</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        /// public enum HHH
        /// {
        ///     AAA = 1,
        ///     BBB = 2,
        ///     CCC = 3,
        /// }
        /// 
        /// string temp = StringExtension.ConvertToCommaSeperatedString(HHH.AAA, HHH.BBB, HHH.CCC);
        /// // temp = "AAA,BBB,CCC"
        /// 
        /// temp = StringExtension.ConvertToCommaSeperatedString( 1, 0, -1, 55);
        /// // temp = "1,0,-1,55"
        /// </code>
        /// </example>
        public static string ConvertToCommaSeperatedString(params object[] items)
        {
            return ConvertToCommaSeperatedString<string>(items);
        }



        /// <summary>
        /// Encode the string base upon base64 encoding
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Base64</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string a = "111111";
        /// string b = a.Base64Encode(Encoding.Unicode); // MQAxADEAMQAxADEA
        /// string c = b.Base64Decode(Encoding.Unicode);
        /// ]]> 
        /// </code>
        /// </example>
        public static string Base64Encode( this string text, Encoding encoding)
        {
            byte[] aryBuf = encoding.GetBytes(text);
            return Convert.ToBase64String(aryBuf);
        }


        /// <summary>
        /// Decode the base 64 to text string.
        /// </summary>
        /// <param name="encodedText">Base64 string</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Text string</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// string a = "111111";
        /// string b = a.Base64Encode(Encoding.Unicode); // MQAxADEAMQAxADEA
        /// string c = b.Base64Decode(Encoding.Unicode);
        /// ]]> 
        /// </code>
        /// </example>
        public static string Base64Decode( this string encodedText, Encoding encoding)
        {
            byte[] aryBuf = Convert.FromBase64String(encodedText);
            return encoding.GetString(aryBuf);
        } 

        /// <summary>
        /// Decode the text string by default key and UTF-16
        /// </summary>
        /// <param name="encrypted">Encrypted text string</param>
        /// <returns>Text string</returns>
        public static string DefaultDecrypt(this string encrypted)
        {
            return Security.DefaultDecrypt(encrypted);
        }

        /// <summary>
        /// Encode the text string by default key and UTF-16
        /// </summary>
        /// <param name="text">Text string</param>
        /// <returns>Encrypted string</returns>
        public static string DefaultEncrypt(this string text)
        {
            return Security.DefaultEncrypt(text);
        }
        /// <summary>
        /// Decode the text string by default key and UTF-16
        /// </summary>
        /// <param name="text">Text string</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Encrypted stirng</returns>
        public static string DefaultEncrypt(this string text, Encoding encoding)
        {
            return Security.DefaultEncrypt(text, encoding);
        }

        /// <summary>
        /// Decode the text string by default key and UTF-16
        /// </summary>
        /// <param name="encrypted">Encrypted text string</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Text string</returns>
        public static string DefaultDecrypt(this string encrypted, Encoding encoding)
        {
            return Security.DefaultDecrypt(encrypted, encoding);
        }

        public static string ScientificEncrypt(this string text)
        {
            StringBuilder sb = new StringBuilder("=\"");
            return sb.Append(text).Append("\"").ToString();
        }
    }
}
