using HomeWork0726.Common.Model;
using HomeWork0726.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork0726.DateAccess
{
    public interface IBaseDAL
    {
        /// <summary>
        /// 查询的单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(int id) where T : BaseModel;

        /// <summary>
        /// 查询所有的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> FindAll<T>() where T : BaseModel;

        /// <summary>
        /// update data by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        void Update<T>(T t) where T : BaseModel;
    }
}
