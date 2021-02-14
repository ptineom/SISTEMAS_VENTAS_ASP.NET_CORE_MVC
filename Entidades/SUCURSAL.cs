using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class SUCURSAL
	{
		public string ACCION { get; set; }
		public string ID_SUCURSAL { get; set; }
		public string NOM_SUCURSAL { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public string ID_EMPRESA { get; set; }
		public string DIRECCION { get; set; }
		public string ID_UBIGEO { get; set; }
		public bool FLG_PRINCIPAL { get; set; }
		public string EMAIL { get; set; }
        public bool FLG_INICIAR_FACTURACION_ELECTRONICA { get; set; }
        //
        public string NOM_UBIGEO { get; set; }
        public bool FLG_INACTIVO { get; set; }
        public bool FLG_SELECCION { get; set; }
        public string ID_ALMACEN { get; set; }
        public string JSON_TELEFONOS { get; set; }
        public string TELEFONOS { get; set; }
        public string NOM_ALMACEN { get; set; }
        public bool FLG_EN_USO { get; set; }
        public decimal STOCK_ACTUAL { get; set; }
    }
}
