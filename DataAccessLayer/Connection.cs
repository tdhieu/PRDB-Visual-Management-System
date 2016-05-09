using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace PRDB_Visual_Management.DataAccessLayer
{
    public class Connection
    {
        #region Properties
        public SQLiteConnection connection;
        public SQLiteCommand command;
        public SQLiteDataAdapter dapt;

        private string commandText;
        public string CommandText { get { return commandText; } set { commandText = value; } }

        private CommandType commandType;
        public CommandType CommandType { get { return commandType; } set { commandType = value; } }

        private string errorMessage;
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }

        private string[] parameterCollection;
        public string[] ParameterCollection { get { return parameterCollection; } set { parameterCollection = value; } }

        private Object[] valueCollection;
        public Object[] ValueCollection { get { return valueCollection; } set { valueCollection = value; } }

        #endregion

        #region Connection

        public Connection()
        {
            connection = new SQLiteConnection();
            command = connection.CreateCommand();
            dapt = new SQLiteDataAdapter();
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.ConnectionString = Resource.connectionString;
                    connection.Open();
                }
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
            }
        }

        #endregion

        #region Query
        public DataSet GetDataSet(string QueryString, string TblName)
        {
            DataSet dts = new DataSet();
            OpenConnection();
            try
            {
                command = new SQLiteCommand();
                command.CommandText = QueryString;
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (parameterCollection != null)
                    AddParametter(command);

                dapt = new SQLiteDataAdapter(command);
                dapt.Fill(dts, TblName);
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return null;
            }
            CloseConnection();
            return dts;
        }

        public DataSet GetDataSet(string QueryString)
        {
            DataSet dts = new DataSet();
            OpenConnection();
            try
            {
                command = new SQLiteCommand();
                command.CommandText = QueryString;
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (parameterCollection != null)
                    AddParametter(command);

                dapt = new SQLiteDataAdapter(command);
                dapt.Fill(dts);
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return null;
            }
            CloseConnection();
            return dts;
        }

        public DataTable GetDataTable(string QueryString, string tablename)
        {
            DataTable dtb = new DataTable(tablename);
            OpenConnection();
            try
            {
                command = new SQLiteCommand();
                command.CommandText = QueryString;
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (parameterCollection != null)
                    AddParametter(command);

                dapt = new SQLiteDataAdapter(command);
                dapt.Fill(dtb);
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return null;
            }
            CloseConnection();
            return dtb;
        }

        public DataTable GetDataTable(string QueryString)
        {
            DataTable dtb = new DataTable();
            OpenConnection();
            try
            {
                command = new SQLiteCommand();
                command.CommandText = QueryString;
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                if (parameterCollection != null)
                    AddParametter(command);

                dapt = new SQLiteDataAdapter(command);
                dapt.Fill(dtb);
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return null;
            }
            CloseConnection();
            return dtb;
        }

        private void AddParametter(SQLiteCommand cmd)
        {
            try
            {
                for (int i = 0; i < parameterCollection.Length; i++)
                    cmd.Parameters.AddWithValue(parameterCollection[i], valueCollection[i]);
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
            }
        }

        public bool Existed(string TableName) // Check if the table had been created
        {
            OpenConnection();
            try
            {
                SQLiteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE name='" + TableName + "'";
                SQLiteDataReader dtreader = cmd.ExecuteReader();                
                if (dtreader.HasRows) return true;
                else return false;
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                MessageBox.Show(sqliteEx.Message);
            }
            CloseConnection();
            return false;
        }

        #endregion

        #region Update

        public bool CreateTable(string sql)
        {
            try
            {
                OpenConnection();
                commandText = sql;
                commandType = CommandType.Text;
                int result = ExecuteNonQuery();
                CloseConnection();
                if (result < 0) return false;
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return false;
            }
            return true;
        }

        public bool DropTable(string TableName)
        {
            try
            {
                OpenConnection();
                commandText = "DROP TABLE IF EXISTS " + TableName;
                commandType = CommandType.Text;
                int result = ExecuteNonQuery();
                CloseConnection();
                if (result < 0) return false;
            }
            catch (SQLiteException sqliteEx)
            {
                ErrorMessage = sqliteEx.Message;
                return false;
            }
            return true;
        }

        public bool Update(string sql)
        {
            try
            {
                OpenConnection();
                commandText = sql;
                commandType = CommandType.Text;
                int result = ExecuteNonQuery();
                CloseConnection();
                if (result < 0) return false;
            }
            catch (SQLiteException sqliteEx)
            {
                ErrorMessage = sqliteEx.Message;
                return false;
            }
            return true;
        }

        private int ExecuteNonQuery()
        {
            int rows = 0;
            try
            {
                command = new SQLiteCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.CommandType = commandType;

                if (parameterCollection != null)
                    AddParametter(command);

                rows = command.ExecuteNonQuery();
            }
            catch (SQLiteException sqliteEx)
            {                
                errorMessage = sqliteEx.Message;
                return -1;
            }
            finally
            {
                command.Dispose();
            }
            return rows; // trả về số mẫu tin thực thi
        }

        private object ExecuteScalar()
        {
            object objValue = null;
            try
            {
                command = new SQLiteCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.CommandText = commandText;

                if (parameterCollection != null)
                    AddParametter(command);

                objValue = command.ExecuteScalar();
            }
            catch (SQLiteException sqliteEx)
            {
                errorMessage = sqliteEx.Message;
                return null;
            }
            finally
            {
                command.Dispose();
            }
            return objValue;
        }

        public int GetSchemeID(string SchName)
        {
            OpenConnection();
            int id;
            try
            {
                commandText = "SELECT ID FROM SystemScheme WHERE SchemeName=" + "'" + SchName + "'";
                parameterCollection = null;
                id = Convert.ToInt16(ExecuteScalar());
            }
            catch (SQLiteException sqliteEx)
            {
                ErrorMessage = sqliteEx.Message;
                return -1;
            }
            return id;
        }

        public object GetValueField(string Query)
        {
            OpenConnection();
            try
            {
                commandText = Query;
                parameterCollection = null;
                return ExecuteScalar();
            }
            catch (SQLiteException sqliteEx)
            {
                ErrorMessage = sqliteEx.Message;
                MessageBox.Show(sqliteEx.Message);
            }
            return null;
        }

        public int GetNumberOfTuple(string TableName)
        {
            OpenConnection();
            int number;
            try
            {
                commandText = "SELECT Count(*) FROM " + TableName;
                parameterCollection = null;
                number = Convert.ToInt16(ExecuteScalar());
            }
            catch (SQLiteException sqliteEx)
            {
                ErrorMessage = sqliteEx.Message;
                return -1;
            }
            return number;
        }

        #endregion
    }
}
