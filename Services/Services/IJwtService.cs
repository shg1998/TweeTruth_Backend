using Entities.User;

namespace Services.Services
{
    public interface IJwtService
    {
        string Generate(User user);
    }
}
