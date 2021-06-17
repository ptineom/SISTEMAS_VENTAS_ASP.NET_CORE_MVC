using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Entidades;
namespace CapaDao.Contracts
{
    public interface IDocCompraRepository
    {
        Task<OBJETOS_DOC_COMPRA> GetLoadObjects(string campusId, string userId);
        Task<List<DOC_COMPRA_LISTADO>> GetAllAsync(DOC_COMPRA obj);
        Task<DOC_COMPRA> GetByIdAsync(DOC_COMPRA obj);
        Task<bool> RegisterAsync(DOC_COMPRA obj, SqlTransaction transaction = null);
        Task<bool> DeleteAsync(DOC_COMPRA obj, SqlTransaction transaction = null);
    }
}
