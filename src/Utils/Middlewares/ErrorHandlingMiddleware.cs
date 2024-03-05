namespace Utils.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate Next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await Next(context);
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

            await context.Response.WriteAsJsonAsync(response);
        }
    }


}
