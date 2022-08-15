﻿using System.Threading;
using System.Threading.Tasks;
using Data.Repositories;
using Entities.User;

namespace Data.Contracts
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);

        Task AddAsync(User user, string password, CancellationToken cancellationToken);
    }
}