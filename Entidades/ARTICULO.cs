using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class ARTICULO
	{
		public string ACCION { get; set; }
		public string ID_ARTICULO { get; set; }
		public string NOM_ARTICULO { get; set; }
		public string NOM_VENTA { get; set; }
		public bool FLG_IMPORTADO { get; set; }
		public int ID_MARCA { get; set; }
		public decimal PRECIO_COMPRA { get; set; }
		public bool FLG_INACTIVO { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public string ID_LINEA { get; set; }
		public string ID_GRUPO { get; set; }
		public string ID_FAMILIA { get; set; }
		public string CODIGO_BARRA { get; set; }
        public decimal PRECIO_VENTA_FINAL { get; set; }
        public decimal PRECIO_VENTA { get; set; }
        public decimal PRECIO_BASE { get; set; }
        //
        public string ID_SUCURSAL { get; set; }
        public decimal STOCK_ACTUAL { get; set; }
        public decimal STOCK_MINIMO { get; set; }
        public string ID_UM { get; set; }
        public string NOM_UM { get; set; }
        public decimal NRO_FACTOR { get; set; }
        public string NOM_MARCA { get; set; }
        public string NOM_GRUPO { get; set; }
        public string NOM_FAMILIA { get; set; }
        public string FOTO1 { get; set; }
        public string FOTO2 { get; set; }
        public string FOTO_B64 { get; set; }
        public string JSON_UM { get; set; }
        public decimal DESCUENTO1 { get; set; }
        public string JSON_SUCURSAL { get; set; }
        public string NOM_FOTO { get; set; }
        //
        public List<ARTICULO_UM> listaArticuloUm { get; set; }
        public List<SUCURSAL> listaSucursal { get; set; }
        public string SGN_MONEDA { get; set; }

        public byte[] BARCODE { get; set; }
	}
}
