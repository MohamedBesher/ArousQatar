using System;

namespace Saned.ArousQatar.Data.Core.Models
{
    public sealed class AdvertismentTransaction : IEntityBase
    {
        public AdvertismentTransaction()
        {
            CreateDate = DateTime.Now;

        }
        public int Id { get; set; }

        public string  PaymentId { get; set; }

        public DateTime CreateDate { get; private set; }
        public int AdvertismentId { get;  set; }
        public Advertisment Advertisment { get;  set; }




    }
}