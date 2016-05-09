using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SQLite;
using System.Windows.Forms;
using PRDB_Visual_Management.DataAccessLayer;
using PRDB_Visual_Management.BusinessLogicLayer;
using PRDB_Visual_Management.PresentationLayer;

namespace PRDB_Visual_Management.BusinessLogicLayer
{
    public class clsProcess
    {
        // ---------------------------- Vùng khai báo các thuộc tính ----------------------------

        ProbDatabase DB;
        private Thread workerThread;                        // Thread xử lý queries
        private bool connected = false;
        private SQLiteConnection connection = new SQLiteConnection();
        private MethodInvoker task = null;					// next task for the worker thread
        public enum RunState { Idle, Running, Cancelling };
        private RunState runState = RunState.Idle;

        SQLiteDataAdapter dataAdapter;
        DataSet dts;
        DataTable dtb;
        

        // ---------------------------- Vùng định nghĩa các phương thức ----------------------------

        public clsProcess()
        {
            dataAdapter = new SQLiteDataAdapter();
        }

        public string GetRootPath(string path)
        {
            string root = "";
            for (int i = 0; i < path.Length; i++)
                if (path[i] == '\\')
                {
                    root = path.Substring(0, i + 1);
                    break;
                }
            return root;
        }

        public ProbDatabase NewDatabase(string strPath)
        {
            DB = new ProbDatabase(strPath);
            this.workerThread = new Thread(new ThreadStart(StartWorker));
            workerThread.Name = "DbClient Worker Thread";
            workerThread.Start();
            return DB;
        }

        public bool Connect()
        {
            if (connected) return true;
            RunOnWorker(new MethodInvoker(DoConnect), true);
            return connected;
        }

