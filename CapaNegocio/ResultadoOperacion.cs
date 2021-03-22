using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class ResultadoOperacion : IResultadoOperacion
    {
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
        public object Data { get; set; }

        public ResultadoOperacion()
        {
            Resultado = false;
            Mensaje = "Ocurrio un error inesperado";
        }
        public void SetResultado(bool resultado, string mensaje)
        {
            if (!resultado && string.IsNullOrEmpty(mensaje))
            {
                mensaje = "Ocurrio un error inesperado";
            }
            else
            {
                Mensaje = mensaje;
            }
            Resultado = resultado;
        }

        public void SetResultado(bool resultado, string mensaje, object modelo)
        {
            if (!resultado && string.IsNullOrEmpty(mensaje))
            {
                mensaje = "Ocurrio un error inesperado";
            }
            else
            {
                Mensaje = mensaje;
            }
            Resultado = resultado;
            Data = modelo;
        }

        public void SetResultado(bool resultado, object modelo)
        {
            if (!resultado)
            {
                Mensaje = "Ocurrio un error inesperado";
            }
            else
            {
                Mensaje = "";
            }
            Resultado = resultado;
            Data = modelo;
        }
    }
}
