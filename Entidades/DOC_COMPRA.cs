using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class DOC_COMPRA
    {
        public string ACCION { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string ID_TIPO_COMPROBANTE { get; set; }
        public string  NRO_SERIE { get; set; }
        public int NRO_DOCUMENTO { get; set; }
        public string ID_PROVEEDOR { get; set; }
        public string ID_MONEDA { get; set; }
        public string FEC_DOCUMENTO { get; set; }
        public int EST_DOCUMENTO { get; set; }
        public string OBS_COMPRA { get; set; }
        public decimal TOT_BRUTO { get; set; }
        public decimal TOT_DESCUENTO { get; set; }
        public decimal TOT_IMPUESTO { get; set; }
        public decimal TOT_COMPRA { get; set; }
        public decimal TOT_COMPRA_REDONDEO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public string ID_TIPO_PAGO { get; set; }
        public decimal TC_COMPRA { get; set; }
        public string ID_TIPO_CONDICION_PAGO { get; set; }
        public decimal TAS_DESCUENTO { get; set; }
        public decimal TAS_IGV { get; set; }
        // 
        public decimal ABONO { get; set; }
        public decimal SALDO { get; set; }
        public string FECHA_CANCELACION { get; set; }
        public string JSON_ARTICULOS { get; set; }

        public List<TIPO_DOCUMENTO> listaDocumentos { get; set; }
        public List<TIPO_COMPROBANTE> listaComprobantes { get; set; }
        public List<MONEDA> listaMonedas { get; set; }
        public List<TIPO_PAGO> listaTipPag { get; set; }
        public List<TIPO_CONDICION_PAGO> listaTipCon { get; set; }
        public List<ESTADO> listaEstados { get; set; }

        public List<UBIGEO> listaDepartamentos { get; set; }
        public List<DOC_COMPRA_DETALLE> detalle { get; set; }
        //
        public string NOM_PROVEEDOR { get; set; }
        public string DIR_PROVEEDOR { get; set; }
        public int ID_TIPO_DOCUMENTO { get; set; }
        public string NRO_DOCUMENTO_PROVEEDOR { get; set; }
        //
        public bool FLG_RETIRAR_CAJA { get; set; }
        public decimal MONTO_RETIRA_CAJA { get; set; }
        public string ID_CAJA_CA { get; set; }
        public string ID_USUARIO_CA { get; set; }
        public int CORRELATIVO_CA { get; set; }
        public string SGN_MONEDA { get; set; }


    }
    public class DOC_COMPRA_LISTADO : DOC_COMPRA
    {
        public string COMPROBANTE { get; set; }
        public string DOC_PROVEEDOR { get; set; }

        public string NOM_TIPO_CONDICION_PAGO { get; set; }
        public string NOM_ESTADO { get; set; }
        public bool FLG_NO_EVALUA_CREDITO { get; set; }
    }
    public class DOC_COMPRA_INFORME: DOC_COMPRA
    {
        public string COMPROBANTE { get; set; }
        public string NOM_TIPO_CONDICION_PAGO { get; set; }
        public string NOM_TIPO_PAGO { get; set; }
        public string TEL_PROVEEDOR { get; set; }
    }
    public class DOC_COMPRA_DETALLE
    {
        public string ID_ARTICULO { get; set; }
        public string NOM_ARTICULO { get; set; }
        public decimal PRECIO_ARTICULO { get; set; }
        public string ID_UM { get; set; }
        public string NOM_UM { get; set; }
        public decimal CANTIDAD { get; set; }
        public decimal TAS_DESCUENTO { get; set; }
        public decimal IMPORTE { get; set; }
        public decimal NRO_FACTOR { get; set; }
        public string XML_UM { get; set; }
        public decimal PRECIO_COMPRA { get; set; }
    }
}
