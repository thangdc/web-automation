using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using ThangDC.Core.Securities;

namespace ThangDC.Core.Entities
{
    public class Connection
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Server;
        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }

        private string _Database;
        public string Database
        {
            get { return _Database; }
            set { _Database = value; }
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        private string _Provider;
        public string Provider
        {
            get { return _Provider; }
            set { _Provider = value; }
        }

        public List<Connection> GetAll()
        {
            var listConnection = new List<Connection>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                foreach (XmlNode node in connections.SelectNodes("root/connections/connection"))
                {
                    var conn = new Connection
                    {
                        Name = node.SelectSingleNode("name").InnerText,
                        Server = node.SelectSingleNode("server").InnerText,
                        Database = node.SelectSingleNode("dbname").InnerText,
                        Username = node.SelectSingleNode("dbuser").InnerText,
                        Password = node.SelectSingleNode("dbpass").InnerText,
                        Provider = node.SelectSingleNode("provider").InnerText
                    };
                    listConnection.Add(conn);
                }
            }

            return listConnection;
        }

        public string GetAllJSON()
        {
            string result = "";

            var listConnection = new List<Connection>();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                foreach (XmlNode node in connections.SelectNodes("root/connections/connection"))
                {
                    var conn = new Connection
                    {
                        Name = node.SelectSingleNode("name").InnerText,
                        Server = node.SelectSingleNode("server").InnerText,
                        Database = node.SelectSingleNode("dbname").InnerText,
                        Username = node.SelectSingleNode("dbuser").InnerText,
                        Password = node.SelectSingleNode("dbpass").InnerText,
                        Provider = node.SelectSingleNode("provider").InnerText
                    };
                    listConnection.Add(conn);
                }

                result = new JavaScriptSerializer().Serialize(listConnection); 
            }

            return result;
        }

        public Connection GetBy(string name)
        {
            var conn = new Connection();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                var node = connections.SelectSingleNode("/root/connections/connection[name='" + name + "']");
                if (node != null)
                {
                    conn.Name = node.SelectSingleNode("name").InnerText;
                    conn.Server = node.SelectSingleNode("server").InnerText;
                    conn.Database = node.SelectSingleNode("dbname").InnerText;
                    conn.Username = node.SelectSingleNode("dbuser").InnerText;
                    conn.Password = node.SelectSingleNode("dbpass").InnerText;
                    conn.Provider = node.SelectSingleNode("provider").InnerText;
                }
                else
                {
                    conn = null;
                }
            }
            else
            {
                conn = null;
            }

            return conn;
        }

        public string GetByJSON(string name)
        {
            string result = "";

            var conn = new Connection();

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                var node = connections.SelectSingleNode("/root/connections/connection[name='" + name + "']");
                if (node != null)
                {
                    conn.Name = node.SelectSingleNode("name").InnerText;
                    conn.Server = node.SelectSingleNode("server").InnerText;
                    conn.Database = node.SelectSingleNode("dbname").InnerText;
                    conn.Username = node.SelectSingleNode("dbuser").InnerText;
                    conn.Password = node.SelectSingleNode("dbpass").InnerText;
                    conn.Provider = node.SelectSingleNode("provider").InnerText;

                    result = new JavaScriptSerializer().Serialize(conn); 
                }
            }

            return result;
        }

        public bool CheckExists(string name)
        {
            bool result = false;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                foreach (XmlNode node in connections.SelectNodes("root/connections/connection"))
                {
                    string _name = node.SelectSingleNode("name").InnerText;

                    if (name == _name)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public int Add()
        {
            int result = 0;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                bool check = CheckExists(Name);

                if (check)
                {
                    result = -1;
                }
                else
                {
                    var connections = security.ReadConnectionConfiguration(User.Current.Path);

                    var node = connections.SelectSingleNode("/root/connections");
                    var connectionNode = connections.CreateElement("connection");

                    node.AppendChild(connectionNode);

                    var nameNode = connections.CreateElement("name");
                    nameNode.AppendChild(connections.CreateTextNode(Name));
                    connectionNode.AppendChild(nameNode);

                    var serverNode = connections.CreateElement("server");
                    serverNode.AppendChild(connections.CreateTextNode(Server));
                    connectionNode.AppendChild(serverNode);

                    var dbNode = connections.CreateElement("dbname");
                    dbNode.AppendChild(connections.CreateTextNode(Database));
                    connectionNode.AppendChild(dbNode);

                    var dbuserNode = connections.CreateElement("dbuser");
                    dbuserNode.AppendChild(connections.CreateTextNode(Username));
                    connectionNode.AppendChild(dbuserNode);

                    var dbPassNode = connections.CreateElement("dbpass");
                    dbPassNode.AppendChild(connections.CreateTextNode(Password));
                    connectionNode.AppendChild(dbPassNode);

                    var dbproviderNode = connections.CreateElement("provider");
                    dbproviderNode.AppendChild(connections.CreateTextNode(Provider));
                    connectionNode.AppendChild(dbproviderNode);

                    security.SaveConnectionConfiguration(User.Current.Path, connections.InnerXml);

                    result = 1;
                }
            }
            else
            {
                result = -2;
            }

            return result;
        }

        public int Update()
        {
            int result = 0;

            if (User.Current != null)
            {
                bool check = CheckExists(Name);

                if (!check)
                {
                    result = -1;
                }
                else
                {
                    var security = new Security(User.Current.Password);
                    var connections = security.ReadConnectionConfiguration(User.Current.Path);

                    XmlNode node = connections.SelectSingleNode("/root/connections/connection[name='" + Name + "']");
                    if (node != null)
                    {
                        node.SelectSingleNode("name").InnerText = Name;
                        node.SelectSingleNode("server").InnerText = Server;
                        node.SelectSingleNode("dbname").InnerText = Database;
                        node.SelectSingleNode("dbuser").InnerText = Username;
                        node.SelectSingleNode("dbpass").InnerText = Password;
                        node.SelectSingleNode("provider").InnerText = Provider;

                        security.SaveConnection(User.Current.Path, connections);

                        result = 1;
                    }
                    else
                    {
                        result = -3;
                    }
                }
            }
            else
            {
                result = -2;
            }

            return result;
        }

        public bool Delete()
        {
            bool result = false;

            if (User.Current != null)
            {
                var security = new Security(User.Current.Password);
                var connections = security.ReadConnectionConfiguration(User.Current.Path);

                var node = connections.SelectSingleNode("/root/connections/connection[name='" + Name + "']");
                if (node != null)
                {
                    node.ParentNode.RemoveChild(node);
                    security.SaveConnectionConfiguration(User.Current.Path, connections.InnerXml);
                    result = true;
                }
            }

            return result;
        }

        public List<Connection> JSONToList(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<List<Connection>>(json);
            return result;
        }

        public Connection JSONToObject(string json)
        {
            var result = new JavaScriptSerializer().Deserialize<Connection>(json);
            return result;
        }

        public string GetDatabases(string name)
        {
            string result = "";

            if (User.Current != null)
            {
                Connection conn = new Connection();
                conn = GetBy(name);

                if (conn != null)
                {
                    if (conn.Provider == "System.Data.SqlClient")
                    {
                        var connection = new SqlConnectionStringBuilder()
                        {
                            DataSource = conn.Server,
                            InitialCatalog = conn.Database,
                            UserID = conn.Username,
                            Password = conn.Password
                        }.ConnectionString;

                        using (var sqlConn = new SqlConnection(connection))
                        {
                            sqlConn.Open();

                            List<string> databases = new List<string>();
                            DataTable dt = sqlConn.GetSchema("Databases");
                            foreach (DataRow row in dt.Rows)
                            {
                                string tablename = (string)row[0];
                                databases.Add(tablename);
                            }

                            sqlConn.Close();

                            result = new JavaScriptSerializer().Serialize(databases);
                        }
                    }
                }
            }

            return result;
        }

        public string GetTables(string name, string dbName)
        {
            string result = "";

            if (User.Current != null)
            {
                 var conn = new Connection();
                 conn = GetBy(name);
                
                var tables = new List<string>();

                if (conn != null)
                {
                    if (conn.Provider == "System.Data.SqlClient")
                    {
                        var connection = new SqlConnectionStringBuilder()
                        {
                            DataSource = conn.Server,
                            InitialCatalog = conn.Database,
                            UserID = conn.Username,
                            Password = conn.Password
                        }.ConnectionString; 

                        using (var sqlConn = new SqlConnection(connection))
                        {
                            sqlConn.Open();

                            var cmd = new SqlCommand();
                            if (string.IsNullOrEmpty(dbName))
                            {
                                cmd.CommandText = "USE " + conn.Database + " SELECT * FROM sys.Tables";
                            }
                            else
                            {
                                cmd.CommandText = "USE " + dbName + " SELECT * FROM sys.Tables";
                            }

                            cmd.Connection = sqlConn;

                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                tables.Add(reader[0].ToString());
                            }

                            sqlConn.Close();

                            result = new JavaScriptSerializer().Serialize(tables);
                        }
                    }
                }
            }

            return result;
        }

        public string GetColumns(string name, string dbName, string tableName)
        {
            string result = "";

            if (User.Current != null)
            {
                var conn = new Connection();
                conn = GetBy(name);

                var tables = new List<string>();

                if (conn != null)
                {
                    if (conn.Provider == "System.Data.SqlClient")
                    {
                        var connection = new SqlConnectionStringBuilder()
                        {
                            DataSource = conn.Server,
                            InitialCatalog = conn.Database,
                            UserID = conn.Username,
                            Password = conn.Password
                        }.ConnectionString; 

                        using (var sqlConn = new SqlConnection(connection))
                        {
                            sqlConn.Open();

                            var cmd = new SqlCommand
                            {
                                CommandText = "USE " + (string.IsNullOrEmpty(dbName) ? conn.Database : dbName) + " Select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tableName + "'",
                                Connection = sqlConn
                            };

                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                tables.Add(reader[3].ToString());
                            }

                            sqlConn.Close();

                            result = new JavaScriptSerializer().Serialize(tables);
                        }
                    }
                }
            }

            return result;
        }

        public string GetRows(string name, string dbName, string sql)
        {
            string result = "";

            if (User.Current != null)
            {
                var conn = new Connection();
                conn = GetBy(name);

                var tables = new DataTable();

                if (conn != null)
                {
                    if (conn.Provider == "System.Data.SqlClient")
                    {
                        var connection = new SqlConnectionStringBuilder()
                        {
                            DataSource = conn.Server,
                            InitialCatalog = conn.Database,
                            UserID = conn.Username,
                            Password = conn.Password
                        }.ConnectionString; 

                        using (var sqlConn = new SqlConnection(connection))
                        {
                            sqlConn.Open();

                            var cmd = new SqlCommand
                            {
                                CommandText = "USE " + (string.IsNullOrEmpty(dbName) ? conn.Database : dbName) + " " + sql,
                                Connection = sqlConn
                            };

                            var reader = cmd.ExecuteReader();

                            tables.Load(reader);

                            if (tables.Rows.Count > 0)
                            {
                                string rowDelimiter = "";

                                var sb = new StringBuilder("[");
                                foreach (DataRow row in tables.Rows)
                                {
                                    sb.Append(rowDelimiter);
                                    sb.Append(FromDataRow(row));
                                    rowDelimiter = ",";
                                }
                                sb.Append("]");

                                result = sb.ToString();
                            }

                            sqlConn.Close();
                        }
                    }
                }
            }

            return result;
        }

        public int ExcuteQuery(string name, string dbName, string sql)
        {
            int result = -1;

            if (User.Current != null)
            {
                var conn = new Connection();
                conn = GetBy(name);

                var tables = new DataTable();

                if (conn != null)
                {
                    if (conn.Provider == "System.Data.SqlClient")
                    {
                        var connection = new SqlConnectionStringBuilder()
                        {
                            DataSource = conn.Server,
                            InitialCatalog = conn.Database,
                            UserID = conn.Username,
                            Password = conn.Password
                        }.ConnectionString; 

                        using (var sqlConn = new SqlConnection(connection))
                        {
                            sqlConn.Open();

                            var cmd = new SqlCommand
                            {
                                CommandText = "USE " + (string.IsNullOrEmpty(dbName) ? conn.Database : dbName) + " " + sql,
                                Connection = sqlConn
                            };

                            result = cmd.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
            }

            return result;
        }

        #region Utilities

        private string FromDataRow(DataRow row)
        {
            DataColumnCollection cols = row.Table.Columns;
            string colDelimiter = "";

            var result = new StringBuilder("{");
            for (int i = 0; i < cols.Count; i++)
            {
                result.Append(colDelimiter).Append("\"")
                      .Append(cols[i].ColumnName).Append("\":")
                      .Append(JSONValueFromDataRowObject(row[i], cols[i].DataType));

                colDelimiter = ",";
            }
            result.Append("}");
            return result.ToString();
        }

        // possible types:
        // http://msdn.microsoft.com/en-us/library/system.data.datacolumn.datatype(VS.80).aspx
        private Type[] numeric = new Type[] {typeof(byte), typeof(decimal), typeof(double), 
                                     typeof(Int16), typeof(Int32), typeof(SByte), typeof(Single),
                                     typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        // I don't want to rebuild this value for every date cell in the table
        private long EpochTicks = new DateTime(1970, 1, 1).Ticks;

        private string JSONValueFromDataRowObject(object value, Type DataType)
        {
            // null
            if (value == DBNull.Value) return "null";

            // numeric
            if (Array.IndexOf(numeric, DataType) > -1)
                return value.ToString(); // TODO: eventually want to use a stricter format. Specifically: separate integral types from floating types and use the "R" (round-trip) format specifier

            // boolean
            if (DataType == typeof(bool))
                return ((bool)value) ? "true" : "false";

            // date -- see http://weblogs.asp.net/bleroy/archive/2008/01/18/dates-and-json.aspx
            if (DataType == typeof(DateTime))
                return "\"\\/Date(" + new TimeSpan(((DateTime)value).ToUniversalTime().Ticks - EpochTicks).TotalMilliseconds.ToString() + ")\\/\"";

            // TODO: add Timespan support
            // TODO: add Byte[] support

            //TODO: this would be _much_ faster with a state machine
            //TODO: way to select between double or single quote literal encoding
            //TODO: account for database strings that may have single \r or \n line breaks
            // string/char  
            return "\"" + value.ToString().Replace(@"\", @"\\").Replace(Environment.NewLine, @"\n").Replace("\"", @"\""") + "\"";
        }

        #endregion
    }
}
