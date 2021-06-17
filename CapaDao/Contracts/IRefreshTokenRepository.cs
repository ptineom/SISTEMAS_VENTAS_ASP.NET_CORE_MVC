using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface IRefreshTokenRepository
    {
        Task<bool> RegisterAsync(REFRESH_TOKEN obj, SqlTransaction transaction = null);
        Task<REFRESH_TOKEN> GetByIdAsync(string idRefreshToken);
    }
}
