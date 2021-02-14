using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
namespace Helper
{
    //Si tienes problemas para que se envie el correo, te sugiero actives permiso a terceras aplicaciones en los dos siguientes links:
    // https://myaccount.google.com/lesssecureapps?utm_source=google-account&utm_medium=web

    public class EmailHelper
    {
        private string _destino;
        private string _cuerpo;
        private string _asunto;
        private string[] _arrStringAdjuntos;
        private List<ArchivosAdjuntos> _archivosAdjuntos;
        private string _cc_destinatarios;
        private string _cco_destinatarios;


        private string _origenUserName;
        private string _origenPassword;

        private string _error;
        public string error
        {
            get
            {
                return _error;
            }
        }

        public EmailHelper(string destino, string cuerpo, string asunto, string cc_destinatarios = "", string cco_destinatarios = "")
        {
            _destino = destino;
            _cuerpo = cuerpo;
            _asunto = asunto;
            _cc_destinatarios = cc_destinatarios;
            _cco_destinatarios = cco_destinatarios;
            _error = string.Empty;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(path, false);

            _origenUserName = configBuilder.Build().GetSection("Email:OrigenUserName").Value;
            _origenPassword = configBuilder.Build().GetSection("Email:OrigenPassword").Value;
        }
        public EmailHelper(string destino, string cuerpo, string asunto, string[] adjuntos,
          string cc_destinatarios = "", string cco_destinatarios = "")
        {
            _destino = destino;
            _cuerpo = cuerpo;
            _asunto = asunto;
            _arrStringAdjuntos = adjuntos;
            _cc_destinatarios = cc_destinatarios;
            _cco_destinatarios = cco_destinatarios;
            _error = string.Empty;
        }

        public EmailHelper(string destino, string cuerpo, string asunto, List<ArchivosAdjuntos> archivosAdjuntos,
            string cc_destinatarios = "", string cco_destinatarios = "")
        {
            _destino = destino;
            _cuerpo = cuerpo;
            _asunto = asunto;
            _archivosAdjuntos = archivosAdjuntos;
            _cc_destinatarios = cc_destinatarios;
            _cco_destinatarios = cco_destinatarios;
            _error = string.Empty;
        }

