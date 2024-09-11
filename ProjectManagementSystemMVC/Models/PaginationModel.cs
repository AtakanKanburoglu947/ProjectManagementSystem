namespace ProjectManagementSystemMVC.Models
{
    public class PaginationModel<T,V> where T : class where V : class
    {
        public PaginationViewModel Pagination { get; set; }

        public V? Data { get; set; }
        public List<T>? Dataset { get; set; }
    }
}
