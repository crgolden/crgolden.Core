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

        public virtual async Task<T> Create(T model, DateTime? created = null)
        {
            model.Created = created ?? DateTime.Now;
            Context.Add(model);
            await Context.SaveChangesAsync();
            return model;
        }

        public virtual async Task<List<T>> Create(List<T> models, DateTime? created = null)
        {
            created = created ?? DateTime.Now;
            foreach (var model in models)
            {
                model.Created = created.Value;
                Context.Add(model);
            }

            await Context.SaveChangesAsync();
            return models;
        }

        public virtual async Task Edit(T model, DateTime? updated = null)
        {
            model.Updated = updated ?? DateTime.Now;
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

        public virtual async Task Edit(List<T> models, DateTime? updated = null)
        {
            updated = updated ?? DateTime.Now;
            foreach (var model in models)
            {
                model.Updated = updated.Value;
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