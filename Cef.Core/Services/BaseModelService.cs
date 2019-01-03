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

#pragma warning disable 1998
        public virtual async Task<IEnumerable<T>> Index()
        {
            return Context.Set<T>().AsNoTracking();
        }
#pragma warning restore 1998

        public virtual async Task<T> Details(Guid id)
        {
            return await Context.FindAsync<T>(id);
        }

        public virtual async Task<T> Create(T model)
        {
            model.Created = model.Created > DateTime.MinValue
                ? model.Created
                : DateTime.Now;
            Context.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public virtual async Task<List<T>> CreateRange(List<T> models)
        {
            var created = DateTime.Now;
            foreach (var model in models)
            {
                model.Created = model.Created > DateTime.MinValue
                    ? model.Created
                    : created;
                Context.Add(model);
            }

            await Context.SaveChangesAsync();
            return models;
        }

        public virtual async Task Edit(T model)
        {
            model.Updated = model.Updated ?? DateTime.Now;
            Context.Entry(model).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await Context.FindAsync<T>(model.Id) != null)
                {
                    throw;
                }
            }
        }

        public virtual async Task EditRange(List<T> models)
        {
            var updated = DateTime.Now;
            foreach (var model in models)
            {
                model.Updated = model.Updated ?? updated;
                Context.Entry(model).State = EntityState.Modified;
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                foreach (var entry in e.Entries)
                {
                    var entity = (BaseModel)entry.Entity;
                    if (await Context.FindAsync<T>(entity.Id) != null)
                    {
                        throw;
                    }
                }
            }
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