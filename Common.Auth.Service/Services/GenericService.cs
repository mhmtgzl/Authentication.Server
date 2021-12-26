using Common.Auth.Core.Repositories;
using Common.Auth.Core.Services;
using Common.Auth.Core.UnitOfWork;
using Common.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Service.Services
{
    public class GenericService<TEntity, TDto> : IService<TEntity, TDto> where TEntity : class where TDto : class
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<TEntity> repository;

        public GenericService(IUnitOfWork unitOfWork, IRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            await repository.AddAsync(newEntity);

            await unitOfWork.CommitAsync();

            var dto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(dto, 200);
        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {

            var records = ObjectMapper.Mapper.Map<List<TDto>>(await repository.GetAllAsync());
            return Response<IEnumerable<TDto>>.Success(records, 200);

        }

        public async Task<Response<TDto>> GetAsync(int id)
        {
            var entity = await repository.GetAsync(id);
            if (entity == null)
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }

            var dto = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dto, 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {

            var record = await repository.GetAsync(id);
            if (record == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            repository.Remove(record);

            return Response<NoDataDto>.Success(204 );
        }

        public async Task<Response<NoDataDto>> UpdateAsync(TDto entity,int id)
        {

            var record = await repository.GetAsync(id);
            if (record == null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }

            var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entity);

            repository.Update(updateEntity);

            await unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(204);


        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = repository.Where(predicate);
            
            var retDto = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());
            
            return Response<IEnumerable<TDto>>.Success(retDto,200);

        }
    }
}
