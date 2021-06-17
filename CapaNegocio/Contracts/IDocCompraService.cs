using Entidades;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface IDocCompraService
    {
        Task<OBJETOS_DOC_COMPRA> GetLoadObjects(string campusId, string userId);
        Task<List<DOC_COMPRA_LISTADO>> GetAllAsync(DOC_COMPRA obj);
        Task<DOC_COMPRA> GetByIdAsync(DOC_COMPRA obj);
        Task<ResponseObject> RegisterAsync(DOC_COMPRA obj);
        Task<ResponseObject> DeleteAsync(DOC_COMPRA obj);
    }
}
