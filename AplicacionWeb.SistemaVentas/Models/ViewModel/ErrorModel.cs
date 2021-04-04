using System;

namespace AplicacionWeb.SistemaVentas.Models.ViewModel
{
    public class ErrorModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
