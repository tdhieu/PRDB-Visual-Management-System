using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PRDB_Visual_Management.DataAccessLayer;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class ProbDatabase
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        public DataSet dataset { get; set; }

        // Tên cơ sở dữ liệu
        public string dbName { get; set; }

        public string connectionString { get; set; }

        // Đường dẫn đến CSDL
        public string dbPath { get; set; }

        // Tập các lược đồ cơ sở dữ liệu
        public List<ProbScheme> schemes { get; set; }

        // Tập các quan hệ cơ sở dữ liệu
        public List<ProbRelation> relations { get; set; }

        // Tập các truy vấn cơ sở dữ liệu
        public List<ProbQuery> queries { get; set; }




        // ---------------------------- Vùng định nghĩa các phương thức ----------------------------

        // Phương thức khởi tạo
        public ProbDatabase(string path)
        {
            // Lấy đường dẫn cho CSDL 
            this.dbPath = path;      
            this.dbName = "";

            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\') break;
                else this.dbName = path[i] + dbName;
            }
            // Đặt chuỗi kết nối
            this.connectionString = "Data Source=" + dbPath + ";Version=3;";
            this.dbName = CutExtension(dbName);
            this.relations = new List<ProbRelation>();
            this.queries = new List<ProbQuery>();
            this.schemes = new List<ProbScheme>();
        }

        public void Rename(string dbName)
        {
            this.dbName = dbName+".pdb";
            int pos = this.dbPath.LastIndexOf('\\');
            this.dbPath = this.dbPath.Remove(pos + 1);
            this.dbPath += dbName;
            this.connectionString = "Data Source=" + dbPath + ";Version=3;";
        }

        public List<string> ListOfSchemeName()
        {
            List<string> List = new List<string>();
            foreach (ProbScheme schema in this.schemes)
                List.Add(schema.schemename);
            return List;
        }

        public List<string> ListOfRelationName()
        {
            List<string> List = new List<string>();
            foreach (ProbRelation relation in this.relations)
                List.Add(relation.relationname);
            return List;
        }

        public List<string> ListOfQueryName()
        {
            List<string> List = new List<string>();
            foreach (ProbQuery query in this.queries)
                List.Add(query.queryName);
            return List;
        }

        public string CutExtension(string name)
        {
            for (int i=name.Length-1; i>=0; i--)
                if (name[i] == '.')
                {
                    name = name.Remove(i);
                    break;
                }
            return name;
        }

        public ProbRelation GetRelation(string relname)
        {
            foreach (ProbRelation relation in this.relations)
                if (relation.relationname.CompareTo(relname) == 0)
                    return relation;
            return null;
        }
        
        public ProbQuery GetQuery(string queryname)
        {
            foreach (ProbQuery query in this.queries)
                if (query.queryName.CompareTo(queryname) == 0)
                    return query;
            return null;
        }

        public ProbScheme GetScheme(string schemename)
        {
            foreach (ProbScheme scheme in this.schemes)
                if (scheme.schemename.Equals(schemename))
                    return scheme;
            return null;
        }

        public bool isProbTripleValue(string V)
        {
            V = V.Replace(" ", "");
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
                if (j1 < 0 || j2 < 0 || j3 < 0 || j4 < 0 || j5 < 0) return false;
                if (j1 >= j2-1) return false;
                if (j2 >= j3) return false;
                if (j3 >= j4-1) return false;
                if (j4 >= j5-1) return false;
            }
            return true;
        }
    }
}
