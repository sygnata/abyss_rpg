using AbyssRpg.Application.Common.Exceptions;
using AbyssRpg.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AbyssRpg.Api.ExceptionHandlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandler> _logger;

	public GlobalExceptionHandler(
		ILogger<GlobalExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		ProblemDetails problemDetails;

		switch (exception)
		{
			case NotFoundException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status404NotFound,
					Title = "Resource not found",
					Detail = exception.Message,
					Instance = httpContext.Request.Path
				};
				break;

			case ConflictException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status409Conflict,
					Title = "Resource conflict",
					Detail = exception.Message,
					Instance = httpContext.Request.Path
				};
				break;

			case DomainException:
				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status400BadRequest,
					Title = "Domain validation error",
					Detail = exception.Message,
					Instance = httpContext.Request.Path
				};
				break;

			default:
				_logger.LogError(
					exception,
					"Unexpected error processing {Method} {Path}",
					httpContext.Request.Method,
					httpContext.Request.Path
				);

				problemDetails = new ProblemDetails
				{
					Status = StatusCodes.Status500InternalServerError,
					Title = "Internal server error",
					Detail = "Ocorreu um erro inesperado.",
					Instance = httpContext.Request.Path
				};
				break;
		}

		httpContext.Response.StatusCode =
			problemDetails.Status
			?? StatusCodes.Status500InternalServerError;

		await httpContext.Response.WriteAsJsonAsync(
			problemDetails,
			cancellationToken
		);

		return true;
	}
}