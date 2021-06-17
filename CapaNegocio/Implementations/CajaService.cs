using CapaDao.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Implementations
{
    public class CajaService : ICajaService
    {
        private readonly ICajaRepository _cajaRepository;
        public CajaService(ICajaRepository cajaRepository)
        {
            _cajaRepository = cajaRepository;
        }
        public async Task<ResponseObject> DeleteAsync(CAJA obj)
        {
            try
            {
                await _cajaRepository.DeleteAsync(obj);
                return new ResponseObject() { Message = Constantes.DELETE_SUCCESS };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.DELETE_ERROR,
                    ErrorDetails = new ErrorDetails() { StatusCode = 500, Message = ex.Message }
                };
            }
        }

        public async Task<List<CAJA>> GetAllAsync(CAJA obj)
        {
            return await _cajaRepository.GetAllAsync(obj);
        }

        public async Task<CAJA> GetByIdAsync(string id)
        {
            return await _cajaRepository.GetByIdAsync(id);
        }

        public async Task<ResponseObject> RegisterAsync(CAJA obj)
        {
            try
            {
                await _cajaRepository.RegisterAsync(obj);
                return new ResponseObject() { Message = Constantes.INSERT_SUCCESS };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.INSERT_ERROR,
                    ErrorDetails = new ErrorDetails() { StatusCode = 500, Message = ex.Message }
                };
            }
        }

        public async Task<ResponseObject> UpdateAsync(CAJA obj)
        {
            try
            {
                await _cajaRepository.UpdateAsync(obj);
                return new ResponseObject() { Message = Constantes.UPDATE_SUCCESS };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.UPDATE_ERROR,
                    ErrorDetails = new ErrorDetails() { StatusCode = 500, Message = ex.Message }
                };
            }
        }

    }
}
