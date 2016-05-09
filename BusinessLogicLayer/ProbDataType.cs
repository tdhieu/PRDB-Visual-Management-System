using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbDataType
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public string typeName { get; set; } // typeName != dataType if dataType == "User-Defined"

        public string dataType { get; set; } // dataType = {Int16, Int32, Int64, Byte, String, Single, Double, Boolean, Decimal, DateTime, Binary, Currency, User-Defined}

        public string domainString { get; set; }

        public List<string> domainList { get; set; }



        // ---------------------------- Vùng định nghĩa các phương thức ----------------------------
        public ProbDataType()
        {
            domainList = new List<string>();
            typeName = string.Empty;
            dataType = string.Empty;
            domainString = string.Empty;
        }

        public void GetDomain(string str)
        {
            try
            {
                this.domainString = str;
                if (this.dataType.Equals("UserDefined"))
                {
                    str = str.Replace("{", "");
                    str = str.Replace("}", "");
                    char[] seperator = { ',' };
                    string[] temp = str.Split(seperator);
                    domainList = new List<string>();
                    foreach (string value in temp)
                        domainList.Add(value.Trim());
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }        

        private bool IsBinaryType(object V)
        {
            try
            {
                foreach (char i in V.ToString())
                    if (i != '0' && i != '1')
                        return false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return true;
        }

        private bool IsCurrencyType(object V)
        {
            try
            {
                double MINCURRENCY = 1.0842021724855044340074528008699e-19;
                double MAXCURRENCY = 9223372036854775807.0;
                double temp = Convert.ToDouble(V);
                if (temp - MINCURRENCY >= 0)
                    if (temp - MAXCURRENCY <= 0)
                        return true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return false;
        }

        public bool CheckDataType(string V)
        {
            try
            {
                List<object> values = new List<object>();

                V = V.Replace(" ", "");
                string[] seperator = { "||" };
                string[] temp = V.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                int j1, j2;
                for (int i = 0; i < temp.Length; i++)
                {
                    j1 = temp[i].IndexOf('{');
                    j2 = temp[i].IndexOf('}');
                    values.Add(temp[i].Substring(j1 + 1, j2 - j1 - 1));
                }

                foreach (object value in values)
                {
                    switch (this.dataType)
                    {
                        case "Int16": Convert.ToInt16(value); break;
                        case "Int32": Convert.ToInt32(value); break;
                        case "Int64": Convert.ToInt64(value); break;
                        case "Byte": Convert.ToByte(value); break;
                        case "String": Convert.ToString(value); break;
                        case "DateTime": Convert.ToDateTime(value); break;
                        case "Decimal": Convert.ToDecimal(value); break;
                        case "Single": Convert.ToSingle(value); break;
                        case "Double": Convert.ToDouble(value); break;
                        case "Boolean": Convert.ToBoolean(value); break;
                        case "Binary": return (IsBinaryType(value));
                        case "Currency": return (IsCurrencyType(value));
                        case "UserDefined": return (this.domainList.Contains(value.ToString()));
                    }
                }
                return true;
            }
            catch { return false; }
        }

        // Lấy dataType từ typeName
        public void GetDataType(string typeName)
        {
            try
            {
                this.typeName = typeName;

                // Mặc định gán dataType = UserDefined
                this.dataType = "UserDefined";

                // Nếu typeName là một trong các kiểu cơ bản, dataType sẽ được gán lại
                switch (this.typeName)
                {
                    case "Int16": this.dataType = "Int16"; break;
                    case "Int64": this.dataType = "Int64"; break;
                    case "Int32": this.dataType = "Int32"; break;
                    case "Byte": this.dataType = "Byte"; break;
                    case "Decimal": this.dataType = "Decimal"; break;
                    case "Currency": this.dataType = "Currency"; break;
                    case "String": this.dataType = "String"; break;
                    case "DateTime": this.dataType = "DateTime"; break;
                    case "Binary": this.dataType = "Binary"; break;
                    case "Single": this.dataType = "Single"; break;
                    case "Double": this.dataType = "Double"; break;
                    case "Boolean": this.dataType = "Boolean"; break;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public void Assign(ProbDataType type)
        {
            try
            {
                if (type == null) return;
                this.typeName = type.typeName;
                this.dataType = type.dataType;
                this.domainString = type.domainString;
                foreach (string domain in type.domainList) this.domainList.Add(domain);
            }
            catch (Exception Ex)
            {
            }
        }

        public bool IsEqualTo(ProbDataType checkDataType)
        {
            try
            {
                // Không cần kiểm tra List<string> Domain vì được suy ra từ domainString
                if (!this.typeName.Equals(checkDataType.typeName)) return false;
                if (!this.dataType.Equals(checkDataType.dataType)) return false;
                if (!this.domainString.Equals(checkDataType.domainString)) return false;
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public bool EquivalentDataType(string value)
        {
            try
            {
                switch (this.dataType)
                {
                    case "Int16": Convert.ToInt16(value); break;
                    case "Int32": Convert.ToInt32(value); break;
                    case "Int64": Convert.ToInt64(value); break;
                    case "Byte": Convert.ToByte(value); break;
                    case "String": Convert.ToString(value); break;
                    case "DateTime": Convert.ToDateTime(value); break;
                    case "Decimal": Convert.ToDecimal(value); break;
                    case "Single": Convert.ToSingle(value); break;
                    case "Double": Convert.ToDouble(value); break;
                    case "Boolean": Convert.ToBoolean(value); break;
                    case "Binary": return (IsBinaryType(value));
                    case "Currency": return (IsCurrencyType(value));
                    case "UserDefined": return (this.domainList.Contains(value.ToString()));
                }
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

    }
}
