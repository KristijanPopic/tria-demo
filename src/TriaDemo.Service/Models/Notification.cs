namespace TriaDemo.Service.Models;

public class Notification
{
    public Guid Id { get; set; }

    public required string Message { get; set; }

    public DateTime DateCreated { get; set; }
}