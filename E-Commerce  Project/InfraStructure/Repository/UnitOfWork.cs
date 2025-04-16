using Core.Contracts;
using Core.Entities;
using InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositoires;

        public UnitOfWork(StoreContext dbContext)
        {
            _repositoires = new Hashtable();
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
        => _dbContext.DisposeAsync();


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositoires.ContainsKey(type)) //first time
            {
                var Repository = new GenericRepository<TEntity>(_dbContext);
                _repositoires.Add(type, Repository);
            }
            return _repositoires[type] as IGenericRepository<TEntity>;

        }
    }
}
