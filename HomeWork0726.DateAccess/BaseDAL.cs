using HomeWork0726.Common;
using HomeWork0726.Common.AttributeExtensions;
using HomeWork0726.Common.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HomeWork0726.DateAccess
{

    public class BaseDAL:IBaseDAL
    {
        private static string ConnectionString = StaticConstant.SqlConnString;

        /// <summary>
        /// 查询的单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            #region 使用泛型缓存之前
            /*如果放在这里每次都要做反射去拼装sql，
             * 如果把这一步到GenericSqlHelper<T>泛型里，
             * 利用静态构造函数,对于相同的类，只会在第一次时去做反射拼装sql,在第一次做完之后，结果就会被缓存起来，
             * 当下次再有相同的类做此操作时就可以直接去拿第一次的结果,无需在去做反射拼装sql
             * 泛型+反射 做缓存
             */
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));mdoel与数据库实体一致不用使用特性
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]")); //mdoel字段与数据库实体一致,使用特性
            //string sql = $"select {columnString} from [{type.Name}] where Id={id}";
            #endregion
            #region 使用泛型缓存之后
            string sql = $"{GenericSqlHelper<T>.FindSingleSql}{id}";
            #endregion
            T t = null;//(T)Activator.CreateInstance(type);

            #region 使用委托之前
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    SqlCommand command = new SqlCommand(sql, conn);
            //    conn.Open();
            //    SqlDataReader reader = command.ExecuteReader();
            //    var list = this.ReaderToList<T>(reader);
            //    t = list.FirstOrDefault();
            //    return t;
            //}
            #endregion

            #region 使用委托
            Func<SqlCommand, T> func = (command) => {
                SqlDataReader reader = command.ExecuteReader();
                var list = this.ReaderToList<T>(reader);
                t = list.FirstOrDefault();
                return t;
            };
            var result = InternalExcute<T>(sql,func);
            return result;
            #endregion
        }

        /// <summary>
        /// 查询所有的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);

            #region 使用泛型缓存之前:
            /*如果放在这里每次都要做反射去拼装sql，
             * 如果把这一步到GenericSqlHelper<T>泛型里，
             * 利用静态构造函数,对于相同的类，只会在第一次时去做反射拼装sql,在第一次做完之后，结果就会被缓存起来，
             * 当下次再有相同的类做此操作时就可以直接去拿第一次的结果,无需在去做反射拼装sql
             * 泛型+反射 做缓存
             */
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));//使用特性之前
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]"));//使用特性之后
            //string sql = $"select {columnString} from [{type.Name}]";
            #endregion
            #region 使用泛型缓存之后
            string sql = GenericSqlHelper<T>.FindAllSql;
            #endregion
            List<T> list = new List<T>();
            #region 使用委托前
            //using (SqlConnection conn = new SqlConnection(ConnectionString))
            //{
            //    SqlCommand comm = new SqlCommand(sql, conn);
            //    conn.Open();
            //    SqlDataReader reader = comm.ExecuteReader();
            //    list = this.ReaderToList<T>(reader);
            //}
            #endregion
            #region 使用委托
            Func<SqlCommand, List<T>> func = (command) =>
            {
                SqlDataReader reader = command.ExecuteReader();
                var list = this.ReaderToList<T>(reader);
                return list;
            };
            var resultList = InternalExcute<List<T>>(sql, func);
            #endregion
            return resultList;
        }

        /// <summary>
        /// update data by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t) where T:BaseModel
        {
            if(!t.Validate<T>())
            {
                throw new Exception("数据不正确");
            }
            Type type = typeof(T);
            var properties = type.GetProperties().Where(p => !p.Name.Equals("Id"));
            var paras = properties.Select(p => new SqlParameter($"@{p.GetColumnName()}", p.GetValue(t) ?? DBNull.Value));
            string ColumnName = string.Join(",", properties.Select(p => $"[{p.GetColumnName()}]=@{p.GetColumnName()}"));
            string sql = $"UPDATE [{type.Name}] SET {ColumnName} where Id={t.Id}";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddRange(paras.ToArray());
                conn.Open();
                var i = comm.ExecuteNonQuery();
                if (i == 0)
                    throw new Exception("the data does not exeit in the database");
            }
        }

        #region
        private List<T> ReaderToList<T>(IDataReader reader) where T : BaseModel
        {
            Type type = typeof(T);
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T t = (T)Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    object value = reader[prop.GetColumnName()];
                    if (value is DBNull)
                        value = null;
                    prop.SetValue(t, value);
                }
                list.Add(t);
            }
            return list;
        }

        private string GetSql(int? Id)
        {
            return string.Empty;
        }

        //通过委托去掉访问数据的重复的代码
        private T InternalExcute<T>(string sql, Func<SqlCommand,T> func)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction =conn.BeginTransaction();
                try
                {
                    SqlCommand comm = new SqlCommand(sql, conn);
                    comm.Transaction = transaction;
                    T result = func.Invoke(comm);
                    transaction.Commit();
                    return result;
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
                
            }
        }
        #endregion
    }
}
