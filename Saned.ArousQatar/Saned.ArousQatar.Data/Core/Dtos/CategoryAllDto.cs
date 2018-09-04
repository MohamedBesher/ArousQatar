using Microsoft.AspNet.Identity;

namespace Saned.ArousQatar.Data.Core.Dtos
{
   
    public class CategoryAllDto
    {
   
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsArchieved { get; set; }
        public string IconName { get; set; }
        public string ImageUrl { get; set; }
        public int OverAllCount { get; set; }
    }
}