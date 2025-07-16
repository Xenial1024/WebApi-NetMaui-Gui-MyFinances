using System.Collections.Generic;

namespace MyFinances.Core.Response
{
    public class PagedResponse<T> : Response
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; } //całkowita liczba operacji
    }
}
