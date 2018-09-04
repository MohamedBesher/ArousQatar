using System.Collections.Generic;

namespace Saned.ArousQatar.Data.Core.Models
{
    public sealed class Category : IEntityBase
    {
        public Category()
        {
            Advertisments = new List<Advertisment>();
            IsArchieved = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsArchieved { get; set; }
        public string IconName { get; set; }
        public ICollection<Advertisment> Advertisments { get; set; }
        public string ImageUrl { get; set; }

        public void Archieve()
        {
            IsArchieved = true;
        }

        public void Modify(string name, string iconName, string imageUrl)
        {
            Name = name;
            IconName = iconName;
            ImageUrl = imageUrl;
        }
    }
}
