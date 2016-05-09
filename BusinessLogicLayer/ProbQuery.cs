using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbQuery
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public string queryName { get; set; }

        public string queryString { get; set; }




        // ---------------------------- Vùng định nghĩa các phương thức ----------------------------

        public ProbQuery()
        {
            queryName = queryString = "";
        }

        public ProbQuery(string queryName)
        {
            this.queryName = queryName;
            queryString = "";
        }

        public ProbQuery(string queryName, string queryString)
        {
            this.queryName = queryName;
            this.queryString = queryString;
        }

        public void GetQueryString(string query)
        {
            this.queryString = query;
        }

        public string CutSpareSpace(string S)
        {
            string result = "";
            for (int i = 0; i < S.Length; i++)
                if (S[i] == ' ')
                {
                    if (S[i - 1] != ' ') result += S[i];
                }
                else result += S[i];
            return result;
        }
    }
}
