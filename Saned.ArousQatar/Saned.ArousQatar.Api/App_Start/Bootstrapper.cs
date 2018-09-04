using System.Web.Http;

namespace Saned.ArousQatar.Api
{
    public class Bootstrapper
    {
        public static void Run()
        {
            //Configure Autofac
            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);

            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
    }
}