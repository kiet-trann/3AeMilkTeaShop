namespace MilkTea.Core.Pagination
{
	public class PaginatingResult<T> 
	{
		public IEnumerable<T> Items { get; set; } 
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }

		public PaginatingResult(IEnumerable<T> items, int currentPage, int totalCount, int pageSize)
		{
			Items = items;
			CurrentPage = currentPage;
			TotalCount = totalCount;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
		}
	}
}
