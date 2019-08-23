using System.Collections;
using System.Collections.Generic;

namespace AnvilBank.Web.Infrastructure.Collections
{
    public class PaginatedList<T> : IPaginatedList, IEnumerable<T>
    {
        private readonly IEnumerable<T> data;

        public PaginatedList(
            IEnumerable<T> data,
            int pageIndex,
            int totalPages,
            int surroundingPageCount)
        {
            this.data = data;
            this.PageIndex = pageIndex;
            this.TotalPages = totalPages;
            this.SurroundingPagesCount = surroundingPageCount;
        }

        public IEnumerator<T> GetEnumerator() => this.data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int PageIndex { get; }

        public int TotalPages { get; }

        public int SurroundingPagesCount { get; }

        public bool HasPreviousPage => this.PageIndex > 1;

        public bool HasNextPage => this.PageIndex < this.TotalPages;
    }
}
