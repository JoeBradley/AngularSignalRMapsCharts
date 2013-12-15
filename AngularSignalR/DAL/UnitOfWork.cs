using AngularSignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularSignalR.DAL
{
    public class UnitOfWork : IDisposable
    {
        private DataContext context = new DataContext();
        private EntityRepository<Store> storeRepository;
        private EntityRepository<Category> categoryRepository;
        private EntityRepository<Product> productRepository;
        private EntityRepository<EventHistory> eventsRepository;

        public EntityRepository<Store> StoreRepository
        {
            get
            {
                if (this.storeRepository == null)
                {
                    this.storeRepository = new EntityRepository<Store>(context);
                }
                return storeRepository;
            }
        }
        public EntityRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new EntityRepository<Category>(context);
                }
                return categoryRepository;
            }
        }
        public EntityRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new EntityRepository<Product>(context);
                }
                return productRepository;
            }
        }
        public EntityRepository<EventHistory> EventsRepository
        {
            get
            {
                if (this.eventsRepository == null)
                {
                    this.eventsRepository = new EntityRepository<EventHistory>(context);
                }
                return eventsRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}