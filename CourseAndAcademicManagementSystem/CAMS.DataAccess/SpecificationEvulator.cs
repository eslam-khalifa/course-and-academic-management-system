using DataAccessLayer.Contract;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class SpecificationEvulator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity,TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if(specification.IsPagingEnabled)
            {
                query = query.Skip(specification.PageIndex).Take(specification.PageSize);
            }

            if (specification.Includes != null && specification.Includes.Count>0)
            {
                foreach(var ex in specification.Includes)
                {
                    query = query.Include(ex);
                }
            }

            return query;
        }
    }
}
