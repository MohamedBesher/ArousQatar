using System.Collections.Generic;

namespace Saned.ArousQatar.Data.Core.Models
{
    public class AdvertismentPrice : IEntityBase
    {
        public AdvertismentPrice()
        {
            IsArchieved = false;
            Advertisments = new List<Advertisment>();
        }
        public int Id { get; set; }

        public string Period { get; set; }

        public decimal Price { get; set; }

        public bool? IsArchieved { get; set; }

        public virtual ICollection<Advertisment> Advertisments { get; set; }

        public void Modify(string period, decimal price)
        {
            Period = period;
            Price = price;
        }

        void Archieve()
        {
            IsArchieved = true;
        }
    }
}
