using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public class Constantes
    {
        public const string sMensajeGrabadoOk = "Se grabaron correctamente los datos";
        public const string sMensajeEliminadoOk = "Se eliminaron correctamente los datos";
        public const string sMensajeErrorForm = "Ocurrió un error en la validación del formulario";
        public const string RequiredMensaje = "Debe de ingresar el/la {0}";
        public const string StringLengthMensaje = "El campo {0} debe tener una longitud mínima de {2} y una longitud máxima de {1}";
        public const string MaxLengthMensaje = "El campo {0} debe tener una longitud máxima de {1}";
        public const string RangeMensaje = "El rango del porcentaje del/la {0} debe ser entre {1} y {2}";
        public const string EmailAddressMensaje = "Formato mal ingresado del email";
        public const string sMensajeNohayRegistro = "No se encontraron datos";

        public const string INSERT_SUCCESS = "La información ha sido guardada correctamente.";
        public const string INSERT_ERROR = "No se pudo guardar la información.";
        public const string INSERT_PROBLEM = "Ocurrió un problema al guardar la información.";

        public const string UPDATE_SUCCESS = "La información ha sido actualizada correctamente.";
        public const string UPDATE_ERROR = "No se pudo actualizar la información.";
        public const string UPDATE_PROBLEM = "Ocurrió un problema al actualizar la información.";

        public const string DELETE_SUCCESS = "La información ha sido eliminada correctamente.";
        public const string DELETE_ERROR = "No se pudo eliminar la información.";
        public const string DELETE_PROBLEM = "Ocurrió un problema al eliminar la información.";

        public const string NOT_FOUND = "Información no encontrada";
    }
    public class Css
    {
        public static string PrintHori = "@page { size: A4 landscape; } @media print {body {-webkit-print-color-adjust: exact;}}";

        public static string PrintVert = "@page { size: A4 portrait; } @media print {body {-webkit-print-color-adjust: exact;}}";
    }
}
