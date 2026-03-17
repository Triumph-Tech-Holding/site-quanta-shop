using MMN.Util.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MMN.Util.Util
{
    public class EmailUtilitis
    {
        /// <summary>
        /// Atributo para determinar o corpo de e-mail para
        /// os casos de Registro e Esquecimento de senha
        /// </summary>
        protected static string _tipoRequisicao;
        protected static string _emailFrom;

        public string TipoRequisicao
        {
            get { return _tipoRequisicao; }
            set { _tipoRequisicao = value; }
        }

        public string EmailFrom
        {
            get { return _emailFrom; }
            set { _emailFrom = value; }
        }

        /// <summary>
        /// Método genérico de preparação de E-mail
        /// Não recebe parâmetros
        /// </summary>
        /// <returns>Caso ocorra tudo certo irá retornar as configurações de SMTP</returns>
        /// <returns>Caso algo dê errado irá salvar no log de exceções</returns>
        public SmtpClient PreparaSmtp()
        {
            try
            {

                var smtp = new SmtpClient();
                var credencial = new NetworkCredential();

                smtp.Host = "smtp.mailtrap.io";
                smtp.Port = 2525;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                credencial.UserName = "3e198b76205d5e";
                credencial.Password = "01e96b04c0286a";
                smtp.Credentials = credencial;

                return smtp;
            }
            catch (Exception ex)
            {
                LogHelper.LogException("Método PreparaSMTP", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">Carregado a partir da IdentityConfig</param>
        /// <returns></returns>
        ///                                     message.Destination,     
        public MailMessage PreparaEmail(ObjEmailUtilitis obj)
        {

            var email = new MailMessage
            {
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                //SubjectEncoding = Encoding.GetEncoding(0),
                From = new MailAddress(obj.From),
                Body = obj.Body,
                Subject = obj.Subject,
                To = { obj.To }
            };

            if (obj.Anexo != null)
            {
                email.Attachments.Add(obj.Anexo);
            }

            if (new EmailUtilitis().ValidarRegEx(obj.From))
                email.To.Add(new MailAddress(obj.From));

            return email;
            //var okEnvio = new EmailService().EnviarEmail(email, smtp);
        }

        /// <summary>
        /// Método responsável pelo envio do E-mail de fato.
        /// </summary>
        /// <param name="corpoEmail"></param>
        /// <param name="smtp"></param>
        /// <returns>Caso ocorra algum erro irá salvar no Log.Trace</returns>
        public bool EnviarEmail(MailMessage corpoEmail, SmtpClient smtp)
        {
            try
            {
                smtp.Send(corpoEmail);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex);
                return false;
            }
        }

        /// <summary>
        /// Metodo responsavel por validar um e-mail
        /// </summary>
        /// <param name="email">string contendo o email</param> 
        /// <returns>O método retorna verdadeiro ou falso.</returns>
        /// <remarks>Deve ser implementado dentro da classe derivada contendo as noticias.</remarks>
        public bool ValidarRegEx(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException ex)
            {
                LogHelper.LogException("ValidarRegEx", ex);
                return false;
            }
        }

        /// <summary>
        /// Método responsável por carregar os templates informados em "templateBody" e "templatePai"
        /// Após isso o sistema irá identificar todos os valores que devem ser substituidos, na variável listaChaves
        /// e substituir por seus devidos valores.
        /// Após o tratamento dos dados o sistema irá enviar o e-mail ao destinatário.
        /// </summary>
        /// <param name="listaChaves"></param>
        /// <param name="caminhoTemplateBody"></param>
        /// <param name="caminhoTemplatePai"></param>
        /// <param name="objEmailService"></param>
        /// <returns></returns>
        public async Task<bool> EnviarEmail(Dictionary<string, string> listaChaves, string caminhoTemplateBody, string caminhoTemplatePai, ObjEmailUtilitis objEmailService)
        {
            try
            {
                var pathTemplateEmailBody = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, caminhoTemplateBody);
                string pathTemplateEmail = null;
                
                if (!string.IsNullOrEmpty(caminhoTemplatePai))
                    pathTemplateEmail = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, caminhoTemplatePai);

                string templateString = File.ReadAllText(pathTemplateEmailBody);

                foreach (var chave in listaChaves)
                {
                    templateString = templateString.Replace(chave.Key, chave.Value);
                }

                if (!string.IsNullOrEmpty(pathTemplateEmail))
                    objEmailService.Body = File.ReadAllText(pathTemplateEmail)
                        .Replace("#BODY#", templateString)
                        .Replace("#EMAIL_FOOTER_CONTATO#", objEmailService.EmailSuporte);
                else
                    objEmailService.Body = templateString;

                //await EnviarSendGrid(objEmailService);
                await EnviarBrevo(objEmailService);

                return true;
            }
            catch (Exception e)
            {
                LogHelper.LogException("Envio de E-mail - EmailUtilitis.cs", e);
                return false;
            }
        }

        public async Task EnviarSendGrid(ObjEmailUtilitis objEmailService)
        {
            try
            {
                var client = new SendGridClient(objEmailService.SendGridClient);
                var from = new EmailAddress(objEmailService.From, objEmailService.FromName);
                var subject = objEmailService.Subject;
                var to = new EmailAddress(objEmailService.To, objEmailService.DestinationName);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", objEmailService.Body);

                foreach (var item in objEmailService.Cc)
                    msg.AddBcc(new EmailAddress(item, null));

                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                LogHelper.LogException("EnviarSendGrid(ObjEmailUtilitis objEmailService)", ex, "EmailUtilitis");
                throw;
            }
        }

        public async Task EnviarSendPulse(ObjEmailUtilitis message)
        {
			
		}

        public async Task EnviarBrevo(ObjEmailUtilitis message)
        {  
            const string url = "https://api.brevo.com/v3/smtp/email";

            using var httpClient = new HttpClient();

            message.BrevoApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY");

            httpClient.DefaultRequestHeaders.Add("accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("api-key", message.BrevoApiKey);
            
            var payload = new
            {
                sender = new { name = message.FromName, email = message.From },
                to = new[] { new { email = message.To, name = message.DestinationName } },
                subject = message.Subject,
                htmlContent = message.Body
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao enviar e-mail via Brevo: {response.StatusCode} - {responseContent}");
			}
		}
	}
}
