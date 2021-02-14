using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public interface IResultadoOperacion
    {
        bool bResultado { get; set; }
        string sMensaje { get; set; }
        object data { get; set; }

        void SetResultado(bool resultado, string mensaje);
        void SetResultado(bool resultado, string mensaje, object modelo);
        void SetResultado(bool resultado, object modelo);
    }
}
