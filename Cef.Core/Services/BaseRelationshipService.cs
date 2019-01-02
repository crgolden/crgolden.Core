﻿namespace Cef.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Relationships;

    [PublicAPI]
    public abstract class BaseRelationshipService<T, T1, T2> : IRelationshipService<T, T1, T2>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        protected readonly DbContext Context;

        protected BaseRelationshipService(DbContext context)
        {
            Context = context;
        }

        public virtual IEnumerable<T> Index()
        {
            return Context.Set<T>().AsNoTracking();
        }

        public virtual async Task<T> Details(Guid id1, Guid id2)
        {
            return await Context.FindAsync<T>(id1, id2);
        }

        public virtual async Task<T> Create(T relationship)
        {
            relationship.Created = relationship.Created > DateTime.MinValue
                ? relationship.Created
                : DateTime.Now;
            Context.Add(relationship);
            await Context.SaveChangesAsync();
            return relationship;
        }

        public virtual async Task<List<T>> CreateRange(List<T> relationships)
        {
            var created = DateTime.Now;
            foreach (var relationship in relationships)
            {
                relationship.Created = relationship.Created > DateTime.MinValue
                    ? relationship.Created
                    : created;
                Context.Add(relationship);
            }

            await Context.SaveChangesAsync();
            return relationships;
        }

        public virtual async Task Edit(T relationship)
        {
            relationship.Updated = relationship.Updated ?? DateTime.Now;
            Context.Entry(relationship).State = EntityState.Modified;
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await Context.FindAsync<T>(relationship.Model1Id, relationship.Model2Id) != null)
                {
                    throw;
                }
            }
        }

        public virtual async Task EditRange(List<T> relationships)
        {
            var updated = DateTime.Now;
            foreach (var relationship in relationships)
            {
                relationship.Updated = relationship.Updated ?? updated;
                Context.Entry(relationship).State = EntityState.Modified;
            }

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                foreach (var entry in e.Entries)
                {
                    var entity = (BaseRelationship<T1, T2>) entry.Entity;
                    if (await Context.FindAsync<T>(entity.Model1Id, entity.Model2Id) != null)
                    {
                        throw;
                    }
                }
            }
        }

        public virtual async Task Delete(Guid id1, Guid id2)
        {
            var entity = await Context.FindAsync<T>(id1, id2);
            if (entity != null)
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}