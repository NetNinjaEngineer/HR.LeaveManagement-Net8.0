namespace HR.LeaveManagement.MVC.Services.Base
{
    public class Response<T>
    {
        public bool Success { get; set; }

        public string? Message { get; set; }

        public string? ValidationErrors { get; set; }

        public T Data { get; set; } = default!;
    }
}
