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
    public class DocCompraService : IDocCompraService
    {
        private readonly IDocCompraRepository _docCompraRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DocCompraService(IDocCompraRepository docCompraRepository, IUnitOfWork unitOfWork)
        {
            _docCompraRepository = docCompraRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseObject> DeleteAsync(DOC_COMPRA obj)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _docCompraRepository.DeleteAsync(obj, _unitOfWork.Transaction);
                _unitOfWork.Commit();
                return new ResponseObject() { Message = Constantes.DELETE_SUCCESS};
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

        public async Task<List<DOC_COMPRA_LISTADO>> GetAllAsync(DOC_COMPRA obj)
        {
           return await _docCompraRepository.GetAllAsync(obj);
        }

        public async Task<DOC_COMPRA> GetByIdAsync(DOC_COMPRA obj)
        {
            return await _docCompraRepository.GetByIdAsync(obj);
        }

        public async Task<OBJETOS_DOC_COMPRA> GetLoadObjects(string campusId, string userId)
        {
            return await _docCompraRepository.GetLoadObjects(campusId, userId);
        }

        public async Task<ResponseObject> RegisterAsync(DOC_COMPRA obj)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                await _docCompraRepository.RegisterAsync(obj, _unitOfWork.Transaction);
                _unitOfWork.Commit();
                return new ResponseObject() { Message = Constantes.INSERT_SUCCESS };
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
    }
}
