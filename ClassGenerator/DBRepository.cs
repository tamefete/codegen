using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ClassGenerator
{   
    public static class DBRepository
    {
        public static List<String> ListTables(String connectionString)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                SqlDataReader reader = cmd.ExecuteReader();

                List<String> list = new List<String>();
                while (reader.Read())
                {
                    list.Add(reader["TABLE_NAME"].ToString());
                }

                reader.Close();
                reader.Dispose();
                con.Close();
                con.Dispose();                

                return list;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Class Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<String>();
            }
        }

        public static List<DBColumn> ListColumns(String connectionString, String TableName)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT COLUMN_NAME, IS_NULLABLE, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_Name = '" + TableName + "'";

                SqlDataReader reader = cmd.ExecuteReader();

                List<DBColumn> list = new List<DBColumn>();

                while (reader.Read())
                {
                    DBColumn col = new DBColumn();
                    col.Name = reader["COLUMN_NAME"].ToString();
                    col.IsNullable = reader["IS_NULLABLE"].ToString().ToLower() == "yes" ? true : false;
                    col.DataType = MapFieldToManagedType(reader["DATA_TYPE"].ToString());
                    list.Add(col);
                }

                reader.Close();
                reader.Dispose();
                con.Close();
                con.Dispose();

                return list;            
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Class Generator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<DBColumn>();
            }
        }

        private static Type MapFieldToManagedType(String fieldType)
        {
            switch (fieldType)
            {
                case "bigint":
                    return typeof(Int32);
                case "binary":
                    return typeof(Byte);
                case "bit":
                    return typeof(Boolean);
                case "char":
                    return typeof(String);
                case "date":
                    return typeof(DateTime);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(Decimal);
                case "float":
                    return typeof(float);                
                case "int":
                    return typeof(Int32);
                case "numeric":
                    return typeof(Decimal);
                case "money":
                    return typeof(Decimal);
                case "nchar":
                    return typeof(String);
                case "ntext":
                    return typeof(String);
                case "nvarchar":
                    return typeof(String);
                case "smalldatetime":
                    return typeof(DateTime);
                case "smallint":
                    return typeof(Int16);
                case "text":
                    return typeof(String);
                case "tinyint":
                    return typeof(Int16);
                case "varbinary":
                    return typeof(Byte);
                case "varchar":
                    return typeof(String);
                case "uniqueidentifier":
                    return typeof(Guid);
                case "real":
                    return typeof(Double);
                case "xml":
                    return typeof(System.Data.SqlTypes.SqlXml);
                default:
                    return typeof(String);
            }
        }
    }
}

