using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PRDB_Visual_Management.BusinessLogicLayer;

namespace PRDB_Visual_Management
{
    static class Resource
    {
        static public string dbShowName;
        static public string dbName;
        static public string curSchemeName;
        static public string curRelationName;
        static public string curQueryName;
        static public string connectionString;
        static public List<string> schemeNames;
        static public List<string> relationNames;
        static public List<string> queryNames;
        static public ProbScheme currentScheme;
        static public ProbRelation currentRelation;
        static public ProbQuery currentQuery;
        static public string currentDirectory;
    }
}
