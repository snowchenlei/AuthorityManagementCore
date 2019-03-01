using Snow.AuthorityManagement.Enum;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Snow.AuthorityManagement.Common.Encryption
{
    public static class Md5Encryption
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="password">要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            return Encrypt(value, Md5EncryptionType.Strong);
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="password">要加密的字符串</param>
        /// <param name="mode">加密强度</param>
        /// <returns></returns>
        public static string Encrypt(string value, Md5EncryptionType mode)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("参数有误");
            }
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            string str = BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(value)));
            provider.Clear();
            if (mode != Md5EncryptionType.Strong)
            {
                return str.Replace("-", null).Substring(8, 0x10);
            }
            return str.Replace("-", null);
        }
    }
}