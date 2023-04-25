namespace Neo.Specs.ScreenPlay.Notifications.Questions;

public class RequestStatusResponse
{
    public string Id { get; set; }
    public Guid? EntityId { get; set; }
    public bool Completed { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public bool Faulted => !string.IsNullOrEmpty(ErrorCode) ||
                           !string.IsNullOrEmpty(ErrorMessage);
}