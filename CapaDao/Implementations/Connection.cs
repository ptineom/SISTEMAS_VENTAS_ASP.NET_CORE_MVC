using CapaDao.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CapaDao.Implementations
{
    public sealed class Connection : IConnection, IDisposable
    {
        private IDbConnection _dbConnection;

        public Connection(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;

            if (_dbConnection == null)
                _dbConnection = new SqlConnection(_dbConnection.ConnectionString);

            _dbConnection.Close();

            if (_dbConnection.State != ConnectionState.Open)
                _dbConnection.Open();
        }

        public SqlConnection DbConnection
        {
            get
            {
                return (SqlConnection)_dbConnection;
            }
        }

        public void CloseConnection()
        {
            if (_dbConnection != null && _dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }
        public void Dispose()
        {
            CloseConnection();
        }
    }
}
