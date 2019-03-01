using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snow.AuthorityManagement.Common
{
    public class EnumHelper
    {
        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription<T>(T enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs == null || objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T">Attribute类型</typeparam>
        /// <param name="enumValue"></param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(Type enumValue) where T : Attribute
        {
            Type type = enumValue.GetType();
            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(T), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return (T)attrs[0];
                }
            }
            return default(T);
        }

        public static S GetAttribute<T, S>(T enumValue) where S : Attribute
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(S), false);
            if (objs != null && objs.Length > 0)
            {

                return (S)objs[0];
            }
            return default(S);
        }
    }
}
