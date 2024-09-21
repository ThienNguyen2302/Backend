using Newtonsoft.Json;
using System.Net;

namespace Udemy.Middlewares;

public class ExceptionMiddleware
{
	private readonly RequestDelegate _next;
	public ExceptionMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Exception: {ex.Message} - Function: {context.Request.Path.Value ?? ""}");
			await HandleExceptionAsync(context, ex);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.OK;

		var response = Result<string>.Failure(exception.Message);
		var jsonResponse = JsonConvert.SerializeObject(response);
		jsonResponse = jsonResponse.Replace("Succeeded", "succeeded");
		jsonResponse = jsonResponse.Replace("ErrorMessage", "errorMessage");
		jsonResponse = jsonResponse.Replace("Data", "data");
		return context.Response.WriteAsync(jsonResponse);
	}
}
