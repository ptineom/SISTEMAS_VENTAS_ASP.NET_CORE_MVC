using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface ICajaAperturaRepository
    {
        Task<CAJA_APERTURA> GetBoxStateAsync(CAJA_APERTURA obj);
        Task<DINERO_EN_CAJA> GetTotalsByUserIdAsync(CAJA_APERTURA obj);
        Task<CAJA_APERTURA> RegisterAsync(CAJA_APERTURA obj, SqlTransaction transaction = null);
        Task<bool> ValidateBoxAsync(CAJA_APERTURA obj, SqlTransaction transaction = null);
        Task<List<CAJA_APERTURA>> GetAllAsync(CAJA_APERTURA obj);
        Task<bool> ReopenBoxAsync(CAJA_APERTURA obj, SqlTransaction transaction = null);
    }
}
