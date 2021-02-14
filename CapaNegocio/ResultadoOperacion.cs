using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class ResultadoOperacion : IResultadoOperacion
    {
        public bool bResultado { get; set; }
        public string sMensaje { get; set; }
        public object data { get; set; }

        public ResultadoOperacion()
        {
            bResultado = false;
            sMensaje = "Ocurrio un error inesperado";
        }
        public void SetResultado(bool resultado, string mensaje)
        {
            if (!resultado && string.IsNullOrEmpty(mensaje))
            {
                mensaje = "Ocurrio un error inesperado";
            }
            else
            {
                sMensaje = mensaje;
            }
            bResultado = resultado;
        }

        public void SetResultado(bool resultado, string mensaje, object modelo)
        {
            if (!resultado && string.IsNullOrEmpty(mensaje))
            {
                mensaje = "Ocurrio un error inesperado";
            }
            else
            {
                sMensaje = mensaje;
            }
            bResultado = resultado;
            data = modelo;
        }

        public void SetResultado(bool resultado, object modelo)
        {
            if (!resultado)
            {
                sMensaje = "Ocurrio un error inesperado";
            }
            else
            {
                sMensaje = "";
            }
            bResultado = resultado;
            data = modelo;
        }
    }
}
