using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAAP2.Data;

namespace CAAP2.Repository
{
    public interface IRepositoryUser : IRepositoryBase<User>
    {
    }

    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        public RepositoryUser() : base()
        {
        }
        // Implement any specific methods for User if needed
    }
}
