using System.Collections.Generic;
using System.Linq;

namespace Saned.ArousQatar.Api.Infrastructure.Core
{
    public class PaginationSet<T>
    {
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }

        public int Count => (this.Items != null) ? Items.Count() : 0;
        public int Page { get; set; }
    }
}