        public void DoConnect()
        {
            if (connected) return;
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.ConnectionString = Resource.connectionString;
                    connection.Open();
                    connected = true;
                }
            }
            catch (SQLiteException sqliteEx)
            {
                MessageBox.Show(sqliteEx.Message);
            }
        }

        protected void StartWorker()
        {
            do
            {
                // Chờ luồng chính thức giấc  
                //Thread.CurrentThread.Suspend();
                try { Thread.Sleep(Timeout.Infinite); }
                catch (Exception) { }					// the wakeup call, ie Interrupt() will throw an exception
                // Nếu không làm gì, form thread sẽ được đóng lại
                if (task == null) break;
                // Ngược lại, thực thi công việc được giao
                task();
                task = null;
            } while (true);
        }

        public void RunOnWorker(MethodInvoker method)
        {
            RunOnWorker(method, false);
        }

        public void RunOnWorker(MethodInvoker method, bool synchronous)
        {
            if (task != null) 								// already doing something?
            {
                Thread.Sleep(100);					        // give it 100ms to finish...
                if (task != null) return;				    // still not finished - cannot run new task
            }
            WaitForWorker();
            task = method;
            workerThread.Interrupt();
            if (synchronous) WaitForWorker();
        }

        public void WaitForWorker()
        {
            while (workerThread.ThreadState != ThreadState.WaitSleepJoin || task != null)
            {
                Thread.Sleep(20);
            }
        }

        public void StopWorker()
        {
            WaitForWorker();
            // kết thúc luồng
            workerThread.Interrupt();			            // interupt thread without task
            workerThread.Join();			                // wait for ending
        }

        public virtual void Disconnect()
        {
            if (runState == RunState.Running) Cancel();
            if (connected)
                RunOnWorker(new MethodInvoker(connection.Close), true);
        }

        public virtual void Cancel()
        {
            // Dừng một truy vấn đang chạy đồng bộ (chờ cho đến khi truy vấn dừng)
            // Phương thức này được goi khi ta đóng một câu truy vấn đang thực thi.
            if (runState == RunState.Running)
            {
                DoCancel();
                WaitForWorker();
                runState = RunState.Idle;
            }
        }

        protected virtual void DoCancel()
        {
            if (runState == RunState.Running)
            {
                runState = RunState.Cancelling;
                Thread cancelThread = new Thread(new ThreadStart(dataAdapter.SelectCommand.Cancel));
                cancelThread.Name = "DbClient Cancel Thread";
                cancelThread.Start();
                cancelThread.Join();
            }
        }

        public virtual void Dispose()
        {
            if (connected) Disconnect();
            StopWorker();
        }


        // Create new Database and create new system tables 
        public bool CreateNewDatabase(ProbDatabase DB)
        {
            try
            {
                SQLiteConnection.CreateFile(DB.dbPath);
                Connection clsConnection = new Connection();
                string SQL = "";

                // Record set of schemes to the database system
                SQL = "";
                SQL += "CREATE TABLE SystemScheme ( ";
                SQL += "ID INT, ";
                SQL += "SchemeName NVARCHAR(200) ";
                SQL += " );";

                if (!clsConnection.CreateTable(SQL))
                    throw new Exception(clsConnection.ErrorMessage);

                // Record set of relations to the database system
                SQL = "";
                SQL += "CREATE TABLE SystemRelation ( ";
                SQL += "ID INT, ";
                SQL += "RelationName NVARCHAR(200), ";
                SQL += "SchemeID INT ";
                SQL += " );";

                if (!clsConnection.CreateTable(SQL))
                    throw new Exception(clsConnection.ErrorMessage);

                // Record set of attributes to the database system  
                SQL = "";
                SQL += "CREATE TABLE SystemAttribute ( ";
                SQL += "ID INT, ";
                SQL += "PrimaryKey NVARCHAR(10), ";
                SQL += "AttributeName NVARCHAR(200), ";
                SQL += "DataType NVARCHAR(200), ";
                SQL += "Domain TEXT, ";
                SQL += "Description TEXT, ";
                SQL += "SchemeID INT ";
                SQL += " ); ";

                if (!clsConnection.CreateTable(SQL))
                    throw new Exception(clsConnection.ErrorMessage);

                // Record set of queries to the database system
                SQL = "";
                SQL += "CREATE TABLE SystemQuery ( ";
                SQL += "ID INT, ";
                SQL += "QueryName NVARCHAR(200), ";
                SQL += "QueryString TEXT ";
                SQL += " );";

                if (!clsConnection.CreateTable(SQL))
                    throw new Exception(clsConnection.ErrorMessage);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
            return true;
        }

        public bool LoadDatabase(ProbDatabase DB)
        {
            try
            {
                Connection clsConnection = new Connection();

                dts = new DataSet();
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemScheme", "system_scheme"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemRelation", "system_relation"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemAttribute", "system_attribute"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemQuery", "system_query"));

                string schemename, relationname;
                int schemeID, ntriples;
                ProbScheme NewScheme;
                ProbRelation NewRelation;
                ProbQuery NewQuery;
                ProbAttribute NewAttr;
                ProbTuple NewTuple;
                ProbTriple NewTriple;

                ////////////////////////////////////////////////////////////// Load schemes ////////////////////////////////////////////////

                foreach (DataRow row in dts.Tables["system_scheme"].Rows)
                {
                    schemename = row[1].ToString();
                    NewScheme = new ProbScheme(schemename);
                    dtb = clsConnection.GetDataTable("SELECT * FROM SystemAttribute Where SchemeID=" + Convert.ToInt16(row[0]));
                    if (dtb != null)
                    foreach (DataRow attrRow in dtb.Rows)
                    {
                        NewAttr = new ProbAttribute();
                        NewAttr.primaryKey = Convert.ToBoolean(attrRow[1]);
                        NewAttr.attributeName = Convert.ToString(attrRow[2]);
                        NewAttr.type.GetDataType(Convert.ToString(attrRow[3]));
                        NewAttr.type.GetDomain(Convert.ToString(attrRow[4]));
                        NewAttr.description = Convert.ToString(attrRow[5]);
                        NewScheme.attributes.Add(NewAttr);
                    }
                    DB.schemes.Add(NewScheme);
                }

                ////////////////////////////////////////////////////////////// Load relation ////////////////////////////////////////////////                    

                foreach (DataRow row in dts.Tables["system_relation"].Rows)
                {
                    relationname = row[1].ToString();
                    schemeID = Convert.ToInt16(row[2]);
                    // Find related scheme name
                    schemename = clsConnection.GetValueField("Select SchemeName From SystemScheme Where ID=" + schemeID).ToString();
                    dtb = clsConnection.GetDataTable("Select * From " + relationname);
                    NewRelation = new ProbRelation(relationname);
                    NewRelation.scheme = DB.GetScheme(schemename);
                    ntriples = NewRelation.scheme.attributes.Count;
                    if (dtb != null)
                    foreach (DataRow tuplerow in dtb.Rows)
                    {
                        NewTuple = new ProbTuple();
                        for (int i = 0; i < ntriples; i++)
                        {
                            NewTriple = new ProbTriple(tuplerow[i].ToString(), NewRelation.scheme.attributes[i].type);
                            NewTuple.triples.Add(NewRelation.scheme.attributes[i], NewTriple);
                        }
                        NewRelation.tuples.Add(NewTuple);
                    }
                    
                    DB.relations.Add(NewRelation);
                }

                ////////////////////////////////////////////////////////////// Load query ////////////////////////////////////////////////                    

                foreach (DataRow queryRow in dts.Tables["system_query"].Rows)
                {
                    NewQuery = new ProbQuery();
                    NewQuery.queryName = queryRow[1].ToString();
                    NewQuery.GetQueryString(queryRow[2].ToString());
                    DB.queries.Add(NewQuery);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
            return true;
        }

        public void DropDatabaseData()
        {
            try
            {
                Connection clsConnection = new Connection();
                dts = new DataSet();
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemScheme", "system_scheme"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemRelation", "system_relation"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemAttribute", "system_attribute"));
                dts.Tables.Add(clsConnection.GetDataTable("SELECT * FROM SystemQuery", "system_query"));

                string relationname;

                foreach (DataRow row in dts.Tables["system_relation"].Rows)
                {
                    relationname = row[1].ToString();
                        if (!clsConnection.DropTable(relationname))
                            throw new Exception(clsConnection.ErrorMessage);
                }

                if (!clsConnection.Update("DELETE FROM SystemScheme"))
                    throw new Exception(clsConnection.ErrorMessage);

                if (!clsConnection.Update("DELETE FROM SystemRelation"))
                    throw new Exception(clsConnection.ErrorMessage);

                if (!clsConnection.Update("DELETE FROM SystemAttribute"))
                    throw new Exception(clsConnection.ErrorMessage);

                if (!clsConnection.Update("DELETE FROM SystemQuery"))
                    throw new Exception(clsConnection.ErrorMessage);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        public bool SaveDatabase(ProbDatabase DB)     // Record schemes, relations and queries to the database
        {
            try
            {
            string SQL = "";
            Connection clsConnection = new Connection();

//////////////////////////////////////////////// Save Schemes ///////////////////////////////////////////////////////////
                int schemeID = 0;
                int attributeID = 0;
                foreach (ProbScheme scheme in DB.schemes)
                {

//////////////////////////////////////////////// Update <Scheme> to System Scheme Table //////////////////////////////////
                    schemeID++;
                    string SchemeName = scheme.schemename;
                    SQL = "";
                    SQL += "INSERT INTO SystemScheme VALUES (";
                    SQL += schemeID + ",";
                    SQL += "'" + SchemeName + "'";
                    SQL += " );";

                    if (!clsConnection.Update(SQL))
                        throw new Exception(clsConnection.ErrorMessage);

//////////////////////////////////// Save attributes of the scheme to the System Attribute Table /////////////////////////
                    if (scheme.attributes.Count > 0)
                    {
                        foreach (ProbAttribute attr in scheme.attributes)
                        {
                            attributeID++;
                            SQL = "";
                            SQL += "INSERT INTO SystemAttribute VALUES ( ";
                            SQL += attributeID + ",";
                            SQL += "'" + attr.primaryKey + "'" + ",";
                            SQL += "'" + attr.attributeName + "'" + ",";
                            SQL += "'" + attr.type.typeName + "'" + ",";
                            SQL += "'" + attr.type.domainString + "'" + ",";
                            SQL += "'" + attr.description + "'" + ",";
                            SQL += schemeID;
                            SQL += " );";

                            if (!clsConnection.Update(SQL))
                                throw new Exception(clsConnection.ErrorMessage);
                        }
                    }
                }

////////////////////////////////////////////// Save Relations /////////////////////////////////////////////////////////

                int relationID = 0;
                foreach (ProbRelation relation in DB.relations)
                {
                    relationID++;
                    string RelationName = relation.relationname;
                    schemeID = clsConnection.GetSchemeID(relation.scheme.schemename);
                    SQL = "";
                    SQL += "INSERT INTO SystemRelation VALUES ( ";
                    SQL += relationID + ",";
                    SQL += "'" + RelationName + "'" + ",";
                    SQL += schemeID;
                    SQL += " );";

                    if (!clsConnection.Update(SQL))
                        throw new Exception(clsConnection.ErrorMessage);

///////////////////////////////////////////// Create Table <Relation> ////////////////////////////////////////////////

                    if (relation.scheme.attributes.Count > 0)
                    {
                        SQL = "";
                        SQL += "CREATE TABLE " + RelationName + " ( ";
                        foreach (ProbAttribute attribute in relation.scheme.attributes)
                        {
                            SQL += attribute.attributeName + " " + "TEXT" + ", ";
                        }
                        SQL = SQL.Remove(SQL.LastIndexOf(','), 1);
                        SQL += " ); ";

                        if (!clsConnection.CreateTable(SQL))
                            throw new Exception(clsConnection.ErrorMessage);
                    }

//////////////////////////////////////////// Insert tuples into Table <Relation> ////////////////////////////////////

                    if (relation.tuples.Count > 0)
                    {
                        foreach (ProbTuple tuple in relation.tuples)
                        {
                            SQL = "";
                            SQL += "INSERT INTO " + RelationName + " VALUES (";
                            foreach (KeyValuePair<ProbAttribute, ProbTriple> triple in tuple.triples)
                            {
                                SQL += "'" + triple.Value.GetStrValue() + "'" + ",";
                            }
                            SQL = SQL.Remove(SQL.LastIndexOf(','), 1);
                            SQL += " );  ";

                            if (!clsConnection.Update(SQL))
                                throw new Exception(clsConnection.ErrorMessage);
                        }
                    }
                }                                

////////////////////////////////////////////////// Save Queries /////////////////////////////////////////////////////////

                int queryID = 0;
                foreach (ProbQuery query in DB.queries)
                {
                    queryID++;
                    SQL = "";
                    SQL += "INSERT INTO SystemQuery VALUES (";
                    SQL += queryID + ",";
                    SQL += "'" + query.queryName + "'" + ",";
                    SQL += "'" + query.queryString + "'";
                    SQL += " );";

                    if (!clsConnection.Update(SQL))
                        throw new Exception(clsConnection.ErrorMessage);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
            return true;
        }

        public bool SaveDatabaseNewName(ProbDatabase DB)   // Save database as a different database
        {
            try
            {
                if (!CreateNewDatabase(DB)) return false;
                if (!SaveDatabase(DB)) return false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return false;
            }
            return true;
        }
    }
}
