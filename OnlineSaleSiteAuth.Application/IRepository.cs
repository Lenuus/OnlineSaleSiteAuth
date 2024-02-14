using OnlineSaleSiteAuth.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application
{
    public interface IRepository<Table> where Table : IBaseEntity
    {
        IQueryable<Table> GetAll();

        Task<Table> GetById(Guid id);

        Task DeleteById(Guid id);

        Task Delete(Table entity);

        Task Update(Table entity);

        Task Create(Table entity);

    }
}
