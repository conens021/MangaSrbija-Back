using MangaSrbija.DAL.Entities.User;

namespace MangaSrbija.DAL.Repositories.users
{
    public interface IUserRepository
    {
        public User GetById(int id);
        public int Save(User category);
        public void Delete(int id);
        public User Update(User user);
        public void UpdateUserPasswod(string password, int userId);
        public User GetByUsernameOrEmail(string usernameOrEmail);
        User GetByUsernameAndPassword(string usernameOrEmail, string password);
        List<int> GetAllUsersIds();
    }
}
