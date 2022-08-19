using System.Threading.Tasks;
using Entities.User;

namespace Service.Services
{
    public interface IJwtService
    {
        Task<string> GenerateAsync(User user);
    }
}
