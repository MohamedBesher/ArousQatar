using System;
using System.Net.Mail;

namespace UtiltyManagemnt
{
    // this is the complete class for sending mails
    // u must put all the attributs in the webconfig 
    // and call them in ur code

    public class Mail
    {

        public Boolean SendEMail(MailData mailData)
        {
            // Configure mail client (may need additional
            // code for authenticated SMTP servers)
            SmtpClient mailClient = new SmtpClient(mailData.Host);
            // Create the mail message
            MailMessage mailMessage = new MailMessage(mailData.FromEmail, mailData.ToEmail, mailData.Subject, mailData.MailContent);
            if (!string.IsNullOrEmpty(mailData.Bcc) && mailData.Bcc.Length > 0)
                mailMessage.Bcc.Add(mailData.Bcc);
            mailMessage.IsBodyHtml = true;
            try
            {
                mailClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
    public class MailData
    {
        public string ToEmail
        {
            get;
            set;
        }
        public string FromEmail
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string MailContent
        {
            get;
            set;
        }
        public string Bcc
        {
            get;
            set;
        }
        public string Host
        {
            get;
            set;
        }
    }

}