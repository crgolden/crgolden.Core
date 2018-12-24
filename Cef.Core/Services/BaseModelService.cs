namespace Cef.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Models;

    [PublicAPI]
    public abstract class BaseModelService<T> : IModelService<T> where T : BaseModel
    {
        protected readonly DbContext Context;

        protected BaseModelService(DbContext context)
        {
            Context = context;
        }

        public virtual IEnumerable<T> Index()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public virtual async Task<T> Details(Guid id)
        {
            return await Context.FindAsync<T>(id);
        }

        public virtual async Task<T> Create(T model)
        {
            model.Created = DateTime.Now;
            Context.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public virtual async Task Edit(T model)
        {
            model.Updated = DateTime.Now;
            Context.Entry(model).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await Context.FindAsync<T>(id);
            if (entity != null)
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}