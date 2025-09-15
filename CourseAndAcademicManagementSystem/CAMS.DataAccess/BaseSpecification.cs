using DataAccessLayer.Contract;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
     public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
     {
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }


        public BaseSpecification(Expression<Func<TEntity,bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }

      


        public Expression<Func<TEntity, object>> OrderBy {get; private set; } 

        public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public Expression<Func<TEntity, object>> OrderByDescending {get; private set; }

       

        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        public int PageSize { get; set; } 

        public int PageIndex{get ; set;}

        public bool IsPagingEnabled {get; set;}

        public void ApplyPaging(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = (pageIndex-1)*PageSize;
            IsPagingEnabled = true;
        }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = [];

       

        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

    }
}
