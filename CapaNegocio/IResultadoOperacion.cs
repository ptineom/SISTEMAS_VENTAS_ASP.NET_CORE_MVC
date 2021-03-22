using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public interface IResultadoOperacion
    {
        bool Resultado { get; set; }
        string Mensaje { get; set; }
        object Data { get; set; }

        void SetResultado(bool resultado, string mensaje);
        void SetResultado(bool resultado, string mensaje, object modelo);
        void SetResultado(bool resultado, object modelo);
    }
}
