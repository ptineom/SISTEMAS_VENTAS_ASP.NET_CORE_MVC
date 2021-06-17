
using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository _securityRepository;
        public SecurityService(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;
        }

        public async Task<List<APLICACION>> GetMenuByUserIdAsync(string userId)
        {
            var result = await _securityRepository.GetMenuByUserId(userId);
            if (result != null)
            {
                //Agregando el menú home
                int idMax = (result.Select(x => x.ID_APLICACION).Max() + 1);
                result.Insert(1, new APLICACION()
                {
                    ID_APLICACION_PADRE = 1,
                    ID_APLICACION = idMax,
                    NOM_APLICACION = "Home",
                    FLG_FORMULARIO = true,
                    NOM_FORMULARIO = "Index",
                    ICON = "bi bi-house-fill",
                    NOM_CONTROLADOR = "Home",
                    FLG_RAIZ = false,
                    BREADCRUMS = $"{idMax}|Home"
                });
            }

            return result;
        }

        public async Task<ResponseObject> UserValidateAsync(string userId, string password, bool noValidar = false)
        {
            try
            {
                var result = await _securityRepository.UserValidateAsync(userId, password, noValidar);
                return new ResponseObject() { Data = result };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.NOT_FOUND,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = 404,
                        Message = ex.Message
                    }
                };
            }
        }

      

    }
}
