using DataAccessLayer.Contract;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryAbstraction
{
    public interface IGenericRepo<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity,Tkey> specification);
        public Task<IEnumerable<TEntity>> GetAllAsync();

        public Task<TEntity?> GetByIdAsync(Tkey id);

        public Task<TEntity?> GetByIdAsync(ISpecification<TEntity, Tkey> specification);


        public Task AddAsync(TEntity entity);

         public Task DeleteAsync(TEntity entity);

        public Task UpdateAsync( TEntity entity);

        public Task<int> CountAsync(ISpecification<TEntity,Tkey> specification);

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
