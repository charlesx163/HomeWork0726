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
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));mdoel与数据库实体一致不用使用特性
            string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]")); //mdoel字段与数据库实体一致,使用特性
            string sql = $"select {columnString} from [{type.Name}] where Id={id}";
            T t = (T)Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();
                var list = this.ReaderToList<T>(reader);
                t = list.FirstOrDefault();
                return t;
            }
        }

        /// <summary>
        /// 查询所有的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            //string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"));
            string columnString = string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]"));
            string sql = $"select {columnString} from [{type.Name}]";
            List<T> list = new List<T>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                list = this.ReaderToList<T>(reader);
            }
            return list;
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
        #endregion
    }
}
