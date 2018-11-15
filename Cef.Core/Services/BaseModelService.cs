namespace Cef.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;

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
            return await Context.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<T> Create(T model)
        {
            Context.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public virtual async Task Edit(T model)
        {
            var entity = await Context.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(model.Id));
            if (entity != null)
            {
                entity.Name = model.Name;
            }
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await Context.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (entity != null)
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}