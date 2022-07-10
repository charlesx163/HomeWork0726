using HomeWork0726.DateAccess;
using HomeWork0726.Factory;
using HomeWork0726.Model.Entities;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeWork0726
{
    /// <summary>
    /// 1 两个泛型的数据库访问方法，用 BaseModel约束
    /// 2 用DataReader去访问数据库，将得到的结果通过反射生成实体对象/集合返回
    /// 3 尝试用特性提供解决实数据库字段与model字段不一致的问题
    /// 4 将数据访问层抽象，使用简单工厂+配置文件+反射的方式，来提供对数据访问层的使用
    /// </summary>
    class Program
    {
        //private static IConfiguration _Configuration;
        static void Main(string[] args)
        {

            try
            {
                //var _Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
                //var conn = _Configuration.GetConnectionString("Study");
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
