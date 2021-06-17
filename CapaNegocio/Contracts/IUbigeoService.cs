using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Contracts
{
    public interface IUbigeoService
    {
        Task<List<UBIGEO>> GetAllDepartamentsAsync();
        Task<List<UBIGEO>> GetAllProvincesAsync(string departamentId);
        Task<List<UBIGEO>> GetAllDistrictsAsync(string provinceId);
    }
}
