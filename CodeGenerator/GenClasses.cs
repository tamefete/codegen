using System;
using System.Data;
using System.Data.SqlClient;

namespace CodeGenerator
{
    public class GenClasses
    {
        #region "Var"
        private bool AutoNumber;  // Kiểm tra xem trong table có thuộc tính identity hay ko?
        private DataTable mData;  // Lưu danh sách các columns trong table
        private string tblName;  // Lưu tên table
        private string obj; // Tên biến khai báo trong phát sinh code cho các class

        private string[] ColName = new string[100];
        private string[] ColType = new string[100];
        private string[] DataType = new string[100];
        private bool[] PrimaryKey = new bool[100];
        private int PrimaryKeyCount = 0;
        private int Leng;  // Số thuộc tính trong một table
        private string _namesp = "";
        private string _connString = "";
        #endregion

        #region "Constructor"
        public GenClasses(string tablename, string _namespace, string connstring)
        {
            _namesp = _namespace;
            _connString = connstring;
            tblName = tablename;
            //Xử lý cách đặt tên biến tương ứng với tên table như chuyển sang chữ thường và bỏ ký tự số nhiều 's' nếu có trong tiếng Anh
            obj = tblName;
            obj = obj.ToLower().TrimEnd('s'); 

            mData = LoadAllField(tablename);
            DataTable pk = LoadPrimaryKey(tablename);
            Leng = mData.Rows.Count;

            int i;
            for (i = 0; i < Leng; i++)
            { 
                ColName[i] = mData.Rows[i]["name"].ToString();
                ColType[i] = mData.Rows[i]["typeName"].ToString(); 
                DataType[i] = mData.Rows[i]["typeName"].ToString();
                //Convert from SQL Server data type to C# data type
                ColType[i] = MappingSQLServerToCSharpDataType(ColType[i]);                
            }

            if (pk.Rows.Count == 0)
            {
                PrimaryKey[0] = true;
                PrimaryKeyCount = 1;
            }
            else
            {
                for (i = 0; i < Leng; i++)
                {
                    PrimaryKey[i] = IsPrimary(ColName[i], pk);
                    if (PrimaryKey[i])
                        PrimaryKeyCount += 1;
                }
            }

            AutoNumber = false;  //Kiểm tra có thuộc tính nào identity trong table hay ko?
            for (i = 0; i < Leng; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == true)
                    AutoNumber = true;
            }
        }
        #endregion

        #region "Methods"

        //Mapping kiểu dữ liệu trong MS SQL Server sang kiểu dữ liệu tương ứng trong C#
        //https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx
        private string MappingSQLServerToCSharpDataType(string sqlDataType)
        {
            switch (sqlDataType)
            {
                case "bigint":
                    return "Int64";
                case "binary":
                    return "Byte[]";
                case "bit":
                    return "Boolean";
                case "char":
                    return "String";
                case "date":
                    return "DateTime";
                case "datetime":
                    return "DateTime";
                case "datetime2":
                    return "DateTime";
                case "datetimeoffset":
                    return "DateTimeOffset";
                case "decimal":
                    return "Decimal";
                case "float":
                    return "Double";
                case "int":
                    return "Int32";
                case "numeric":
                    return "Decimal";
                case "money":
                    return "Decimal";
                case "nchar":
                    return "String";
                case "ntext":
                    return "String";
                case "nvarchar":
                    return "String";
                case "smalldatetime":
                    return "DateTime";
                case "smallint":
                    return "Int16";
                case "smallmoney":
                    return "Decimal";
                case "text":
                    return "String";
                case "tinyint":
                    return "Byte";
                case "varbinary":
                    return "Byte[]";
                case "varchar":
                    return "String";
                case "uniqueidentifier":
                    return "Guid";
                case "time":
                    return "TimeSpan";
                case "real":
                    return "Single";
                case "xml":
                    return "System.Data.SqlTypes.SqlXml";
                default:
                    return "String";
            }
        }

        private bool IsPrimary(string col, DataTable pk)
        {
            for (int i = 0; i < pk.Rows.Count; i++)
            {
                if (col == (string)pk.Rows[i][0])
                {
                    return true;
                }
            }

            return false;
        }

