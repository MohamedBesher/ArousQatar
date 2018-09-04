using Microsoft.AspNet.Identity;
using Saned.ArousQatar.Data.Core;
using Saned.ArousQatar.Data.Persistence.Tools;
using System.Threading.Tasks;

namespace Saned.ArousQatar.Data.Persistence.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return configSendGridasync(message);
        }
        private Task configSendGridasync(IdentityMessage message)
        {
            EmailManager mngMail = new EmailManager();
            string str = mngMail.SendActivationEmail(message.Subject, message.Destination, message.Body);
            //string path = System.Web.HttpContext.Current.Server.MapPath("/MyTest.txt");
            //if (!File.Exists(path))
            //{
            //    // Create a file to write to.
            //    using (StreamWriter sw = File.CreateText(path))
            //    {
            //        sw.WriteLine("Hello");
            //        sw.WriteLine(str);

            //    }
            //}

            // Open the file to read from.

            return Task.FromResult(0);
        }
    }
}