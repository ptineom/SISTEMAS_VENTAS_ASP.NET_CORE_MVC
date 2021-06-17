using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<List<T>> GetAllAsync(T obj);
        Task<bool> RegisterAsync(T obj, SqlTransaction transaction = null);
        Task<bool> UpdateAsync(T obj, SqlTransaction transaction = null);
        Task<bool> DeleteAsync(T obj, SqlTransaction transaction = null);
    }
}
