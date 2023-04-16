namespace Api.Responses;

public class ListItemsResponse<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
}