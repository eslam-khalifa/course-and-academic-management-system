using DataAccessLayer.Contract;
using DataAccessLayer.DbContexts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryAbstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.RepositoryImplementation
{
    public class GenericRepo<TEntity, TKey> : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly LearningPlatformDbContext _dbContext;

        public GenericRepo(LearningPlatformDbContext dbContext)
        {
             _dbContext = dbContext;
        }



        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
           
        }

       

        public async Task DeleteAsync(TEntity entity)
        {
           _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
          return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvulator.CreateQuery(_dbContext.Set<TEntity>(), specification).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvulator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync( TEntity entity)
        {
           _dbContext.Set<TEntity>().Update(entity);  
        }

        public Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
        {
            return SpecificationEvulator.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();    
        }
    }
}
