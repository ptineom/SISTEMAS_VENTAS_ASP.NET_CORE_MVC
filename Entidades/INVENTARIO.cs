using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class INVENTARIO
    {
        public string ACCION { get; set; }
        public string ID_SUCURSAL { get; set; }
        public int NRO_INVENTARIO { get; set; }
        public string ID_USUARIO_INVENTARIO { get; set; }
        public string FECHA_INVENTARIO { get; set; }
        public string OBSERVACION { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public int ID_ESTADO { get; set; }
        public string ID_USUARIO_APROBACION { get; set; }
        public string FEC_APROBACION { get; set; }
        public string ID_TIPO_INVENTARIO { get; set; }
        public string NOM_ESTADO { get; set; }
        public string CADENA_ARTICULOS { get; set; }
        public List<INVENTARIO_DETALLE> listaDetalle { get; set; }
    }
    public class INVENTARIO_DETALLE
    {
        public string ACCION { get; set; }
        public string ID_SUCURSAL { get; set; }
        public int NRO_INVENTARIO { get; set; }
        public string ID_ARTICULO { get; set; }
        public decimal STOCK_VIRTUAL { get; set; }
        public decimal STOCK_FISICO { get; set; }
        public decimal DIFERENCIA { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        //
        public string CODIGO { get; set; }
        public string NOM_ARTICULO { get; set; }
        public string NOM_MARCA { get; set; }
        public string NOM_UM { get; set; }
    }

    public class KARDEX
    {
        public string ID_ARTICULO { get; set; }
        public string NOM_ARTICULO { get; set; }
        public string NOM_CPTO_MOVIMIENTO { get; set; }
        public string DOCUMENTO { get; set; }
        public DateTime FECHA_MOVIMIENTO { get; set; }
        public decimal CAN_SALDO_INICIAL { get; set; }
        public decimal CAN_INGRESO { get; set; }
        public decimal CAN_SALIDA { get; set; }
        public decimal CAN_SALDO_FINAL { get; set; }
        public string NOM_UM { get; set; }
        public decimal STOCK_ACTUAL { get; set; }
    }
}
