using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime.Internal.Util;
using DataBaseContext.Entidades;
using Microsoft.Extensions.Logging;
using Repository;

namespace Services
{
    public class Service : IService
    {
        private readonly IDynamoDBContext _dynamo;
        private readonly ILogger<Service> _logger;
        private readonly IRepository _repository;

        public Service(IDynamoDBContext dynamo, ILogger<Service> logger, IRepository repository)
        {
            _dynamo = dynamo;
            _logger = logger;
            _repository = repository;
        }

        public async Task AddPessoa(Person pessoa)
        {
            _logger.LogInformation("Iniciando Services");
            await _repository.AddPessoa(pessoa);
        }

        public async Task<List<Person>> GetAllPessoas()
        {
            _logger.LogInformation("Iniciando Services");
            return await _repository.GetAll();
        }

        public async Task<Person> GetPessoaName(string name)
        {
            _logger.LogInformation("Iniciando Services");
            return await _repository.GetByName(name);
        }

        public async Task<dynamic> putdynamo(FaturaModel fatura)
        {
            await _dynamo.SaveAsync(fatura);
            return "OK";
        }

        public async Task<string> Servico()
        {
            _logger.LogInformation("Iniciando Services");
            return "Esta é a resposta do serviço";
        }
    }
}