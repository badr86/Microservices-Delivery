﻿using Infrastructure;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessCenter.Entity;
using CommandCenter.Entity;

namespace ProcessCenter.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly IRepository<Order> repository;
        public OrderCreatedConsumer(IRepository<Order> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var message = context.Message;
            var item = await repository.GetAsync(message.Id);
            if (item != null)
            {
                return;
            }

            item = new Order
            {
                Id = message.Id,
                Address = message.Address,
                Quantity = message.Quantity
            };

            await repository.CreateAsync(item);
        }
    }
}
