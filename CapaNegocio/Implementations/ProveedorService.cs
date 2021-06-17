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
    public class ProveedorService : IProveedorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProveedorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PROVEEDOR>> GetAllAsync(PROVEEDOR obj)
        {
            return await _unitOfWork.ProveedorRepository.GetAllAsync(obj);
        }

        public async Task<PROVEEDOR> GetByDocument(PROVEEDOR obj)
        {
            return await _unitOfWork.ProveedorRepository.GetByDocument(obj);
        }

        public async Task<PROVEEDOR> GetByIdAsync(string id)
        {
            return await _unitOfWork.ProveedorRepository.GetByIdAsync(id);
        }

        public async Task<ResponseObject> RegisterAsync(PROVEEDOR obj)
        {
            try
            {
                await _unitOfWork.ProveedorRepository.RegisterAsync(obj);
                return new ResponseObject() { Message = Constantes.INSERT_SUCCESS, Data = obj.ID_PROVEEDOR };
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

        public async Task<ResponseObject> UpdateAsync(PROVEEDOR obj)
        {
            try
            {
                await _unitOfWork.ProveedorRepository.UpdateAsync(obj);
                return new ResponseObject() { Message = Constantes.UPDATE_SUCCESS };
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

        public async Task<ResponseObject> DeleteAsync(PROVEEDOR obj)
        {
            try
            {
                await _unitOfWork.ProveedorRepository.DeleteAsync(obj);
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

    }
}
