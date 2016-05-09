using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbTriple   // Giá trị bộ ba xác suất
    {
        // ---------------------------- Phần khai báo thuộc tính ----------------------------        
        
        // Tập các giá trị
        public List<object> values { get; set; }

        // Tập xác suất cận dưới
        public List<double> minprob { get; set; }

        // Tập xác suất cận trên
        public List<double> maxprob { get; set; }
        
        
        // ---------------------------- Phần định nghĩa các phương thức ----------------------------       

        public ProbTriple()
        {
            this.values = new List<object>();
            this.minprob = new List<double>();
            this.maxprob = new List<double>();
        }

        // Tạo bộ ba xác suất từ chuỗi text
        public ProbTriple(string V, ProbDataType dataType)
        {
            try
            {
                this.values = new List<object>();
                this.minprob = new List<double>();
                this.maxprob = new List<double>();

                string[] seperator = { "||" };
                string[] value = V.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                int j1, j2, j3, j4, j5;
                for (int i = 0; i < value.Length; i++)
                {
                    j1 = value[i].IndexOf('{');
                    j2 = value[i].IndexOf('}');
                    j3 = value[i].IndexOf('[');
                    j4 = value[i].IndexOf(',');
                    j5 = value[i].IndexOf(']');
                    values.Add(value[i].Substring(j1 + 1, j2 - j1 - 1));
                    minprob.Add(Convert.ToDouble(value[i].Substring(j3 + 1, j4 - j3 - 1)));
                    maxprob.Add(Convert.ToDouble(value[i].Substring(j4 + 1, j5 - j4 - 1)));
                }
            }
            catch
            {
                MessageBox.Show("Syntax Error! Cannot convert to Probabilistic Triple!");
            }
        }

        // Xuất bộ ba xác suất ra chuỗi giá trị
        public string GetStrValue()
        {
            string strValue = "";
            int n = values.Count;

            for (int i = 0; i < values.Count; i++)
            {
                strValue += "{ ";
                strValue += values[i].ToString();
                strValue += " }";
                strValue += "[ ";
                strValue += minprob[i];
                strValue += ", ";
                strValue += maxprob[i];
                strValue += " ]";
                strValue += "  ||  ";
            }
            strValue = strValue.Remove(strValue.Length - 6); // loại bỏ kí tự '||' thừa

            return strValue;
        }

        public void Assign(ProbTriple triple)
        {
            try
            {
                if (triple == null) return;
                for (int i = 0; i < triple.values.Count; i++)
                {
                    this.values.Add(triple.values[i]);
                    this.minprob.Add(triple.minprob[i]);
                    this.maxprob.Add(triple.maxprob[i]);
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public bool IsEqualTo(ProbTriple checkTriple, string dataType)
        {
            try
            {
                if (this.values.Count != checkTriple.values.Count) return false;
                bool checkEqual;
                
                // Kiểm tra xem các giá trị trong tập giá trị của 2 triple có bằng nhau hay không
                for (int i = 0; i < this.values.Count; i++)
                {
                    checkEqual = false;
                    for (int j = 0; j < checkTriple.values.Count; j++)
                        if (QueryExecution.EQUAL(this.values[i], checkTriple.values[j], dataType))
                            if (QueryExecution.EQUAL(this.minprob[i], checkTriple.minprob[j], "double"))
                                if (QueryExecution.EQUAL(this.maxprob[i], checkTriple.maxprob[j], "double"))
                                {
                                    checkEqual = true;
                                    break;
                                }
                    if (checkEqual == false) return false;
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
