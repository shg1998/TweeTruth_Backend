﻿using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data.Contracts;
using Entities.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using WebFrameworks.Api;
using WebFrameworks.Filters;

namespace MyBackendApis.Controllers
{
    [ApiController]
    [Authorize]
    [ApiResultFilter]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> _repository;
        private readonly IMapper _mapper;

        public AccountController(IRepository<Account> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AbstractAccountDto>>> Get(CancellationToken cancellationToken)
        {
            var list = await _repository.TableNoTracking.ProjectTo<AbstractAccountDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<AccountDto>> Get(int id, CancellationToken cancellationToken)
        {
            var dto = await _repository.TableNoTracking.ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (dto == null)
                return NotFound();

            return dto;
        }

       
        [HttpPost]
        public async Task<ApiResult<AccountDto>> Create(CreateAccountDto dto, CancellationToken cancellationToken)
        {
            var model = dto.ToEntity(_mapper);
            model.AuthorId = int.Parse(User.Identity.GetUserId());
            await _repository.AddAsync(model, cancellationToken);

            var resultDto = await _repository.TableNoTracking.ProjectTo<AccountDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            return resultDto;
        }

        [HttpPut("{id:int}")]
        public async Task<ApiResult<AccountDto>> Update(int id, EditAccountDto dto, CancellationToken cancellationToken)
        {
            //var userId = int.Parse(User.Identity.GetUserId());
            //if (userId != id) throw new UnauthorizedAccessException("عدم دسترسی");

            var model = await _repository.GetByIdAsync(cancellationToken, id);

            _mapper.Map(dto, model);

            await _repository.UpdateAsync(model, cancellationToken);

            var resultDto = await _repository.TableNoTracking.ProjectTo<AccountDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(p => p.Id == model.Id, cancellationToken);

            return resultDto;
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            //var userId = int.Parse(User.Identity.GetUserId());
            //if (userId != id) throw new UnauthorizedAccessException("عدم دسترسی");

            var model = await _repository.GetByIdAsync(cancellationToken, id);
            await _repository.DeleteAsync(model, cancellationToken);

            return Ok();
        }
    }
}
