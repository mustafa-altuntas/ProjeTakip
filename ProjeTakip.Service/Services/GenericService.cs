using ProjeTakip.Core.Repositories;
using ProjeTakip.Core.Services;
using ProjeTakip.Core.UnitOfWork;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjeTakip.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _genericRepository;

        public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
            await _genericRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);

            return Response<TDto>.Success(newDto, 200);

        }

        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = _genericRepository.GetAll();
            var dtoEntities = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(entities.ToList());
            return Response<IEnumerable<TDto>>.Success(dtoEntities, 200);


        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity is null)
            {
                return Response<TDto>.Fail("Id not found", 404, true);
            }
            var dtoEntity = ObjectMapper.Mapper.Map<TDto>(entity);
            return Response<TDto>.Success(dtoEntity, 200);
        }

        public async Task<Response<NoDataDto>> Remove(int id)
        {
            var entity = await _genericRepository.GetByIdAsync(id);
            if (entity is null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }
            await _genericRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public async Task<Response<NoDataDto>> Update(TDto dto, int id)
        {
            var isExistEntity = await _genericRepository.GetByIdAsync(id);
            if (isExistEntity is null)
            {
                return Response<NoDataDto>.Fail("Id not found", 404, true);
            }
            await _genericRepository.Update(ObjectMapper.Mapper.Map<TEntity>(dto));
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        public Response<IEnumerable<TDto>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _genericRepository.Where(predicate);
            return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(list.ToList()), 200);

            
        }
    }
}
