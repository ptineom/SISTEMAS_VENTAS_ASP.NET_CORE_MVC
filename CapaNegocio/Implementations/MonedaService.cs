using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class MonedaService : IMonedaService
    {
        private readonly IMonedaRepository _monedaRepository;
        public MonedaService(IMonedaRepository monedaRepository)
        {
            _monedaRepository = monedaRepository;
        }
        public async Task<List<MONEDA>> GetAllAsync()
        {
            return await _monedaRepository.GetAllAsync();
        }
    }
}
