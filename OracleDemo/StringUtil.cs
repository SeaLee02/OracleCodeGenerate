using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OracleDemo
{
    public class StringUtil
    {
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetUpperF(string str)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 序列化json文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static MyConfig GetConfig(string filePath = null)
        {
            if (File.Exists(filePath))
            {
                //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                //return javaScriptSerializer.Deserialize<MyConfig>(File.ReadAllText(filePath));
            }
            return null;
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string[] arry = value.Split('_');
            string str = "";
            foreach (string item in arry)
            {
                string newstr = item.Replace("(", "").Replace(".", "").Replace(")", "");
                string firstLetter = newstr.Substring(0, 1);
                string rest = newstr.Substring(1, newstr.Length - 1);
                str += firstLetter.ToUpper() + rest;
            }
            return str;
        }


        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLower(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string[] arry = value.Split('_');
            string str = "";
            foreach (string item in arry)
            {
                string newstr = item.Replace("(", "").Replace(".", "").Replace(")", "");
                string firstLetter = newstr.Substring(0, 1);
                string rest = newstr.Substring(1, newstr.Length - 1);
                str += firstLetter.ToLower() + rest;
            }
            return str;
        }


        /// <summary>
        /// 转大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToUpper(string value)
        {
            return value.ToUpper();
        }


        /// <summary>
        /// DataTable转强类型
        /// </summary>
        /// <typeparam name="T">需要转换的类型</typeparam>
        /// <param name="dt">数据源</param>
        /// <returns>返回List<T></returns>
        public static List<T> Mapper<T>(DataTable dt)
        {
            List<T> result = new List<T>();
            List<string> ColumnNameList = new List<string>();
            foreach (DataColumn item in dt.Columns)
            {
                ColumnNameList.Add(item.ColumnName);
            }
            foreach (DataRow item in dt.Rows)
            {
                T d = Activator.CreateInstance<T>();
                Type pp = typeof(T);
                PropertyInfo[] ppList = pp.GetProperties();
                foreach (PropertyInfo pro in pp.GetProperties())
                {
                    if (ColumnNameList.Contains(pro.Name.ToUpper()) && item[pro.Name.ToUpper()] != null && item[pro.Name.ToUpper()] != DBNull.Value)
                    {
                        Type type = pro.PropertyType;
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            type = type.GetGenericArguments()[0];
                        }
                        object value = Convert.ChangeType(item[pro.Name], type);
                        pro.SetValue(d, value, null);
                    }
                }
                result.Add(d);
            }
            return result;
        }
    }
}
