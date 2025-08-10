using EghalTech.Models;

namespace EghalTech.Repository
{
    public interface IUserDataCleaner
    {
        void DeleteUserDataAsync(User user);
    }
}