        private DataTable LoadAllField(string tablename)
        {
            //string strSQL = "SELECT A.name, B.name As typeName, A.length, B.length As sizeLength, A.Autoval "
            //               + " FROM SysColumns A, SysTypes B, SysObjects C "
            //               + " WHERE A.id=C.id AND A.xtype=B.xtype AND B.name<>'sysname' "
            //               + " AND C.name='" + tablename + "' ORDER BY A.ColOrder";
            /*
             * Truy vấn các thông tin về column trong một table như: 
             *   column name, data type
             * */
            string strSQL = "SELECT C.name, T.name As typeName, C.max_length, C.is_nullable, C.is_identity "
                           + "FROM sys.columns As C " 
                           + "INNER JOIN sys.types As T "
                           + "ON T.system_type_id = C.system_type_id "
                           + "WHERE C.object_id = OBJECT_ID('" + tablename + "') AND T.name <> 'sysname' "
                           + "ORDER BY C.column_id ";

            SqlDataAdapter da = new SqlDataAdapter(strSQL, _connString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        private DataTable DanhSach(string tablename)
        {
            string strSQL = "SELECT column_name as 'Column Name', data_type as 'Data Type', character_maximum_length FROM information_schema.columns WHERE table_name = '" + tblName + "' ";

            SqlDataAdapter da = new SqlDataAdapter(strSQL, _connString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        private DataTable LoadPrimaryKey(string tablename)
        {
            string strSQL = "select c.name " + " from sysindexes i " + " join sysobjects o ON i.id = o.id "
                           + " join sysobjects pk ON i.name = pk.name " + " AND pk.parent_obj = i.id "
                           + " AND pk.xtype = 'PK' " + " join sysindexkeys ik on i.id = ik.id "
                           + " and i.indid = ik.indid  " + " join syscolumns c ON ik.id = c.id  "
                           + " AND ik.colid = c.colid  " + " where o.name = '" + tablename + "'  "
                           + " order by ik.keyno";

            SqlDataAdapter da = new SqlDataAdapter(strSQL, _connString);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private string Header1()
        {
            string code = "";
            code += "-- ==========================================================================" + "\n";
            code += "-- Author : Phan Minh Tâm - K27A.HCM.KTPM" + "\n";
            code += "-- Created Date : " + System.DateTime.Now.ToLongDateString() + "\n";
            code += "-- ==========================================================================" + "\n" + "\n" + "\n";
            return code;
        }

        private string Header()
        {
            string code = "";
            code += "//-------------------------------------------------------------------------------------" + "\n";
            code += "// Author : Phan Minh Tâm - K27A.HCM.KTPM" + "\n";
            code += "// Created Date : " + System.DateTime.Now.ToLongDateString() + "\n";
            code += "//-------------------------------------------------------------------------------------" + "\n" + "\n" + "\n";
            return code;
        }

        private string GetInitValue(string coltype)
        {
            switch (coltype)
            {
                case "Int64":
                case "Int32":
                case "Int16":
                case "Byte":
                case "Decimal":
                case "Double":
                case "Single":
                    return 0.ToString();
                case "object":
                    return "null";
                case "String":
                    // hai dau nhay ""
                    return "\"\"";
                case "Boolean":
                    return "false";
                case "DateTime":
                case "TimeSpan":
                    return "DateTime.Now";
            }
            return "\"\"";
        }

        private string new1Info()
        {
            string code = "";
            code += "\t" + "\t" + "public " + tblName + "_Entities()" + "\n";
            code += "\t" + "\t" + "{" + "\n" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";
            return code;
        }

        private string new1_DAL()
        {
            string code = "";
            code += "\t" + "\t" + "public " + tblName + "_DAL()\n\t\t{\n\n\t\t}\n" + "\n";
            return code;
        }

        private string new1_BLL()
        {
            string code = "";
            code += "\t" + "\t" + "public " + tblName + "_BLL()\n\t\t{\n\n\t\t}\n" + "\n";
            return code;
        }

        private string Insert_BLL()
        {
            string code = "";
            code += "\t" + "\t" + "public int Insert(" + _namesp + "_Entities." + tblName + "_Entities " + obj + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "return obj.Insert(" + obj + ");" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Insert_GUI()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public int Insert()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + _namesp + "_Entities." + tblName + "_Entities " + obj + " = new " + _namesp + "_Entities." + tblName + "_Entities();" + "\n";
            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";

            for (i = 0; i < Leng; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += "\t" + "\t" + "\t" + "\t" + obj + "." + ColName[i] + " = " + "\"" + "value" + "\"" + ";" + "\n";
                }
            }
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.Insert(" + obj + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "if (result > 0)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Add Success!" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "else" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Error! Please try again" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Update_GUI()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public int Update()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + _namesp + "_Entities." + tblName + "_Entities  " + obj + " = new " + _namesp + "_Entities." + tblName + "_Entities();" + "\n";
            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            for (i = 0; i < Leng; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += "\t" + "\t" + "\t" + "\t" + obj + "." + ColName[i] + " = " + "\"" + "value" + "\"" + ";" + "\n";
                }
            }
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.Update(" + obj + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "if (result > 0)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Update Success" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "else" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Error! Please try again" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";

            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Del_GUI()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public int Delete()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + _namesp + "_Entities." + tblName + "_Entities " + obj + " = new " + _namesp + "_Entities." + tblName + "_Entities();" + "\n";
            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";

            for (i = 0; i < Leng; i++)
            {
                if (PrimaryKey[i])
                {
                    code += "\t" + "\t" + "\t" + "\t" + obj + "." + ColName[i] + " = " + "\"" + "value" + "\"" + ";" + "\n";
                }
            }

            code += "\t" + "\t" + "\t" + "\t" + "result = obj.Delete(" + obj + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "if (result > 0)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Delete Success" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "else" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "MessageBox.Show(" + "\"" + "Error! Please try again" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Load_GUI()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public DataTable SelectByID()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + _namesp + "_Entities." + tblName + "_Entities " + obj + " = new " + _namesp + "_Entities." + tblName + "_Entities();" + "\n";
            code += "\t" + "\t" + "\t" + "DataTable result = new DataTable();" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";

            for (i = 0; i < Leng; i++)
            {
                if (PrimaryKey[i])
                {
                    code += "\t" + "\t" + "\t" + "\t" + obj + "." + ColName[i] + " = " + "\"" + "value" + "\"" + "; //Thay value bang gia tri cua cac controls tuong ung tren form vao" + "\n";
                }
            }

            code += "\t" + "\t" + "\t" + "\t" + "result = obj.SelectByID(" + obj + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string LoadTable_GUI()
        {
            string code = "";
            code += "\t" + "\t" + "public DataTable SelectAll()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "DataTable result = new DataTable();" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.SelectAll();" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }


        private string Insert_Store()
        {
            string code = "";
            int i;
            code += "\t" + "if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + tblName + "_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)" + "\n";
            code += "\t" + "drop table [dbo].[" + tblName + "_Insert]" + "\n";
            code += "\t" + "GO" + "\n";
            code += "\t" + "CREATE PROCEDURE " + tblName + "_Insert" + "\n";
            // AutoNumber = False
            for (i = 0; i <= Leng - 2; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += "\t" + "@" + ColName[i] + " " + DataType[i] + " " + DataType.Length.ToString() + "," + "\n";
                }
            }

            code += "\t" + "@" + ColName[Leng - 1] + " " + mData.Rows[i]["typeName"] + "\n";
            code += "\t" + "AS" + "\n";
            code += "\t" + "INSERT INTO " + tblName + " (";

            for (i = 0; i <= Leng - 2; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += ColName[i] + ", ";
                }
            }
            code += ColName[Leng - 1] + ") " + "\n" + "\t" + " VALUES (";
            for (i = 0; i <= Leng - 2; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += "@" + ColName[i] + ", ";
                }
            }
            code += "@" + ColName[Leng - 1] + ")" + "\n";
            code += "Go" + "\n" + "\n" + "\n" + "\n";
            return code;
        }

        private string Insert()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public int Insert(" + _namesp + "_Entities." + tblName + "_Entities " + obj + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";

            if (AutoNumber)  // Nếu có thuộc tính indentity trong table thì loại bỏ thuộc tính này trong procedure insert
            {
                code += "\t\t\t\t" + "//Co thuoc tinh la identity nen loai thuoc tinh nay khi insert \n";
                code += "\t\t\t\t" + "string[] a = new string[" + (Leng - 1).ToString() + "];" + "\n";
                code += "\t\t\t\t" + "object[] b = new object[" + (Leng - 1).ToString() + "];" + "\n";
                for (i = 0; i < Leng; i++)
                {
                    if ((bool)mData.Rows[i]["is_identity"] == false)  //Loại bỏ thuộc tính identity trong procedure insert 
                    {
                        code += "\t" + "\t" + "\t" + "\t" + "a[" + (i-1).ToString() + "] = \"@" + ColName[i] + "\";" + "\n";
                        code += "\t" + "\t" + "\t" + "\t" + "b[" + (i-1).ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                    }
                }
                code += "\t" + "\t" + "\t" + "\t" + "result = obj.UpdateData(" + "\"" + tblName + "_Insert" + "\"" + ", a, b, " + (Leng-1).ToString() + ");" + "\n";
            }
            else
            {
                code += "\t" + "\t" + "\t" + "\t" + "string[] a = new string[" + Leng.ToString() + "];" + "\n";
                code += "\t" + "\t" + "\t" + "\t" + "object[] b = new object[" + Leng.ToString() + "];" + "\n";

                for (i = 0; i < Leng; i++)
                {
                    code += "\t" + "\t" + "\t" + "\t" + "a[" + i.ToString() + "] = \"@" + ColName[i] + "\";" + "\n";
                    code += "\t" + "\t" + "\t" + "\t" + "b[" + i.ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                }
                code += "\t" + "\t" + "\t" + "\t" + "result = obj.UpdateData(" + "\"" + tblName + "_Insert" + "\"" + ", a, b, " + Leng.ToString() + ");" + "\n";
            }
                        
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Update_BLL()
        {
            string code = "";
            code += "\t" + "\t" + "public int Update(" + _namesp + "_Entities." + tblName + "_Entities " + obj + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "return obj.Update(" + obj + ");" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Update_store()
        {
            string code = "";
            int i;
            code += "\t" + "CREATE PROCEDURE " + tblName + "_Update" + "\n";

            for (i = 0; i <= Leng - 2; i++)
            {
                if ((bool)mData.Rows[i]["is_identity"] == false)
                {
                    code += "\t" + "@" + ColName[i] + " " + mData.Rows[i]["typeName"] + "," + "\n";
                }
            }
            code += "\t" + "@" + ColName[Leng - 1] + " " + mData.Rows[i]["typeName"] + "\n";
            code += "\t" + "AS" + "\n";

            code += "\t" + "\t" + "UPDATE " + tblName + " SET " + "\n";
            for (i = 0; i <= Leng - 1; i++)
            {
                if (!PrimaryKey[i])
                {
                    code += "\t" + "\t" + "\t" + ColName[i] + " = @" + ColName[i] + "," + "\n";
                }
            }
            code = code.Substring(0, code.Length - 5) + "\n";
            code += "\t" + "\t" + "\t" + " WHERE (";
            for (i = 0; i <= Leng - 1; i++)
            {
                if (PrimaryKey[i])
                {
                    code += ColName[i] + " = @" + ColName[i] + ") AND (";
                }
            }
            code = code.Substring(0, code.Length - 6) + "\n";
            code += "Go" + "\n" + "\n" + "\n" + "\n";
            return code;
        }

        private string Update()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public int Update(" + _namesp + "_Entities." + tblName + "_Entities " + obj + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "string[] a = new string[" + Leng.ToString() + "];" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "object[] b = new object[" + Leng.ToString() + "];" + "\n";

            for (i = 0; i < Leng; i++)
            {
                if (!PrimaryKey[i])
                {
                    code += "\t" + "\t" + "\t" + "\t" + "a[" + i.ToString() + "] = \"@" + ColName[i] + "\";" + "\n";
                    code += "\t" + "\t" + "\t" + "\t" + "b[" + i.ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                }
            }
            for (i = 0; i < Leng; i++)
            {
                if (PrimaryKey[i])
                {
                    code += "\t" + "\t" + "\t" + "\t" + "a[" + i.ToString() + "] = \"@" + ColName[i] + "\";" + "\n";
                    code += "\t" + "\t" + "\t" + "\t" + "b[" + i.ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                }
            }
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.UpdateData(" + "\"" + tblName + "_Update" + "\"" + ", a, b, " + Leng.ToString() + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private int GetIndexPrimary(int count)
        {
            for (int i = 0; i <= Leng - 1; i++)
            {
                if (PrimaryKey[i])
                {
                    count -= 1;
                    if (count == -1)
                        return i;
                }
            }
            return -1;
        }

        private string Del_BLL()
        {
            string code = "";
            int i;
            code += "\t" + "\t" + "public int Delete(";
            for (i = 0; i <= Leng - 1; i++)
            {
                //+ tblName + "Info my" + tblName +
                if (PrimaryKey[i])
                {
                    code += _namesp + "_Entities." + tblName + "_Entities " + obj + ", ";
                }
            }
            code = code.Substring(0, code.Length - 2) + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + " return obj.Delete(" + obj + ");" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Del()
        {
            string code = "";
            int i;
            code += "\t" + "\t" + "public int Delete(";
            for (i = 0; i <= Leng - 1; i++)
            {
                //+ tblName + "Info my" + tblName +
                if (PrimaryKey[i])
                {
                    code += _namesp + "_Entities." + tblName + "_Entities " + obj + ", ";
                }
            }
            code = code.Substring(0, code.Length - 2) + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";

            code += "\t" + "\t" + "\t" + "int result = 0;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "string[] a = new string[1];" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "object[] b = new object[1];" + "\n";

            //code += "\t" + "\t" + "\t" + "\t" + "ArrayList param = new ArrayList();" + "\n"
            for (i = 0; i <= PrimaryKeyCount - 1; i++)
            {
                int idx = GetIndexPrimary(i);
                if (idx != -1)
                {
                    code += "\t" + "\t" + "\t" + "\t" + "a[" + i.ToString() + "] = \"@" + ColName[idx] + "\";" + "\n";
                    code += "\t" + "\t" + "\t" + "\t" + "b[" + i.ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                }
            }
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.UpdateData(" + "\"" + tblName + "_Delete" + "\"" + ", a, b, 1);" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Load_BLL()
        {
            string code = "";
            int i;
            code += "\t" + "\t" + "public DataTable SelectByID(";
            for (i = 0; i < Leng; i++)
            {
                if (PrimaryKey[i])
                {
                    code += _namesp + "_Entities." + tblName + "_Entities " + obj + ", ";
                }
            }
            code = code.Substring(0, code.Length - 2) + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + " return obj.SelectByID(" + obj + ");" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string Load()
        {
            string code = "";
            int i;

            code += "\t" + "\t" + "public DataTable SelectByID(";
            for (i = 0; i < Leng; i++)
            {
                if (PrimaryKey[i])
                {
                    code += _namesp + "_Entities." + tblName + "_Entities " + obj + ", ";
                }
            }
            code = code.Substring(0, code.Length - 2) + ")" + "\n";
            code += "\t" + "\t" + "{" + "\n";

            code += "\t" + "\t" + "\t" + "DataTable result = null;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";

            code += "\t" + "\t" + "\t" + "\t" + "string[] a = new string[1];" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "object[] b = new object[1];" + "\n";

            for (i = 0; i <= PrimaryKeyCount - 1; i++)
            {
                int idx = GetIndexPrimary(i);

                if (idx != -1)
                {
                    code += "\t" + "\t" + "\t" + "\t" + "a[" + i.ToString() + "] = \"@" + ColName[idx] + "\";" + "\n";
                    code += "\t" + "\t" + "\t" + "\t" + "b[" + i.ToString() + "] = " + obj + "." + ColName[i] + ";" + "\n";
                }
            }
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.LoadData( " + "\"" + tblName + "_SelectByID" + "\"" + ", a, b, 1);" + "\n";

            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string LoadTable_BLL()
        {
            string code = "";
            code += "\t" + "\t" + "public DataTable SelectAll()" + "\n";
            code += "\t" + "\t" + "{" + "\n";

            code += "\t" + "\t" + "\t" + " return obj.SelectAll();" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        private string LoadTable()
        {
            string code = "";

            //code += "\t" + "\t" + "private const string LOAD_LIST_" + tblName + " = ""SELECT * FROM " + tblName + """;" + "\n"
            code += "\t" + "\t" + "public DataTable SelectAll()" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "DataTable result = null;" + "\n";
            code += "\t" + "\t" + "\t" + "try" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "result = obj.LoadData(" + "\"" + tblName + "_SelectAll" + "\"" + ");" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "catch(Exception ex)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "throw ex;" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return result;" + "\n";
            code += "\t" + "\t" + "}" + "\n" + "\n";

            return code;
        }

        #endregion

        #region "main Function"
        public string GetInfo()
        {
            string code = Header();
            int i;
            code += "// Khai bao lop" + "\n";
            code += "using System;" + "\n" + "\n";
            code += "namespace " + _namesp + "_Entities" + "\n" + "{" + "\n";
            code += "\t" + "public class " + tblName + "_Entities" + "\n";
            code += "\t" + "{" + "\n";

            code += "\t" + "\t" + "// Ham khoi tao khong tham so" + "\n";
            code += "\t" + "\t" + "#region Constructor" + "\n";
            code += new1Info();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";

            code += "\t" + "\t" + "// Khai bao cac bien" + "\n";
            code += "\t" + "\t" + "#region vars" + "\n";
            for (i = 0; i < Leng; i++)
            {
                code += "\t" + "\t" + "private " + ColType[i] + " _" + ColName[i] + " = " + GetInitValue(ColType[i]) + ";" + "\n";
            }
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";

            code += "\t" + "\t" + "// Cac thuoc tinh" + "\n";
            code += "\t" + "\t" + "#region Property" + "\n";            
            for (i = 0; i <= mData.Rows.Count - 1; i++)
            {
                code += "\t" + "\t" + "public " + ColType[i] + " " + ColName[i] + "\n";
                code += "\t" + "\t" + "{" + "\n";
                code += "\t" + "\t" + "\t" + "get { return _" + ColName[i] + "; }" + "\n";

                code += "\t" + "\t" + "\t" + "set { _" + ColName[i] + " = value; }" + "\n";
                code += "\t" + "\t" + "}" + "\n\n";
            }
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";

            // Đóng Class
            code += "\t" + "}" + "\n";
            code += "}" + "\n" + "\n" + "\n";
            return code;
        }

         public string Get_Chuoi()
        {
            string code = Header();
            code += "// Khai bao lop" + "\n";
            code += "using System;" + "\n";
            code += "using System.Linq;" + "\n";
            code += "using System.Text;" + "\n";
            code += "using System.Data;" + "\n";
            code += "using System.Data.SqlClient;" + "\n";
            code += "using System.Collections;" + "\n";
            code += "namespace " + _namesp + "_DAL" + "\n" + "{" + "\n";
            code += "\t" + "public class KetNoi" + "\n";
            code += "\t" + "{" + "\n";
            //code += "\t" + "\t" + "// Ham khoi tao khong tham so" + "\n"
            code += "\t" + "\t" + "#region Mo Ket Noi" + "\n";


            code += "\t" + "\t" + " private SqlConnection con;" + "\n";
            code += "\t" + "\t" + " public void MoKetNoi()" + "\n";
            code += "\t" + "\t" + " {" + "\n";
            //string[] strConnection = System.IO.File.ReadAllLines("ChuoiKN.txt");
            code += "\t" + "\t" + "\t" + "string chuoikn = " + "\"" + Global.connectionString + "\"" + ";" + "\n";
            code += "\t" + "\t" + "\t" + "con = new SqlConnection(chuoikn);" + "\n";
            code += "\t" + "\t" + "\t" + "con.Open();//Mo Ket Noi" + "\n";
            code += "\t" + "\t" + " }" + "\n";

            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "#region Dong Ket Noi" + "\n";


            code += "\t" + "\t" + " public void DongKetNoi()" + "\n";
            code += "\t" + "\t" + " {" + "\n";
            code += "\t" + "\t" + "\t" + "con.Close();//Dong Ket Noi" + "\n";
            code += "\t" + "\t" + " }" + "\n";

            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "#region Ham Xu Ly" + "\n";
            code += "\t" + "\t" + "public DataTable LoadData(string sql)" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "MoKetNoi();" + "\n";
            code += "\t" + "\t" + "\t" + "SqlCommand command = new SqlCommand(sql, con);" + "\n";
            code += "\t" + "\t" + "\t" + "command.CommandType = CommandType.StoredProcedure;" + "\n";
            code += "\t" + "\t" + "\t" + "SqlDataAdapter adapter = new SqlDataAdapter(command);" + "\n";
            code += "\t" + "\t" + "\t" + "DataTable dt = new DataTable();" + "\n";
            code += "\t" + "\t" + "\t" + "adapter.Fill(dt);" + "\n";
            code += "\t" + "\t" + "\t" + "return dt;" + "\n";
            code += "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "public DataTable LoadData(string sql, string[] name, object[] value, int nparameter)" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "MoKetNoi();" + "\n";
            code += "\t" + "\t" + "\t" + "SqlCommand command = new SqlCommand(sql, con);" + "\n";
            code += "\t" + "\t" + "\t" + "command.CommandType = CommandType.StoredProcedure;" + "\n";
            code += "\t" + "\t" + "\t" + "for (int i = 0; i < nparameter; i++)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "command.Parameters.AddWithValue(name[i], value[i]);" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "SqlDataAdapter adapter = new SqlDataAdapter(command);" + "\n";
            code += "\t" + "\t" + "\t" + "DataTable dt = new DataTable();" + "\n";
            code += "\t" + "\t" + "\t" + "adapter.Fill(dt);" + "\n";
            code += "\t" + "\t" + "\t" + "return dt;" + "\n";
            code += "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "public int UpdateData(string sql)" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "SqlCommand command = new SqlCommand(sql, con);" + "\n";
            code += "\t" + "\t" + "\t" + "command.CommandType = CommandType.StoredProcedure;" + "\n";
            code += "\t" + "\t" + "\t" + "return command.ExecuteNonQuery();" + "\n";
            code += "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "public int UpdateData(string sql, string[] name, object[] value, int nparameter)" + "\n";
            code += "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "SqlCommand command = new SqlCommand(sql, con);" + "\n";
            code += "\t" + "\t" + "\t" + "command.CommandType = CommandType.StoredProcedure;" + "\n";
            code += "\t" + "\t" + "\t" + "for (int i = 0; i < nparameter; i++)" + "\n";
            code += "\t" + "\t" + "\t" + "{" + "\n";
            code += "\t" + "\t" + "\t" + "\t" + "command.Parameters.AddWithValue(name[i], value[i]);" + "\n";
            code += "\t" + "\t" + "\t" + "}" + "\n";
            code += "\t" + "\t" + "\t" + "return command.ExecuteNonQuery();" + "\n";
            code += "\t" + "\t" + "}" + "\n";

            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "}" + "\n";
            code += "}" + "\n" + "\n" + "\n";
            return code;
        }

        public string GetDAL()
        {
            string code = Header();
            code += "// Khai bao lop" + "\n";
            code += "using System;" + "\n";
            code += "using System.Collections;" + "\n";
            code += "using System.Data;" + "\n";
            code += "using System.Data.SqlClient;" + "\n";
            code += "using " + _namesp + "_Entities ;//Add reference vao" + "\n";
            code += "namespace " + _namesp + "_DAL" + "\n" + "{" + "\n";
            code += "\t" + "public class " + tblName + "_DAL" + "\n";
            code += "\t" + "{" + "\n";

            code += "\t" + "\t" + "// Ham khoi tao khong tham so" + "\n";
            code += "\t" + "\t" + "#region Constructor" + "\n";
            code += new1_DAL();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n\n";
            code += "\t" + "\t" + "private " + _namesp + "_DAL.KetNoi obj = new " + _namesp + "_DAL.KetNoi();" + "\n\n";
            code += "\t" + "\t" + "#region CRUD functions" + "\n";
            code += "\t" + "\t" + "// Insert" + "\n";
            code += Insert();
            code += "\t" + "\t" + "// Update" + "\n";
            code += Update();
            code += "\t" + "\t" + "// Delete" + "\n";
            code += Del();
            // Get Info
            //code += Convert()
            code += "\t" + "\t" + "// SelectByID" + "\n";
            code += Load();
            code += "\t" + "\t" + "// SelectALL" + "\n";
            code += LoadTable();
            // Check Exist
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// Dong Class" + "\n";
            code += "\t" + "}" + "\n";
            code += "}" + "\n" + "\n" + "\n";
            return code;
        }

        public string GetGUI()
        {
            string code = Header();
            code += "// Khai bao lop" + "\n";
            code += "using System;" + "\n";
            code += "using System.Collections;" + "\n";
            code += "using System.Data;" + "\n";
            code += "using " + _namesp + "_Entities;//Add reference vao" + "\n";
            code += "using " + _namesp + "_BLL;//Add reference vao" + "\n";
            code += "namespace " + _namesp + "\n" + "{" + "\n";
            code += "\t" + "public class " + tblName + "\n";
            code += "\t" + "{" + "\n";
            code += "\t" + "\t" + "private " + _namesp + "_BLL." + tblName + "_BLL obj = new " + _namesp + "_BLL." + tblName + "_BLL();" + "\n\n";
            code += "\t" + "\t" + "// Insert" + "\n";
            code += "\t" + "\t" + "#region Insert" + "\n";
            code += Insert_GUI();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// Update" + "\n";
            code += "\t" + "\t" + "#region Update" + "\n";
            code += Update_GUI();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// Delete" + "\n";
            code += "\t" + "\t" + "#region Delete" + "\n";
            code += Del_GUI();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// SelectByID" + "\n";
            code += "\t" + "\t" + "#region SelectByID" + "\n";
            code += Load_GUI();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// SelectALL" + "\n";
            code += "\t" + "\t" + "#region SelectALL" + "\n";
            code += LoadTable_GUI();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "}" + "\n";
            code += "}" + "\n" + "\n" + "\n";
            return code;
        }

        public string GetBLL()
        {
            string code = Header();
            code += "// Khai bao lop" + "\n";
            code += "using System;" + "\n";
            code += "using System.Collections;" + "\n";
            code += "using System.Data;" + "\n";
            code += "using " + _namesp + "_Entities; //Add reference vao" + "\n";
            code += "using " + _namesp + "_DAL; //Add reference vao" + "\n";
            code += "namespace " + _namesp + "_BLL" + "\n" + "{" + "\n";
            code += "\t" + "public class " + tblName + "_BLL" + "\n";
            code += "\t" + "{" + "\n";

            code += "\t" + "\t" + "// Ham khoi tao khong tham so" + "\n";

            code += "\t" + "\t" + "#region Constructor" + "\n";
            code += new1_BLL();
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "private " + _namesp + "_DAL." + tblName + "_DAL obj = new " + _namesp + "_DAL." + tblName + "_DAL();" + "\n\n";
            code += "\t" + "\t" + "#region CRUD functions" + "\n";
            code += "\t" + "\t" + "// Insert" + "\n";
            code += Insert_BLL();
            code += "\t" + "\t" + "// Update" + "\n";
            code += Update_BLL();
            code += "\t" + "\t" + "// Delete" + "\n";
            code += Del_BLL();
            code += "\t" + "\t" + "// SelectByID" + "\n";
            code += Load_BLL();
            code += "\t" + "\t" + "// SelectALL" + "\n";
            code += LoadTable_BLL();
            // Check Exist
            code += "\t" + "\t" + "#endregion" + "\n" + "\n";
            code += "\t" + "\t" + "// Dong Class" + "\n";
            code += "\t" + "}" + "\n";
            code += "}" + "\n" + "\n" + "\n";
            return code;
        }

        public string GetStore()
        {
            string code = Header1();
            code += "-- InSert" + "\n";
            code += Insert_Store();
            code += "-- Update" + "\n";
            code += Update_store();
            return code;
        }
        #endregion

        #region "Bind"
        public string BindControl()
        {
            string code = "";
            int i;

            // Các biến số
            for (i = 0; i <= Leng - 1; i++)
            {
                code += "txt" + ColName[i] + ".Text = v." + ColName[i] + ";" + "\n";
            }
            return code;
        }
        #endregion

        #region "Gen Control"
        public string GenControl()
        {
            string code = "<table width=\"100%\">" + "\n";
            int i;

            code += "\t" + "<tr>" + "\n";
            code += "\t" + "\t" + "<td></td>" + "\n";
            code += "\t" + "\t" + "<td><asp:Label ID=\"lblMessage\" runat=\"server\" Visible=\"false\" Font-Bold=\"true\" ForeColor=\"red\"></asp:Label></td>" + "\n";
            code += "\t" + "</tr>" + "\n";

            // Các biến số
            for (i = 0; i <= Leng - 1; i++)
            {
                code += "\t" + "<tr>" + "\n";
                code += "\t" + "\t" + "<td>" + ColName[i] + "</td>" + "\n";
                code += "\t" + "\t" + "<td><asp:TextBox ID=\"txt" + ColName[i] + "\" runat=\"server\" style=\"width:300px;\"></asp:TextBox></td>" + "\n";
                code += "\t" + "</tr>" + "\n";
            }
            code += "\t" + "<tr>" + "\n";
            code += "\t" + "\t" + "<td></td>" + "\n";
            code += "\t" + "\t" + "<td><asp:Button ID=\"btnSave\" runat=\"server\" Text=\"Save\"></asp:Button></td>" + "\n";
            code += "\t" + "</tr>" + "\n";
            code += "</table>";
            return code;
        }
        #endregion


        #region "Gencode script oracle"
        private string IdentityColumn(string[] ora)
        {
            for (int i = 0; i <= Leng - 1; i++)
            {
                if (!object.ReferenceEquals(mData.Rows[i]["is_identity"], DBNull.Value))
                {
                    if (ora[i] == "NUMBER" | ora[i] == "INTEGER")
                    {
                        return ColName[i];
                    }
                }
            }
            return "";
        }

        private string[] OracleDataType()
        {
            string[] ora = new string[Leng];
            // ERROR: Not supported in C#: ReDimStatement

            for (int i = 0; i <= Leng - 1; i++)
            {
                ora[i] = "VARCHAR2(250)";
                if (ColType[i] == "long" | ColType[i] == "int" | ColType[i] == "byte" | ColType[i] == "bool")
                    ora[i] = "INTEGER";
                if (ColType[i] == "double" | ColType[i] == "float")
                    ora[i] = "NUMBER";
                if (ColType[i] == "datetime")
                    ora[i] = "DATE";
            }

            return ora;
        }


        public string GenOracleScript()
        {
            string[] ora = OracleDataType();
            string code = "--" + tblName.ToUpper() + "\n";
            code += "CREATE TABLE @@TABLENAME (" + "\n";
            for (int i = 0; i <= Leng - 2; i++)
            {
                string col = ColName[i].ToUpper() + " " + ora[i].ToUpper();

                if ((PrimaryKey[i]))
                {
                    col += " PRIMARY KEY ";
                }

                code += col + "," + "\n";
            }
            code += ColName[Leng - 1].ToUpper() + " " + ora[Leng - 1].ToUpper() + "\n" + ");" + "\n" + "\n";

            string identity = IdentityColumn(ora);

            if ((!identity.Equals("")))
            {
                code += "CREATE SEQUENCE SEQ_@@TABLENAME" + "\n";
                code += "MINVALUE 1" + "\n";
                code += "MAXVALUE 999999999999999999" + "\n";
                code += "START WITH 1" + "\n";
                code += "INCREMENT BY 1" + "\n";
                code += "NOCACHE " + "\n";
                code += ";" + "\n" + "\n";


                code += "CREATE OR REPLACE TRIGGER AUTOVAL_@@TABLENAME" + "\n";
                code += "BEFORE INSERT ON @@TABLENAME" + "\n";
                code += "FOR EACH ROW" + "\n";
                code += "BEGIN" + "\n";
                code += "IF :new.ID IS NULL THEN" + "\n";
                code += "\t" + "Select Case SEQ_SYS_ACTION.nextval" + "\n";
                code += "\t" + "INTO :new.@@ID FROM DUAL;" + "\n";
                code += "END IF;" + "\n";
                code += "END;" + "\n" + "\n";

                code = code.Replace("@@ID", identity.ToUpper());
            }
            code = code.Replace("@@TABLENAME", tblName.ToUpper());

            return code;

        }
        #endregion

        #region "add node"
        public string GenAddNodeXML()
        {
            string code = "XmlElement e = doc.CreateElement(\"elem\");" + "\n";

            for (int i = 0; i <= ColName.Length - 1; i++)
            {
                code += "CommonFunction.AddNode(doc, e, \"" + ColName[i] + "\", dt.Rows[i][\"" + ColName[i] + "\"].ToString());" + "\n";
            }

            return code;
        }
        #endregion
    }
}
