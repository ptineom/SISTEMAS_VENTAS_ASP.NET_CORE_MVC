using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<REFRESH_TOKEN> GetByIdAsync(string id)
        {
            return await _refreshTokenRepository.GetByIdAsync(id);
        }
        public async Task<ResponseObject> RegisterAsync(REFRESH_TOKEN obj)
        {
            try
            {
                await _refreshTokenRepository.RegisterAsync(obj);
                return new ResponseObject() { Message = Constantes.INSERT_SUCCESS };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.INSERT_ERROR,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = 500,
                        Message = ex.Message
                    }
                };
            }
        }
    }
}
