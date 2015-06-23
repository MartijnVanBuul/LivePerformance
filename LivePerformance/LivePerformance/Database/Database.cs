using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace LivePerformance.Database
{
    public class Database
    {
        //Fields
        private OracleConnection _connection;

        //Properties

        //Constructor
        
        //Methods
        public bool Connect()
        {
            try
            {
                _connection = new OracleConnection();
                _connection.ConnectionString = "Data Source=//192.168.15.50:1521/fhictora;User Id=dbi329063;Password=d1Ftx896xr;";
                _connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public void EditDatabase(string query)
        {
            if (Connect())
            {
                using (OracleCommand oracleCommand = new OracleCommand(query))
                {
                    using (oracleCommand.Connection = _connection)
                    {
                        oracleCommand.ExecuteNonQuery();
                    }
                }

                Close();
            }
        }

        public List<string>[] SelectQuery(string query, List<string> columnNames)
        {
            if (Connect())
            {
                List<string>[] dataTable = new List<string>[columnNames.Count()];

                for (int i = 0; i < columnNames.Count(); i++)
                {
                    dataTable[i] = new List<string>();
                    dataTable[i].Add(columnNames[i]);
                }

                using (OracleCommand oracleCommand = new OracleCommand(query))
                {
                    using (oracleCommand.Connection = _connection)
                    {
                        oracleCommand.Parameters.Add(":port_id", 1521);
                        OracleDataReader reader = oracleCommand.ExecuteReader();

                        while (reader.Read())
                        {
                            for (int i = 0; i < columnNames.Count(); i++)
                            {
                                dataTable[i].Add(Convert.ToString(reader[columnNames[i]]));
                            }
                        }
                        Close();
                    }
                }
                return dataTable;
            }
            return null;
        }

        public List<string>[] ExecuteProcedure(string procedureName, List<string> columns, List<OracleParameter> procedureInParameter, List<OracleParameter> procedureOutParameter)
        {
            List<string>[] dataTable = new List<string>[columns.Count()];

            for (int i = 0; i < columns.Count(); i++)
            {
                dataTable[i] = new List<string>();
            }

            if (Connect())
            {
                OracleCommand oracleCommand = new OracleCommand();

                oracleCommand.Connection = _connection;
                oracleCommand.CommandText = procedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter parameter in procedureInParameter)
                {
                    oracleCommand.Parameters.Add(parameter);
                }

                foreach (OracleParameter parameter in procedureOutParameter)
                {
                    oracleCommand.Parameters.Add(parameter).Direction = ParameterDirection.Output;
                }

                OracleDataReader reader = oracleCommand.ExecuteReader(CommandBehavior.CloseConnection);


                while (reader.Read())
                {
                    for (int i = 0; i < columns.Count(); i++)
                    {
                        dataTable[i].Add(Convert.ToString(reader[columns[i]]));
                    }
                }
            }

            return dataTable;
        }
    }
}