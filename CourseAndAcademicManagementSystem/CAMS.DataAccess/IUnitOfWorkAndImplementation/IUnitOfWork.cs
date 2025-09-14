using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IUnitOfWorkAndImplementation
{
    public interface IUnitOFWork
    {
        IGenericRepo<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task SaveChangesAsync();
    }
}
