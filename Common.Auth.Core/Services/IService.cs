using Common.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Core.Services
{
    public interface IService<TEntity, TDto> where TEntity : class where TDto : class
    {
        Task<Response<TDto>> GetAsync(int id);
        Task<Response<IEnumerable<TDto>>> GetAllAsync();
        Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<Response<TDto>> AddAsync(TDto entity);
        Task<Response<NoDataDto>> UpdateAsync(TDto entity,int id);
        Task<Response<NoDataDto>> Remove(int id);
    }
}
