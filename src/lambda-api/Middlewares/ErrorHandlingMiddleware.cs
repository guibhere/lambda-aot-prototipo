namespace Utils.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await Handler(context, ex);
            }
        }

        private async Task Handler(HttpContext context, Exception ex)
        {
            switch (ex)
            {
                case AddPessoaException:
                    context.Response.StatusCode = 400;
                    break;
                case ArgumentException:
                    context.Response.StatusCode = 501;
                    break;
                case Exception:
                    context.Response.StatusCode = 500;
                    break;
                default:
                    context.Response.StatusCode = 501;
                    break;
            }
            var response = new
            {
                codigo_erro = context.Response.StatusCode,
                correlation_id = context.Request.Headers is not null &&
                    context.Request.Headers["correlation_id"].ToString() is not null ?
                    context.Request.Headers["correlation_id"].ToString() : null,
                mensagem = ex.Message,
                excecao = ex.InnerException is not null ? ex.InnerException.Message : null
            };

            _logger.LogError("Ocorreu um erro: {@response} {@exception}", response, ex);
            await context.Response.WriteAsJsonAsync(response);
        }
    }


}
