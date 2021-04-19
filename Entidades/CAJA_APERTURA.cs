using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{

	public class CAJA_APERTURA
	{
        public string ACCION { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string ID_CAJA { get; set; }
        public string ID_USUARIO { get; set; }
        public int CORRELATIVO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
		public string FECHA_APERTURA { get; set; }
        public string FECHA_CIERRE { get; set; }
        public string HORA_CIERRE { get; set; }
        public decimal MONTO_APERTURA { get; set; }
		public bool FLG_CIERRE { get; set; }
		public decimal MONTO_COBRADO { get; set; }
        public string ID_MONEDA { get; set; }
        //
        public string NOM_CAJA { get; set; }
        public string SGN_MONEDA { get; set; }
        public string NOM_USUARIO { get; set; }
        public int ITEM { get; set; }
        public bool FLG_REAPERTURADO { get; set; }
        public string NOM_MONEDA { get; set; }
        public bool FLG_CIERRE_DIFERIDO { get; set; }
    }

    public class DINERO_EN_CAJA
    {
        public decimal MONTO_APERTURA_CAJA { get; set; }
        public decimal MONTO_COBRADO_CONTADO { get; set; }
        public decimal MONTO_COBRADO_CREDITO { get; set; }
        public decimal MONTO_CAJA_OTROS_INGRESO { get; set; }
        public decimal MONTO_CAJA_SALIDA { get; set; }
        public decimal MONTO_TOTAL { get; set; }
    }

    public class ARQUEO_CAJA: DINERO_EN_CAJA
    {
        public string NOM_CAJA { get; set; }
        public string SGN_MONEDA { get; set; }
        public string FECHA_APERTURA { get; set; }
        public string FECHA_CIERRE { get; set; }
        public string NOM_USUARIO { get; set; }
    }

    public class COMBOS_REPORTE_CAJA_ARQUEO
    {
        public List<USUARIO> listaUsuarios { get; set; }
        public List<CAJA> listaCajas{ get; set; }
    }
}
