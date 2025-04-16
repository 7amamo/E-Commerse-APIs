using Core.Contracts;
using Core.Entities;
using Core.Specifications;
using InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Without Specification

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
             => await _dbContext.Set<T>().FindAsync(id);

        #endregion


        #region With Specification

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(Specifications<T> spec)
        {
            return await ApplySpecificatiobs(spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecAsync(Specifications<T> spec)
        {
            return await ApplySpecificatiobs(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecificatiobs(Specifications<T> spec)  // to minimizw code and Repetition
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpec(Specifications<T> spec)
        {
            return await ApplySpecificatiobs(spec).CountAsync();
        }

        #endregion

        public async Task AddAsync(T Item)
        {
            await _dbContext.Set<T>().AddAsync(Item);
        }

        public void Update(T Item)
        {
            _dbContext.Set<T>().Update(Item);

        }

        public void Delete(T Item)
        {
            _dbContext.Set<T>().Remove(Item);

        }
    }
}
