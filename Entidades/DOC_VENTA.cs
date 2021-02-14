using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class DOC_VENTA
	{
		public string ACCION { get; set; }
		public string ID_SUCURSAL { get; set; }
		public string ID_TIPO_COMPROBANTE { get; set; }
		public string NRO_SERIE { get; set; }
		public int NRO_DOCUMENTO { get; set; }
		public string ID_CLIENTE { get; set; }
		public string ID_TIPO_CONDICION_PAGO { get; set; }
		public string ID_MONEDA { get; set; }
		public string FEC_DOCUMENTO { get; set; }
		public int EST_DOCUMENTO { get; set; }
		public decimal TOT_BRUTO { get; set; }
		public decimal TOT_DESCUENTO { get; set; }
		public decimal TOT_IMPUESTO { get; set; }
		public decimal TOT_VENTA { get; set; }
        public decimal TOT_VENTA_REDONDEO { get; set; }
		public string OBS_VENTA { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public string ID_TIPO_PAGO { get; set; }
		public decimal TC_VENTA { get; set; }
        public decimal TAS_DESCUENTO { get; set; }
        public string ID_CAJA_CA { get; set; }
        public string ID_USUARIO_CA { get; set; }
        public int CORRELATIVO_CA { get; set; }
        public decimal DIFERENCIA { get; set; }
        public string JSON_ARTICULOS { get; set; }
        //
        public List<TIPO_DOCUMENTO> listaDocumentos { get; set; }
        public List<TIPO_COMPROBANTE> listaComprobantes { get; set; }
        public List<MONEDA> listaMonedas { get; set; }
        public List<TIPO_PAGO> listaTipPag { get; set; }
        public List<TIPO_CONDICION_PAGO> listaTipCon { get; set; }
        public List<ESTADO> listaEstados { get; set; }
        public List<DOC_VENTA_DETALLE> listaDetalle { get; set; }
        public List<USUARIO> listaUsuarios { get; set; }
        public List<UBIGEO> listaDepartamentos { get; set; }
        //
        public decimal ABONO { get; set; }
        public decimal SALDO { get; set; }
        public string FEC_VENCIMIENTO { get; set; }
        //
        public string COMPROBANTE { get; set; }
        public string DOC_CLIENTE { get; set; }
        public string NOM_CLIENTE { get; set; }
        public bool FLG_NO_EVALUA_CREDITO { get; set; }
        public string NOM_TIPO_CONDICION_PAGO { get; set; }
        public string NOM_ESTADO { get; set; }
        public string SGN_MONEDA { get; set; }
        public string DIR_CLIENTE { get; set; }
        public int ID_TIPO_DOCUMENTO { get; set; }
        public string NRO_DOCUMENTO_CLIENTE { get; set; }
        //
        public string TOT_VENTA_EN_LETRAS { get; set; }
        public string NOM_CAJA { get; set; }
        public string NOM_USUARIO_CAJA { get; set; }
        public string HOR_DOCUMENTO { get; set; }
        public string EMAIL_CLIENTE { get; set; }
    }

    public class DOC_VENTA_DETALLE
    {
        public string ACCION { get; set; }
        public string ID_ARTICULO { get; set; }
        public string ID_ALMACEN { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string ID_TIPO_COMPROBANTE { get; set; }
        public string NRO_SERIE { get; set; }
        public int NRO_DOCUMENTO { get; set; }
        public decimal CANTIDAD { get; set; }
        public string ID_UM { get; set; }
        public decimal NRO_FACTOR { get; set; }
        public decimal PRECIO_ARTICULO { get; set; }
        public decimal TAS_DESCUENTO { get; set; }
        public decimal TAS_IGV { get; set; }
        public decimal IMPORTE { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        ///
        public string NOM_ARTICULO { get; set; }
        public string NOM_UM { get; set; }
        public string ABREVIADO { get; set; }
        public decimal PRECIO_UNITARIO { get; set; }
    }
   
    public class CONSULTA_X_MES
    {
        public string NOMBRE_MES { get; set; }
        public decimal MONTO_TOTAL { get; set; }
        public decimal PORCENTAJE { get; set; }
        public string SGN_MONEDA { get; set; }
    }
}
