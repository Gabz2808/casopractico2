using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAAP2.Data;
using CAAP2.Repository;


namespace AdvancedProgramming.Business
{
    public class UserManager
    {
        private readonly RepositoryUser _repositoryUser;
        public UserManager()
        {
            _repositoryUser = new RepositoryUser();
        }
        public IEnumerable<User> GetAllUsers()
        {
            var Users = _repositoryUser.GetAll();
            return Users;
        }
        public User GetById(int id)
        {

            var User = _repositoryUser.GetById(id);
            return User;
        }

        public User Add(User User)
        {
            _repositoryUser.Add(User);
            return User;
        }
        public void Save()
        {
            _repositoryUser.Save();

        }

        public User Update(User User)
        {
            _repositoryUser.Update(User);
            return User;
        }

        public void Delete(int id)
        {
            _repositoryUser.Delete(id);
        }

        public void Dispose()
        {
            _repositoryUser.Dispose();
        }
    }
}
