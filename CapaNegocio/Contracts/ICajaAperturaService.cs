using Entidades;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface ICajaAperturaService
    {
        Task<CAJA_APERTURA> GetBoxStateAsync(CAJA_APERTURA obj);
        Task<DINERO_EN_CAJA> GetTotalsByUserIdAsync(CAJA_APERTURA obj);
        Task<ResponseObject> RegisterAsync(CAJA_APERTURA obj);
        Task<ResponseObject> ValidateBoxAsync(CAJA_APERTURA obj);
        Task<List<CAJA_APERTURA>> GetAllAsync(CAJA_APERTURA obj);
        Task<ResponseObject> ReopenBoxAsync(CAJA_APERTURA obj);
    }
}
