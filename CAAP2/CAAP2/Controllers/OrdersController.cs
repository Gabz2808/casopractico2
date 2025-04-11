using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvancedProgramming.Business;
using CAAP2.Business;
using CAAP2.Data;

namespace CAAP2.Controllers
{
    public class OrdersController : Controller
    {
        private OrderManager _manager = new OrderManager();
        private UserManager _userManager = new UserManager();

        // GET: Orders
        public ActionResult Index()
        {

            var orders = _manager.GetOrdersToProcess();
            var processingOrder = orders.FirstOrDefault(o => o.Status == "Processing");
            var model = new Tuple<IEnumerable<Order>, Order>(orders, processingOrder);
            return View(model);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _manager.GetById((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,OrderDetail,CreatedDate,Priority,TotalAmount,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                _manager.Add(order);
                _manager.Save();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _manager.GetById((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Users = _userManager.GetAllUsers();
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,OrderDetail,CreatedDate,Priority,TotalAmount,Status")] Order order)
        {
            if (ModelState.IsValid)
            {
                _manager.Update(order);
                _manager.Save();
                return RedirectToAction("Index");
            }

            ViewBag.Users = _userManager.GetAllUsers();
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _manager.GetById((int)id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = _manager.GetById((int)id);
            _manager.Delete(id);
            _manager.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _manager.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Orders/CurrentProcessing
        public PartialViewResult CurrentProcessing()
        {
            var processingOrders = _manager.GetAllOrders()
                .Where(o => o.Status == "Processing")
                .OrderBy(o => o.Priority)
                .ThenBy(o => o.CreatedDate)
                .ToList();

            if (processingOrders.Any())
            {
                processingOrders[0].IsBeingProcessed = true;
            }
            return PartialView("ProcessingOrderPartial", processingOrders);
        }


        // GET: Orders/CurrentPending
        public PartialViewResult CurrentPending()
        {
            var pendingOrders = _manager.GetAllOrders().Where(o => o.Status == "Pending").ToList();
            return PartialView("PendingOrderPartial", pendingOrders);
        }


        // POST: Orders/Execute
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Execute(int id)
        {
            Order order = _manager.GetById(id);

            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (order.Status == "Pending")
            {
                order.Status = "Processing";
                _manager.Update(order);
                _manager.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "La orden no se puede ejecutar.");
            }

            
        }

        // POST: Orders/Pause
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pause(int id)
        {
            Order order = _manager.GetById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (order.Status != "Processing")
            {
                order.Status = "Paused";
                _manager.Update(order);
                _manager.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "La orden está en procesamiento y no se puede pausar.");
            }
        }

        // GET: Orders/CurrentPaused
        public PartialViewResult CurrentPaused()
        {
            var pausedOrders = _manager.GetAllOrders().Where(o => o.Status == "Paused").ToList();
            return PartialView("PausedOrderPartial", pausedOrders);
        }

        // POST: Orders/Resume
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Resume(int id)
        {
            Order order = _manager.GetById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (order.Status == "Paused")
            {
                order.Status = "Pending";
                _manager.Update(order);
                _manager.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "La orden no se pudo reanudar.");
            }
        }

        // POST: Orders/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            Order order = _manager.GetById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (order.Status != "Processing")
            {
                order.Status = "Cancelled";
                _manager.Update(order);
                _manager.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "La orden está en procesamiento y no se puede cancelar.");
            }
        }

        // POST: Orders/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelProcessing(int id)
        {
            Order order = _manager.GetById(id);
            if (order == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (!order.IsBeingProcessed)
            {
                order.Status = "Pending";
                _manager.Update(order);
                _manager.Save();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "La orden está en procesamiento y no se puede cancelar.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdvanceProcessingQueue()
        {
            var processingOrders = _manager.GetAllOrders()
                .Where(o => o.Status == "Processing")
                .OrderBy(o => o.Priority)
                .ThenBy(o => o.CreatedDate)
                .ToList();

            if (processingOrders.Any())
            {
                var current = processingOrders[0];
                current.Status = "Processed";
                _manager.Update(current);

                if (processingOrders.Count > 1)
                {
                    var next = processingOrders[1];
                }

                _manager.Save();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


    }
}
