using Common.Auth.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Auth.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DbContext context;

        public UnitOfWork(AppDbContext dbContext)
        {
            context = dbContext;
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
