using System;
using System.Configuration;

namespace HomeWork0726.Common
{
    public class StaticConstant
    {
        public static string SqlConnString = ConfigurationManager.ConnectionStrings["Study"].ConnectionString;
        private static string DALTypeDLL =ConfigurationManager.AppSettings["DALTypeDLL"].ToString();
        public static string DALDllName = DALTypeDLL.Split(',')[0];
        public static string DALTypeName = DALTypeDLL.Split(',')[1];
    }
}
