using System;
using System.Configuration;

namespace HomeWork0726.Common
{
    public class StaticConstant
    {
        public static string SqlConnString = System.Configuration.ConfigurationManager.ConnectionStrings["Study"].ConnectionString;
    }
}
