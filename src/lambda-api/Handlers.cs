using Amazon.CloudWatchLogs;
using Amazon.Lambda.APIGatewayEvents;
using DataBaseContext;
using DataBaseContext.Entidades;
using lambda_api.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using lambda_api.Extensions;

namespace lambda_api.Handlers
{

    static class Handlers
    {
        internal static ILogger _logger;
        internal static IService _services;

        public static async Task<string> teste()
        {
            _logger.LogInformation("Hello!");
            return "hello";
        }

        public static async Task<string> testepost(testemodel input)
        {
            _logger.LogInformation("Payload : {@payload}", input);
            return await _services.Servico();
        }

        public static async Task<dynamic> testepostfrombody([FromBody] testemodel input, [FromQuery] int? q1, [FromQuery] string q2, long param)
        {
            try
            {
                _logger.LogInformation("Payload : {@payload}", input);
                _logger.LogInformation("Query 1: {@id}", q1);
                _logger.LogInformation("Query 2: {@id}", q2);
                _logger.LogInformation("Parameter: {@param}", param);
                var result = await _services.Servico();

                return new
                {
                    input = input,
                    q1 = q1,
                    q2 = q2,
                    param = param
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ex : {@exception}", ex);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = ex.Message
                };
            }
        }

        public static async Task<APIGatewayProxyResponse> addfatura([FromBody] FaturaModel input)
        {
            try
            {
                _logger.LogInformation("Payload : {@payload}", input);

                var result = await _services.putdynamo(input);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ex : {@exception}", ex);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = ex.Message
                };
            }
        }

        public static async Task<dynamic> testedelegate([FromServices] IService _service, [FromBody] testemodel input, [FromQuery] int? q1, [FromQuery] string q2, long param)
        {
            try
            {
                _logger.LogInformation("Payload : {@payload}", input);
                _logger.LogInformation("Query 1: {@id}", q1);
                _logger.LogInformation("Query 2: {@id}", q2);
                _logger.LogInformation("Parameter: {@param}", param);
                var result = await _service.Servico();

                return new
                {
                    input = input,
                    q1 = q1,
                    q2 = q2,
                    param = param
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Ex : {@exception}", ex);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = ex.Message
                };
            }
        }

        public static async Task<ActionResult<dynamic>> AddPessoa([FromServices] IService _service, [FromBody] Person input)
        {
            _logger.LogInformation("Add pessoa: {@payload}", input);
            await _service.AddPessoa(input);
            return ("Ok");
        }
        public static async Task<ActionResult<dynamic>> GetAllPessoas([FromServices] IService _service, [FromQuery] string? name)
        {
            _logger.LogInformation("Get pessoas");
            if (string.IsNullOrEmpty(name))
                return await _service.GetAllPessoas();
            return await _service.GetPessoaName(name);
        }

        public static async Task<IResult> PutPessoasAsync(
            [FromServices] DataContext _dataContext,
            [FromBody] Person input,
            int id_pessoa)
        {
            var pessoa = await _dataContext.Persons.FindAsync(id_pessoa);

            if (pessoa == null)
                throw new Exception("Pessoa não existe na base");

            pessoa.UpdateProperties(input);
            await _dataContext.SaveChangesAsync();

            return new CustomResult(200, pessoa);
        }

        public static IResult PutPessoas(
            [FromServices] DataContext _dataContext,
            [FromBody] Person input,
            int id_pessoa)
        {
            var pessoa = _dataContext.Persons.Find(id_pessoa);

            if (pessoa == null)
                throw new Exception("Pessoa não existe na base");

            pessoa.UpdateProperties(input);
            _dataContext.SaveChanges();

            return new CustomResult(200, pessoa);
        }

        public async static Task<IResult> PutPessoasAnyAsync(
            [FromServices] DataContext _dataContext,
            [FromBody] Person input,
            int id_pessoa)
        {
            input.Id = id_pessoa;
            if (!await _dataContext.Persons.AnyAsync(x => x.Id == id_pessoa))
                throw new Exception("Pessoa não existe na base");

            _dataContext.Persons.Update(input);

            await _dataContext.SaveChangesAsync();
            return new CustomResult(200, $"Pessoa {id_pessoa} atualizada");

        }

        public static IResult PutPessoasAny(
            [FromServices] DataContext _dataContext,
            [FromBody] Person input,
            int id_pessoa)
        {
            input.Id = id_pessoa;

            if (!_dataContext.Persons.Any(x => x.Id == id_pessoa))
                throw new Exception("Pessoa não existe na base");

            _dataContext.Persons.Update(input);
            int linhas = _dataContext.SaveChanges();
            return new CustomResult(200, $"Pessoa {id_pessoa} atualizada");


        }

        public static async Task NotFound(HttpContext context)
        {
            var response = new
            {
                codigo_erro = context.Response.StatusCode,
                correlation_id = context.Request.Headers is not null &&
                    context.Request.Headers["correlation_id"].ToString() is not null ?
                    context.Request.Headers["correlation_id"].ToString() : null,
                mensagem = $"Rota {context.Request.Method.ToString()} {context.Request.Path.Value} não existe"
            };
            await Results.NotFound(response).ExecuteAsync(context);
        }

    }
}