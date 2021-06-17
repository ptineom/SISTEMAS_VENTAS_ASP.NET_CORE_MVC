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
    public class CajaAperturaService : ICajaAperturaService
    {
        private readonly ICajaAperturaRepository _cajaAperturaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CajaAperturaService(ICajaAperturaRepository cajaAperturaRepository, IUnitOfWork unitOfWork)
        {
            _cajaAperturaRepository = cajaAperturaRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<CAJA_APERTURA>> GetAllAsync(CAJA_APERTURA obj)
        {
            return await _unitOfWork.CajaAperturaRepository.GetAllAsync(obj);
        }

        public async Task<CAJA_APERTURA> GetBoxStateAsync(CAJA_APERTURA obj)
        {
            return await _cajaAperturaRepository.GetBoxStateAsync(obj);
        }

        public async Task<DINERO_EN_CAJA> GetTotalsByUserIdAsync(CAJA_APERTURA obj)
        {
            return await _cajaAperturaRepository.GetTotalsByUserIdAsync(obj);
        }

        public async Task<ResponseObject> RegisterAsync(CAJA_APERTURA obj)
        {
            try
            {
                CAJA_APERTURA result = await _unitOfWork.CajaAperturaRepository.RegisterAsync(obj);

                return new ResponseObject()
                {
                    Message = Constantes.INSERT_SUCCESS,
                    Data = result
                };
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

        public async Task<ResponseObject> ReopenBoxAsync(CAJA_APERTURA obj)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.CajaAperturaRepository.ReopenBoxAsync(obj, _unitOfWork.Transaction);
                _unitOfWork.Commit();
                return new ResponseObject() { };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails() { StatusCode = 500, Message = ex.Message }
                };
            }
        }

        public async Task<ResponseObject> ValidateBoxAsync(CAJA_APERTURA obj)
        {
            try
            {
                await _unitOfWork.CajaAperturaRepository.ValidateBoxAsync(obj);

                return new ResponseObject() { };
            }
            catch (Exception ex)
            {
                return new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails() { StatusCode = 500, Message = ex.Message }
                };
            }
        }
    }
}
