using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAAP2.Data;
using CAAP2.Repository;


namespace CAAP2.Business
{

    public class OrderManager
    {
        private readonly RepositoryOrder _repositoryOrder;
        public OrderManager()
        {
            _repositoryOrder = new RepositoryOrder();
        }
        public IEnumerable<Order> GetAllOrders()
        {
            var orders = _repositoryOrder.GetAll();
            return orders;
        }
        public Order GetById(int id)
        {

            var Order = _repositoryOrder.GetById(id);
            return Order;
        }

        public Order Add(Order Order)
        {
            _repositoryOrder.Add(Order);
            return Order;
        }
        public void Save()
        {
            _repositoryOrder.Save();

        }

        public Order Update(Order Order)
        {
            _repositoryOrder.Update(Order);
            return Order;
        }

        public void Delete(int id)
        {
            _repositoryOrder.Delete(id);
        }

        public void Dispose()
        {
            _repositoryOrder.Dispose();
        }

        public List<Order> GetOrdersToProcess()
        {
            var orders = _repositoryOrder.GetAll()
                .Where(o => o.Status == "Pending")
                .OrderByDescending(o => o.User.IsPremium)
                .ThenByDescending(o => o.Priority)
                .ThenBy(o=> o.CreatedDate).ToList();
            return orders;
        }
    }
}
