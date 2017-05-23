using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace CodeGenerator
{
    public class GenStoreProcedures
    {
        //Đọc tên column + kiểu dữ liệu + is null + max size và kiểm tra thuộc tính có phải identity hay foreign key
        //hay ko từ một table? Lưu tất cả thông tin vào đối tượng DBColumn
        public List<DBColumn> ReadTableColumns(string table)
        {
            List<DBColumn> columnName = new List<DBColumn>();
            string query = "select column_name, data_type, is_nullable, character_maximum_length "
                          + "from information_schema.Columns where Table_name = '" + table + "'";

            SqlConnection con = new SqlConnection(Global.connectionString);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        #region IsIdentity
                        string sql = @"Select [COLUMN_NAME] From INFORMATION_SCHEMA.Columns 
                                       Where TABLE_NAME = '" + table + "' AND COLUMNPROPERTY(OBJECT_ID('" + table + "'), '" + reader[0] + "', 'IsIdentity')=1";
                        bool check_Identity = false;
                        var con2 = new SqlConnection(Global.connectionString);
                        var cmd2 = new SqlCommand(sql, con2);
                        try
                        {
                            con2.Open();
                            check_Identity = cmd2.ExecuteScalar() != null;
                        }
                        catch (Exception)
                        {
                            check_Identity = false;
                        }
                        finally
                        {
                            if (con2.State == ConnectionState.Open)
                                con2.Close();
                            con2.Dispose();
                            cmd2.Dispose();
                        }
                        #endregion
                        #region IsPrimaryKey or IsForeignKey
                        //Chú ý: Chưa xử lý trong trường hợp primary key gồm 2 thuộc tính trở lên
                        sql = @"Select [CONSTRAINT_NAME] From INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE 
                                Where TABLE_NAME = '" + table + "' AND COLUMN_NAME = '" + reader[0].ToString() + "'";
                        bool check_FK = false;
                        bool check_PK = false;
                        var con3 = new SqlConnection(Global.connectionString);
                        var cmd3 = new SqlCommand(sql, con3);
                        try
                        {
                            con3.Open();
                            string s = cmd3.ExecuteScalar().ToString();
                            if (s != null || s != "")
                            {
                                if (s.Substring(0, 2) == "FK")
                                    check_FK = true;
                                else if (s.Substring(0, 2) == "PK")
                                    check_PK = true;
                            }
                        }
                        catch (Exception)
                        {
                            check_FK = false;
                            check_PK = false;
                        }
                        finally
                        {
                            if (con3.State == ConnectionState.Open)
                                con3.Close();
                            con3.Dispose();
                            cmd3.Dispose();
                        }
                        #endregion
                        
                        var column = new DBColumn
                        {
                            Name = reader[0].ToString(),
                            DataType = reader[1].ToString(),
                            IsNullable = reader[2].ToString() == "NO" ? false : true,
                            IsIdentity = check_Identity,
                            DataTypeLength = reader[3].ToString(),
                            IsForeignKey = check_FK,
                            IsPrimaryKey = check_PK
                        };
                        columnName.Add(column);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            return columnName;      
        }

        // Sinh store procedure Insert với format tên procedure là Tablename_Insert
        public string GenInsertSP(string table)
        {
            string command = "-- Create procedure: " + table + "_Insert \n";
            command += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + table
                       + "_Insert]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n"
                       + "\tDrop procedure [dbo].[" + table + "_Insert]" + "\nGO\n\n"
                       + "Create procedure " + table + "_Insert(\n";

            List<DBColumn> columns = ReadTableColumns(table);
            foreach (var column in columns)
            {
                if (!column.IsIdentity)  //Nếu thuộc tính của table ko phải là identity thì insert cho các thuộc tính này
                {
                    //Xử lý trong trường hợp là kiểu số hay kiểu chuỗi có độ dài, ví dụ nvarchar(30)
                    string length = column.DataTypeLength.ToString() == "" ? "" : "(" + column.DataTypeLength + ")";
                    command += "\t@" + column.Name + " " + column.DataType + length + ",\n";
                }
            }
            command = command.Remove(command.Length - 2, 2);
            command += "\n) with encryption \nAS\n  Begin\n";
            command += "\tInsert into " + table + "(";
            foreach (var column in columns)
            {
                if (!column.IsIdentity)
                {
                    command += "[" + column.Name + "], ";
                }
            }
            command = command.Remove(command.Length - 2, 2); // Xóa bỏ ký tự dư thừa là dấu ,  và khoảng trắng cuối
            command += ") \n\tValues(";
            foreach (var column in columns)
            {
                if (!column.IsIdentity)
                {
                    command += "@" + column.Name + ", ";
                }
            }
            command = command.Remove(command.Length - 2, 2);
            command += ")\n  End\nGO\n\n";

            return command;
            
        }

        // Sinh store procedure SelectAll với format tên procedure là Tablename_SelectAll
        public string GenSelectAllSP(string table)
        {
            string command = "-- Create procedure: " + table + "_SelectAll \n";
            command += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + table
                       + "_SelectAll]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n"
                       + "\tDrop procedure [dbo].[" + table + "_SelectAll]" + "\nGO\n\n"
                       + "Create procedure " + table + "_SelectAll with encryption \nAS\n  Begin\n";

            command += "\tSelect * From " + table + "\n  End\nGO\n\n";
            
            return command;

        }

        // Sinh store procedure GetById với format tên procedure là Tablename_GetById
        public string GenSelectByIdSP(string table)
        {
            string command = "-- Create procedure: " + table + "_SelectById \n";
            command += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + table
                       + "_SelectById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n"
                       + "\tDrop procedure [dbo].[" + table + "_SelectById]" + "\nGO\n\n"
                       + "Create procedure " + table + "_SelectById(\n";

            List<DBColumn> columns = ReadTableColumns(table);
            bool isCompositePrimaryKey = true;  //Kiểm tra trong trường hợp primary key có nhiều hơn 1 thuộc tính thì ko tạo procedure
            foreach (var column in columns)
            {
                if (column.IsPrimaryKey)  //Tìm kiếm theo primary key
                {
                    isCompositePrimaryKey = false;
                    //Xử lý trong trường hợp là kiểu số hay kiểu chuỗi có độ dài, ví dụ nvarchar(30)
                    string length = column.DataTypeLength.ToString() == "" ? "" : "(" + column.DataTypeLength + ")";
                    command += "\t@" + column.Name + " " + column.DataType + length + "\n";
                    break;
                }
            }
            if (isCompositePrimaryKey)
            {
                command = "-- Create procedure: " + table + "_SelectById \n";
                command += "-- The table " + table + " contains the composite primary key\n\n";
                return command;
            }

            command += ") with encryption \nAS\n  Begin\n";
            command += "\tSelect * From " + table + " Where "; 
            foreach (var column in columns)
            {
                if (column.IsPrimaryKey)
                {
                    command += column.Name + " = @" + column.Name;
                    break;
                }
            }
            command += "\n  End\nGO\n\n";

            return command;

        }

        // Sinh store procedure Delete với format tên procedure là Tablename_Delete
        public string GenDeleteSP(string table)
        {
            string command = "-- Create procedure: " + table + "_Delete \n";
            command += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + table
                       + "_Delete]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n"
                       + "\tDrop procedure [dbo].[" + table + "_Delete]" + "\nGO\n\n"
                       + "Create procedure " + table + "_Delete(\n";

            List<DBColumn> columns = ReadTableColumns(table);
            bool isCompositePrimaryKey = true;  //Kiểm tra trong trường hợp primary key có nhiều hơn 1 thuộc tính thì ko tạo procedure
            foreach (var column in columns)
            {
                if (column.IsPrimaryKey)  //Tìm kiếm theo primary key
                {
                    isCompositePrimaryKey = false;
                    //Xử lý trong trường hợp là kiểu số hay kiểu chuỗi có độ dài, ví dụ nvarchar(30)
                    string length = column.DataTypeLength.ToString() == "" ? "" : "(" + column.DataTypeLength + ")";
                    command += "\t@" + column.Name + " " + column.DataType + length + "\n";
                    break;
                }
            }
            if (isCompositePrimaryKey)
            {
                command = "-- Create procedure: " + table + "_Delete \n";
                command += "-- The table " + table + " contains the composite primary key\n\n";
                return command;
            }

            command += ") with encryption \nAS\n  Begin\n";
            command += "\tDelete From " + table + " Where ";
            foreach (var column in columns)
            {
                if (column.IsPrimaryKey)
                {
                    command += column.Name + " = @" + column.Name;
                    break;
                }
            }
            command += "\n  End\nGO\n\n";

            return command;

        }

        // Sinh store procedure Update với format tên procedure là Tablename_Update
        public string GenUpdateSP(string table)
        {
            string command = "-- Create procedure: " + table + "_Update \n";
            command += "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + table
                       + "_Update]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n"
                       + "\tDrop procedure [dbo].[" + table + "_Update]" + "\nGO\n\n"
                       + "Create procedure " + table + "_Update(\n";

            List<DBColumn> columns = ReadTableColumns(table);
            bool isCompositePrimaryKey = true;  //Kiểm tra trong trường hợp primary key có nhiều hơn 1 thuộc tính thì ko tạo procedure
            foreach (var column in columns)
            {
                if(column.IsPrimaryKey)
                {
                    isCompositePrimaryKey = false;
                }
                //Xử lý trong trường hợp là kiểu số hay kiểu chuỗi có độ dài, ví dụ nvarchar(30)
                string length = column.DataTypeLength.ToString() == "" ? "" : "(" + column.DataTypeLength + ")";
                command += "\t@" + column.Name + " " + column.DataType + length + ",\n";
            }
            if (isCompositePrimaryKey)
            {
                command = "-- Create procedure: " + table + "_Update \n";
                command += "-- The table " + table + " contains the composite primary key\n\n";
                return command;
            }

            command = command.Remove(command.Length - 2, 2); // Xóa bỏ ký tự dư thừa là dấu ,  và khoảng trắng cuối
            command += "\n) with encryption \nAS\n  Begin\n";
            command += "\tUpdate " + table + "\n\tSet ";
            foreach (var column in columns)
            {
                if (!column.IsIdentity && !column.IsPrimaryKey)
                {
                    command += column.Name + " = @" + column.Name + ", ";
                }
            }
            command = command.Remove(command.Length - 2, 2);
            command += "\n\tWhere ";
            foreach (var column in columns)
            {
                if (column.IsPrimaryKey)
                {
                    command += column.Name + " = @" + column.Name;
                    break;
                }
            }

            command += "\n  End\nGO\n\n";

            return command;

        }
    }
}
