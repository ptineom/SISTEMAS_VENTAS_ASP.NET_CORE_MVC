using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface IUbigeoRepository
    {
        Task<List<UBIGEO>> GetAllDepartamentsAsync();
        Task<List<UBIGEO>> GetAllProvincesAsync(string departamentId);
        Task<List<UBIGEO>> GetAllDistrictsAsync(string provinceId);
    }
}
