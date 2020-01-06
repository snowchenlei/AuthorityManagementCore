using System;
using System.Collections.Generic;
using System.Text;

namespace Snow.AuthorityManagement.Core
{
    public class AuthorityManagementConsts
    {
        #region CacheKey

        /// <summary>
        /// User响应Chche Key
        /// </summary>
        public const string MenuTreeCache = "Menu_Tree_All";

        #endregion CacheKey

        public const string ConnectionStringName = "Default";
        public const int MaxNameLength = 64;
        public const int MaxUserNameLength = 256;
        public const int MaxEmailAddressLength = 256;
        public const int MaxPasswordLength = 128;
        public const int MaxPhoneNumberLength = 32;
    }
}