using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ClassGenerator
{
    public class CodeFile
    {
        public String ClassCode { get; set; }
        public String ClassCodePath { get; set; }
        private List<DBColumn> DBColumns { get; set; }

        public CodeFile WriteCode(string connectionString, string className, string outputDirectory, string classNamespace)
        {
            //1. Get the fields
            DBColumns = DBRepository.ListColumns(connectionString, className);
                        
            ClassCode = ReadClassTemplate();
            ClassCode = WriteUsings(ClassCode);
            ClassCode = ClassCode.Replace("@@ClassName@@", className);
            ClassCode = WriteClassNamepace(ClassCode, classNamespace);

            //2. Write the fields
            ClassCode = WriteClassFields(ClassCode);

            //3. Save the contents into a new file in the output directory
            File.WriteAllText(outputDirectory + @"\" + className + ".cs", ClassCode);
            ClassCodePath = outputDirectory + @"\" + className + ".cs";
                      
            return this;
        }

        private String ReadClassTemplate()
        {
            ClassCode = "@@Usings@@\n";
            ClassCode += "namespace @@Namespace@@\n";
            ClassCode += "{\n\t";
            ClassCode += "public class @@ClassName@@\n\t";
            ClassCode += "{\n\t\t";
            ClassCode += "@@Fields@@\n\t";
            ClassCode += "}\n";
            ClassCode += "}";

            return ClassCode;
        }

        private String WriteUsings(String code)
        {
            String newCode = "";

            List<String> list = AppHelper.LoadUsingStatements();

            foreach (String s in list)
            {
                newCode = String.Concat(newCode, s, "\r\n");
            }

            code = code.Replace("@@Usings@@", newCode);

            return code;
        }

        private String WriteClassNamepace(String code, String classNamespace)
        {
            code = code.Replace("@@Namespace@@", classNamespace);
            return code;
        }

        private String WriteClassFields(String code)
        {
            String newCode = "";
            foreach (DBColumn field in DBColumns)
            {
                switch (field.DataType.ToString())
                {
                    case "System.String":                        
                        newCode = String.Concat(newCode, "public " + field.DataType + " ", field.Name + " { get; set; }", "\r\n\t\t");
                        break;

                    case "System.Byte":
                        newCode = String.Concat(newCode, "public " + field.DataType + "[] ", field.Name + " { get; set; }", "\r\n\t\t");
                        break;

                    default:
                        if(field.IsNullable)
                            newCode = String.Concat(newCode, "public " + field.DataType + "? ", field.Name + " { get; set; }", "\r\n\t\t");
                        else
                            newCode = String.Concat(newCode, "public " + field.DataType + " ", field.Name + " { get; set; }", "\r\n\t\t");
                        break;
                } 
                
            }
            code = code.Replace("@@Fields@@", newCode);
            return code;
        }
    }
}