        // envio de correo
        public bool sendMail()
        {
            bool bEnvioExitoso = false;
            string mensaje = string.Empty, email = string.Empty;

            //Validar todos los parametros ingresados.
            if (!validarParametros(ref mensaje))
            {
                _error = mensaje;
                return false;
            }
            MemoryStream ms = null;
            try
            {
                SmtpClient mySmtpClient;
                using (MailMessage myMessage = new MailMessage())
                {
                    //Lista de destinatarios.
                    if (!string.IsNullOrEmpty(_destino))
                    {
                        string[] ListaDestinatarios = _destino.Split(';');
                        if (ListaDestinatarios.Length > 0)
                        {
                            for (int i = 0; i < ListaDestinatarios.Length; i++)
                            {
                                email = string.Empty;
                                email = ListaDestinatarios[i].Trim();
                                if (!validarFormatoEmail(email))
                                {
                                    mensaje = "Formato mal ingresado en el email de destino";
                                    break;
                                }
                                myMessage.To.Add(email);
                            }
                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                throw new System.ArgumentException(mensaje);
                            }
                        }
                    }
                    mensaje = string.Empty;
                    //Lista de destinatarios como copias.
                    if (!string.IsNullOrEmpty(_cc_destinatarios))
                    {
                        string[] ListaCc_Destinatarios = _cc_destinatarios.Split(';');
                        if (ListaCc_Destinatarios.Length > 0)
                        {
                            for (int i = 0; i < ListaCc_Destinatarios.Length; i++)
                            {
                                email = string.Empty;
                                email = ListaCc_Destinatarios[i].Trim();
                                if (!validarFormatoEmail(email))
                                {
                                    mensaje = "Formato mal ingresado en el email de copias destinatarios";
                                    break;
                                }
                                myMessage.CC.Add(email);
                            };
                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                throw new System.ArgumentException(mensaje);
                            }
                        }
                    }
                    //Lista de destinatario como oculto.
                    if (!string.IsNullOrEmpty(_cco_destinatarios))
                    {
                        string[] ListaCco_Destinatarios = _cco_destinatarios.Split(';');
                        if (ListaCco_Destinatarios.Length > 0)
                        {
                            for (int i = 0; i < ListaCco_Destinatarios.Length; i++)
                            {
                                email = string.Empty;
                                email = ListaCco_Destinatarios[i].Trim();
                                if (!validarFormatoEmail(email))
                                {
                                    mensaje = "Formato mal ingresado en el email de destinatarios ocultos";
                                    break;
                                }
                                myMessage.CC.Add(email);
                            };
                            if (!string.IsNullOrEmpty(mensaje))
                            {
                                throw new System.ArgumentException(mensaje);
                            }
                        }
                    }
                    //Lista de direcciones absolutas de archivos adjuntos.
                    if (_arrStringAdjuntos != null)
                    {
                        // Agregado de archivo
                        foreach (string archivo in _arrStringAdjuntos)
                        {
                            // Comprobamos si existe el archivo y lo agregamos a los adjuntos
                            if (System.IO.File.Exists(@archivo))
                                myMessage.Attachments.Add(new Attachment(@archivo));

                        }
                    }
                    if (_archivosAdjuntos != null)
                    {
                        // Agregado de archivo
                        foreach (ArchivosAdjuntos archivo in _archivosAdjuntos)
                        {
                            if (archivo.buffer.Length > 0 && !string.IsNullOrEmpty(archivo.fileName) && !string.IsNullOrEmpty(archivo.mediaType))
                            {
                                ms = new MemoryStream(archivo.buffer);
                                Attachment attachment = new Attachment(ms, archivo.fileName, archivo.mediaType);
                                myMessage.Attachments.Add(attachment);
                                ms.Position = 0;
                            }
                        }
                    }

                    // Evaluar el tipo de email.
                    string sTipoEmail = _origenUserName.Substring(_origenUserName.LastIndexOf("@")).ToLower();
                    int nPuerto = 0;
                    string sSmtp = "";
                    if (sTipoEmail == "@gmail.com")
                    {
                        nPuerto = 587;
                        sSmtp = "smtp.gmail.com";
                    }
                    else if (sTipoEmail == "@hotmail.com" || sTipoEmail == "@outlook.com")
                    {
                        nPuerto = 465;
                        sSmtp = "smtp.live.com";
                    }
                    //
                    myMessage.From = new MailAddress(_origenUserName);
                    myMessage.Subject = _asunto;
                    myMessage.Body = _cuerpo;
                    myMessage.IsBodyHtml = true;

                    mySmtpClient = new SmtpClient(sSmtp);
                    mySmtpClient.Port = nPuerto;
                    mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mySmtpClient.UseDefaultCredentials = false;
                    mySmtpClient.EnableSsl = true;
                    // Especificar Cuenta de correo y contraseña
                    mySmtpClient.Credentials = new NetworkCredential(_origenUserName, _origenPassword);
                    // Enviamos el mail
                    mySmtpClient.Send(myMessage);
                    bEnvioExitoso = true;
                    mySmtpClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                bEnvioExitoso = false;
                Helper.Elog.save(this, ex);
            }
            finally
            {
                if (ms != null) ms.Dispose();
            }
            return bEnvioExitoso;

        }

        private bool validarParametros(ref string mensaje)
        {
            if (string.IsNullOrEmpty(_origenUserName) || string.IsNullOrEmpty(_origenPassword))
            {
                mensaje = "Existe un problema con el email del sistema, comunicarse con el administrador.";
                return false;
            }
            if (!validarFormatoEmail(_origenUserName))
            {
                mensaje = "Formato mal ingresado del email de sistema";
                return false;
            }
            if (string.IsNullOrEmpty(_asunto))
            {
                mensaje = "Debe de ingresar el asunto";
                return false;
            }
            if (string.IsNullOrEmpty(_cuerpo))
            {
                mensaje = "Debe de ingresar el cuerpo";
                return false;
            }
            mensaje = string.Empty;
            return true;
        }

        private bool validarFormatoEmail(string email)
        {
            string sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, sFormato))
            {
                if (Regex.Replace(email, sFormato, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
    public class ArchivosAdjuntos
    {
        public byte[] buffer { get; set; }
        public string fileName { get; set; }
        public string mediaType { get; set; }
    }
}
