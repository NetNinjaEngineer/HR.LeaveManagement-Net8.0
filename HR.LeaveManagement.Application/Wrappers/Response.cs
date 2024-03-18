namespace HR.LeaveManagement.Application.Wrappers
{
    public class Response<T> where T : new()
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; } = null!;

        public T Data { get; set; } = default!;

        public Response(T data, string? message = null)
        {
            Data = data;
            Succeeded = true;
            Message = message!;
        }

        public Response(string message)
        {
            Message = message;
            Succeeded = false;
        }
    }
}
