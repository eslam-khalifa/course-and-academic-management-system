using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryAbstraction;
using DataAccessLayer.RepositoryImplementation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.IUnitOfWorkAndImplementation
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly LearningPlatformDbContext _dbContext;
        private readonly Hashtable _Repository;

        public UnitOfWork(LearningPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
            _Repository = new Hashtable();
        }
        public IGenericRepo<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
           var type = typeof(TEntity).Name;
            if (!_Repository.ContainsKey(type))
            {
                var Repository = new GenericRepo<TEntity, TKey>(_dbContext);
                _Repository.Add(type, Repository);
            }

            return _Repository[type] as IGenericRepo<TEntity, TKey>;
        }

        public async Task SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();     
        }
    }
}
