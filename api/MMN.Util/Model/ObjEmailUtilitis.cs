using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MMN.Util.Model
{
    public class ObjEmailUtilitis
    {
        public ObjEmailUtilitis(DateTime data, string trigger, string body, string email, string nome, string subject, string message, string userName)
        {
            Data = data;
            Trigger = trigger;
            Body = body;
            Subject = subject;
            From = email;
            DestinationName = nome;
            Message = message;
            UserName = userName;
        }

        public ObjEmailUtilitis()
        {
            Cc = new List<string>();
        }

        //Data de ERRO
        public DateTime Data { get; set; }
        //Gatilho de quem esta solicitando
        public string Trigger { get; set; }
        //Titulo do e-mail
        public string Subject { get; set; }
        //Chave para validação
        public string Body { get; set; }
        //Mensagem Do corpo do e-mail
        public string Message { get; set; }
        //Para
        public string To { get; set; }
        //E-mail para quem irá enviar
        public string From { get; set; }
        //Nome de quem está enviando
        public string FromName { get; set; }
        //Nome de quem está mandando
        public string DestinationName { get; set; }
        public string UserName { get; set; }
        public string CaminhoAnexo { get; set; }
        public Attachment Anexo { get; set; }
        public string EmailSuporte { get; set; }
        public string SendGridClient { get; set; }
		public string SendPulseApiKey { get; set; }
        public string BrevoApiKey { get; set; }
		public List<string> Cc { get; set; }
    }

}
