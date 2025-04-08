using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAAP2.Data;

namespace CAAP2.Repository
{
    public interface IRepositoryOrder : IRepositoryBase<Order>
    {
    }

    public class RepositoryOrder : RepositoryBase<Order>, IRepositoryOrder
    {
        public RepositoryOrder() : base()
        {
        }
        // Implement any specific methods for Order if needed
    }
}
