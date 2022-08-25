using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entities.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using WebFrameworks.Api;
using WebFrameworks.Filters;

namespace MyBackendApis.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [ApiResultFilter]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IRepository<Post> _repository;
        private readonly IMapper _mapper;

        public PostsController(IRepository<Post> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> Get(CancellationToken cancellationToken)
        {
            #region old code
            //var posts = await _repository.TableNoTracking
            //    .Include(p => p.Category).Include(p => p.Author).ToListAsync(cancellationToken);
            //var list = posts.Select(p =>
            //{
            //    var dto = Mapper.Map<PostDto>(p);
            //    return dto;
            //}).ToList();

            //var list = await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).ToListAsync(cancellationToken);
            #endregion

            var list = await _repository.TableNoTracking.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .Where(postDto => postDto.Title.Contains("test") || postDto.CategoryName.Contains("test"))
                .ToListAsync(cancellationToken);

            return Ok(list);
        }

        [HttpGet("{id:guid}")]
        public async Task<ApiResult<PostDto>> Get(Guid id, CancellationToken cancellationToken)
        {

            var dto = await _repository.TableNoTracking.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (dto == null)
                return NotFound();

            #region old code
            //var dto = new PostDto
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    Description = model.Description,
            //    CategoryId = model.CategoryId,
            //    AuthorId = model.AuthorId,
            //    AuthorFullName = model.Author.FullName,
            //    CategoryName = model.Category.Name
            //};
            #endregion

            return dto;
        }

        [HttpPost]
        public async Task<ApiResult<PostDto>> Create(PostDto dto, CancellationToken cancellationToken)
        {
            //var model = _mapper.Map<Post>(dto);
            var model = dto.ToEntity(_mapper);
            #region old code
            //var model = new Post
            //{
            //    Title = dto.Title,
            //    Description = dto.Description,
            //    CategoryId = dto.CategoryId,
            //    AuthorId = dto.AuthorId
            //};
            #endregion

            await _repository.AddAsync(model, cancellationToken);

            #region old code
            //await _repository.LoadReferenceAsync(model, p => p.Category, cancellationToken);
            //await _repository.LoadReferenceAsync(model, p => p.Author, cancellationToken);
            //model = await _repository.TableNoTracking
            //    .Include(p => p.Category)
            //    .Include(p =>p.Author)
            //    .SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            //var resultDto = new PostDto
            //{
            //    Id = model.Id,
            //    Title = model.Title,
            //    Description = model.Description,
            //    CategoryId = model.CategoryId,
            //    AuthorId = model.AuthorId,
            //    AuthorName = model.Author.FullName,
            //    CategoryName = model.Category.Name
            //};


            //var resultDto = await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            #endregion

            var resultDto = await _repository.TableNoTracking.ProjectTo<PostDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            return resultDto;
        }

        [HttpPut]
        public async Task<ApiResult<PostDto>> Update(Guid id, PostDto dto, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(cancellationToken, id);

            _mapper.Map(dto, model);

            #region old code
            //model.Title = dto.Title;
            //model.Description = dto.Description;
            //model.CategoryId = dto.CategoryId;
            //model.AuthorId = dto.AuthorId;
            #endregion

            await _repository.UpdateAsync(model, cancellationToken);

            #region old code
            //var resultDto = await _repository.TableNoTracking.Select(p => new PostDto
            //{
            //    Id = p.Id,
            //    Title = p.Title,
            //    Description = p.Description,
            //    CategoryId = p.CategoryId,
            //    AuthorId = p.AuthorId,
            //    AuthorFullName = p.Author.FullName,
            //    CategoryName = p.Category.Name
            //}).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);
            #endregion

            var resultDto = await _repository.TableNoTracking.ProjectTo<PostDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            return resultDto;
        }

        [HttpDelete("{id:guid}")]
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var model = await _repository.GetByIdAsync(cancellationToken, id);
            await _repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }
    }
}
