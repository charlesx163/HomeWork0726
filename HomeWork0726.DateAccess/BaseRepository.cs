using HomeWork0726.Common;
using HomeWork0726.Model;
using HomeWork0726.Model.AttributeExtensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HomeWork0726.DateAccess
{

    public class BaseRepository
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
            using (SqlConnection conn=new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = comm.ExecuteReader();
                list = this.ReaderToList<T>(reader);
            }
            return list;
        }

        #region
        private List<T> ReaderToList<T>(IDataReader reader) where T:BaseModel
        {
            Type type = typeof(T);
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T t = (T)Activator.CreateInstance(type);
                foreach (var prop in type.GetProperties())
                {
                    prop.SetValue(t, reader[prop.GetColumnName()]);
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
