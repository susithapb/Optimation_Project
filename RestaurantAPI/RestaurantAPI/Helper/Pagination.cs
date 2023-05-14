using RestaurantAPI.Models;

namespace RestaurantAPI.Helper
{
    public class Pagination
    {
        public Pagination()
        {
            this.PageIndex = 1;
            this.PageSize = 10;
        }
        public Pagination(int pageNumber, int pageSize)
        {
            this.PageIndex = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<Dish> Data { get; set; }
    }
}
