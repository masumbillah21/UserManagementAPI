namespace UserManagementAPI.Responses
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public object? Data { get; set; }
    }
}
