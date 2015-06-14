using AngularSignalRMapsCharts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularSignalRMapsCharts.DAL
{
    public class UnitOfWork : IDisposable
    {
        private DataContext context = new DataContext();
        private EntityRepository<Company> companyRepository;
        private EntityRepository<EventLog> eventsRepository;

        public EntityRepository<Company> CompanyRepository
        {
            get
            {
                if (this.companyRepository == null)
                {
                    this.companyRepository = new EntityRepository<Company>(context);
                }
                return companyRepository;
            }
        }
        public EntityRepository<EventLog> EventsRepository
        {
            get
            {
                if (this.eventsRepository == null)
                {
                    this.eventsRepository = new EntityRepository<EventLog>(context);
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