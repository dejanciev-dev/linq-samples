using LinqSamples.App.Ordering.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LinqSamples.App.Ordering
{
    public class LinqOrdering
    {
        public static readonly List<OrderingModel> models = new List<OrderingModel>
        {
            new OrderingModel
            {
                Name = "Test1",
                Status = "Pending"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Awaiting Payment"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Shipped"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Awaiting Payment"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Completed"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Completed"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Shipped"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Cancelled"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Cancelled"
            },
             new OrderingModel
            {
                Name = "Test2",
                Status = "Declined"
            }
        };

        public static List<OrderingModel> LinqOrderBySpecificValues()
        {
            List<string> preferences = new List<string>
            {
                "Awaiting Payment", "Pending", "Declined", "Cancelled", "Shipped", "Completed"
            };
            var orderingModels = models.OrderBy(x => preferences.IndexOf(x.Status)).ToList();
            return orderingModels;
        }
    }
}