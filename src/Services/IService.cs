using DataBaseContext.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IService
    {
        Task<string> Servico();
        Task<dynamic> putdynamo(FaturaModel fatura);
        Task AddPessoa(Person pessoa);
        Task<List<Person>> GetAllPessoas();
        Task<Person> GetPessoaName(string name);
    }
}
