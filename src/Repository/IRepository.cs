using DataBaseContext.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository
    {
        Task AddPessoa(Person pessoa);
        Task<List<Person>> GetAll();
        Task<Person> GetByName(string name);
    }
}
