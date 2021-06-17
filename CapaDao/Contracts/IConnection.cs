using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CapaDao.Contracts
{
    public interface IConnection
    {
        SqlConnection DbConnection { get; }
        void CloseConnection();
    }
}
