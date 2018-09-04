using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Saned.ArousQatar.Api.Utilities
{
    public static class Common
    {
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }
    }
}