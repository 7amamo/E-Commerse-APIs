using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specifications
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        #region With Specification

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(Specifications<T> spec);
        Task<T> GetEntityWithSpecAsync(Specifications<T> spec);

        Task<int> GetCountWithSpec(Specifications<T> spec);
        #endregion

        Task AddAsync(T Item);
        void Update(T Item);
        void Delete(T Item);


    }
}
