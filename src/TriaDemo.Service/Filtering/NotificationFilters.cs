namespace TriaDemo.Service.Filtering;

public sealed class NotificationFilters
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsRead { get; set; }
}
