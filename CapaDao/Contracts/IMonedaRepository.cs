using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Contracts
{
    public interface IMonedaRepository
    {
        Task<List<MONEDA>> GetAllAsync();
    }
}
