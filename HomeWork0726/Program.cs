using HomeWork0726.DateAccess;
using HomeWork0726.Factory;
using HomeWork0726.Model.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeWork0726
{
    class Program
    {
        private static IConfiguration _Configuration;
        static void Main(string[] args)
        {

            try
            {
                var _Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                var conn = _Configuration.GetConnectionString("Study");
                IBaseDAL dal = DALFactory.CreateInstance();
                Company c = dal.Find<Company>(1);
                c.Name = "";
                dal.Update<Company>(c);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
           
        }

        private static void Show<T>(T t)
        {
            //显示中文名称
        }
    }
}
