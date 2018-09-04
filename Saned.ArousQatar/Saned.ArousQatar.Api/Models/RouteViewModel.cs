namespace Saned.ArousQatar.Api.Models
{
    public class RouteViewModel
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 8;
        public string Filter { get; set; } = "";
        public int CategoryId { get; set; }
        public int? CategoryIdFilter { get; set; }
        public bool? IsPaidedFilter { get; set; }
        public string UserIdFilter { get; set; }
        public bool? IsExpired { get; set; }
    }
    public class PagingViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Filter { get; set; } = null;
        public string ApplicationUserId { get; set; } 
        public bool? isArchieve { get; set; } = null;
    }
    public class ViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Filter { get; set; } = null;
        public string ApplicationUserId { get; set; }
        public int Id { get; set; }
        public bool? isArchieve { get; set; } = null;
    }
}