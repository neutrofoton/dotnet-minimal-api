using System.Net;

namespace Lab.MinimalApi.Dto;

public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public Object? Result { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
}
