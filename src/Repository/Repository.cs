using DataBaseContext;
using DataBaseContext.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;
using Utils;

namespace Repository
{
    public class Repository : IRepository
    {
        private readonly DataContext _dbContext;
        private readonly ILogger<Repository> _logger;

        public Repository(ILogger<Repository> logger, DataContext dataContext)
        {
            _logger = logger;
            _dbContext = dataContext;
        }

        public async Task AddPessoa(Person pessoa)
        {
            _logger.LogInformation("Persistindo pessoa : {@pessoa}", pessoa);
            try
            {
                await _dbContext.Persons.AddAsync(pessoa);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new AddPessoaException("Ocorreu um erro ao cadastrar a pessoa",ex);
            }
        }

        public async Task<List<Person>> GetAll()
        {
            _logger.LogInformation("Consultando pessoas");
            return await _dbContext.Persons.ToListAsync();
        }

        public async Task<Person> GetByName(string name)
        {
            _logger.LogInformation("Consultando nome pessoas");
            return await _dbContext.Persons.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}