using HomeWork0726.Common;
using HomeWork0726.DateAccess;
using System;
using System.Configuration;
using System.Reflection;

namespace HomeWork0726.Factory
{
    public class DALFactory
    {
        static DALFactory()
        {
            var assemly = Assembly.Load(StaticConstant.DALDllName);
            DAL = assemly.GetType(StaticConstant.DALTypeName);
        }
        private static Type DAL = null;
        public static IBaseDAL CreateInstance()
        {
            return (IBaseDAL) Activator.CreateInstance(DAL);
           
        }
    }
}
