namespace HR.LeaveManagement.Application.Responses
{
    public abstract class BaseCommandResponse
    {
        public int Id { get; set; }

        public string? Message { get; set; }

        public bool Succeeded { get; set; } = true;

        public List<string> Errors { get; set; } = [];
    }
}
