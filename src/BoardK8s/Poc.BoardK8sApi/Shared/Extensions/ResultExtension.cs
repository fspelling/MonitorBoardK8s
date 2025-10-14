using FluentResults;

namespace Poc.BoardK8sApi.Shared.Extensions
{
    public static class ResultExtension
    {
        public static IResult ToResultCustom<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return Results.Ok(result);

            return Results.BadRequest(result.Errors);
        }
    }
}
