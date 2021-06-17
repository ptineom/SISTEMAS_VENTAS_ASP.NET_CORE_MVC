using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class SucursalCajaUsuarioService : ISucursalCajaUsuarioService
    {
        private readonly ISucursalCajaUsuarioRepository _sucursalCajaUsuarioRepository;
        public SucursalCajaUsuarioService(ISucursalCajaUsuarioRepository sucursalCajaUsuarioRepository)
        {
            _sucursalCajaUsuarioRepository = sucursalCajaUsuarioRepository;
        }
        public async Task<List<CAJA>> GetAllBoxes(SUCURSAL_CAJA_USUARIO obj)
        {
            return await _sucursalCajaUsuarioRepository.GetAllBoxes(obj);
        }

        public async Task<List<USUARIO>> GetAllUsersByCampusId(string campusId)
        {
            return await _sucursalCajaUsuarioRepository.GetAllUsersByCampusId(campusId);
        }
    }
}
