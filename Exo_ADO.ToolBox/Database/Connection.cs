using Exo_ADO.ToolBox.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exo_ADO.ToolBox.Database
{
    public sealed class Connection
    {
        public string ConnectionString { get; init; }

        public Connection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private SqlConnection? _DbConnection;
        #region Gestion de l'ouverture et fermeture de la connexion

        public void Open()
        {
            if (_DbConnection != null)
            {
                throw new ConnectionException("Connection already open !");
            }

            _DbConnection = new SqlConnection(ConnectionString);
            _DbConnection.Open();
        }

        public void Close()
        {
            if (_DbConnection != null)
            {
                _DbConnection.Close();
                _DbConnection = null;
            }
        }
        #endregion

        #region Méthode privé
        private SqlCommand CreateSqlCommand(Command command)
        {
            if (_DbConnection is null)
            {
                throw new ConnectionException("SQL Connection is not open !");
            }

            // Command => Query / StoredProc? / Parametres
            SqlCommand dbCommand = _DbConnection.CreateCommand();
            dbCommand.CommandText = command.Query;
            dbCommand.CommandType = command.IsStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

            foreach (KeyValuePair<string, object> param in command.Parameters)
            {
                SqlParameter dbParameter = new SqlParameter();
                dbParameter.ParameterName = param.Key;
                dbParameter.Value = param.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }
        #endregion

        #region Méthode => ExecuteNonQuery, ExecuteScalar, ExecuteReader
        public int ExecuteNonQuery(Command command)
        {
            using (SqlCommand dbCommand = CreateSqlCommand(command))
            {
                return dbCommand.ExecuteNonQuery();
            }
        }

        public TResult? ExecuteScalar<TResult>(Command command)
        {
            //  Implementation générique, pour éviter de caster le resultat lors de l'utilisation

            using (SqlCommand dbCommand = CreateSqlCommand(command))
            {
                object result = dbCommand.ExecuteScalar();

                return result is DBNull ? default : (TResult)result;
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> convert)
        {
            using(SqlCommand dbCommand = CreateSqlCommand(command))
            {
                using(SqlDataReader reader = dbCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        // Utilisation d'un délégué "Func" pour obtenir le resultat converti en TResult
                        TResult result = convert(reader);

                        // Renvoi de resultat différé
                        yield return result;
                    }
                }
            }
        }
        #endregion
    }
}