namespace MvsTaskOnlineShop.Utilities.Paginate
{
    public class Pagination<T>
    {
        public Pagination(List<T> datas, int currentPage, int totalPage, int count)
        {
            Datas = datas;
            CurrentPage = currentPage;
            TotalPage = totalPage;
            Count = count;
        }
        public List<T> Datas { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int Count { get; set; }
    }
}
