using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using DropDownBoxLinkage.Models;

namespace DropDownBoxLinkage.DBOperator
{
    public class AddressHelper
    {
        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString; }
        }

        public List<Province> GetAllProvince()
        {
            List<Province> provinceLists = new List<Province>();
            string sql = @"select * from dbo.Province";

            SqlConnection conn = new SqlConnection(ConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                provinceLists.Add(new Province()
                {
                    ProvinceID = Convert.ToInt32(reader["ProvinceID"]),
                    ProvinceName = reader["ProvinceName"].ToString()
                });
            }

            conn.Close();
            reader.Close();

            return provinceLists;
        }

        public List<City> GetCityListByProvinceID(int id)
        {
            DataSet ds = new DataSet();
            string sql = @"select CityID,CityName from dbo.City where ProvinceID = @ProvinceID";

            SqlConnection conn = new SqlConnection(ConnectionString);

            SqlDataAdapter sda = new SqlDataAdapter(sql,conn);

            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.Parameters.AddWithValue("@ProvinceID", id);

            conn.Open();

            sda.Fill(ds);

            conn.Close();

            return DataTableToList<City>.ConvertToModel(ds.Tables[0]).ToList<City>();
        }

        public static class DataTableToList<T> where T : new()
        {
            public static IList<T> ConvertToModel(DataTable dt)
            {
                IList<T> ts = new List<T>();
                T t = new T();
                string tempName = "";

                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (DataRow row in dt.Rows)
                {
                    t = new T();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if(!pi.CanWrite)
                                continue;
                            object value = row[tempName];
                            if(value != DBNull.Value)
                                pi.SetValue(t,value,null);
                        }
                    }

                    ts.Add(t);
                }
                return ts;
            }
        }
    }
}