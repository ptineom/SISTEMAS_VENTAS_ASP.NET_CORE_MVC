using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class UbigeoService : IUbigeoService
    {
        private readonly IUbigeoRepository _ubigeoRepository;
        public UbigeoService(IUbigeoRepository ubigeoRepository)
        {
            _ubigeoRepository = ubigeoRepository;
        }
        public async Task<List<UBIGEO>> GetAllDepartamentsAsync()
        {
            return await _ubigeoRepository.GetAllDepartamentsAsync();
        }

        public async Task<List<UBIGEO>> GetAllDistrictsAsync(string provinceId)
        {
            return await _ubigeoRepository.GetAllDistrictsAsync(provinceId);
        }

        public async Task<List<UBIGEO>> GetAllProvincesAsync(string departamentId)
        {
            return await _ubigeoRepository.GetAllProvincesAsync(departamentId);
        }
    }
}
