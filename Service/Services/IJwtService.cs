using Entities.User;

namespace Service.Services
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}
