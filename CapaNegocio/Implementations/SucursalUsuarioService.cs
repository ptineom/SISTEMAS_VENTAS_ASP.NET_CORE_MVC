using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class SucursalUsuarioService : ISucursalUsuarioService
    {
        private ISucursalUsuarioRepository _sucursalUsuarioRepository;
        public SucursalUsuarioService(ISucursalUsuarioRepository sucursalUsuarioRepository)
        {
            _sucursalUsuarioRepository = sucursalUsuarioRepository;
        }
        public Task<List<SUCURSAL>> GetAllByCampusId(string userId)
        {
            return _sucursalUsuarioRepository.GetAllByCampusId(userId);
        }
    }
}
