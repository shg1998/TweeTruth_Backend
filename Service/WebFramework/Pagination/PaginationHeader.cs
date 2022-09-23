using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WebFramework.Pagination
{
    public class PaginationHeader
    {
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }

        public PaginationHeader(int totalPages, int itemsPerPage, int currentpage, int totalItems)
        {
            this.TotalPages = totalPages;
            this.TotalItems = totalItems;
            this.ItemsPerPage = itemsPerPage;
            this.CurrentPage = currentpage;

        }
    }
}
