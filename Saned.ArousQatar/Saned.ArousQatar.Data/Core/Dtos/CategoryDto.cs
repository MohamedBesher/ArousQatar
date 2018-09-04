namespace Saned.ArousQatar.Data.Core.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }

    }

    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int OverallCount { get; set; }
    }
}