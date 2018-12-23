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
            return await Context.Set<T>().SingleOrDefaultAsync(x =>
                x.Model1Id.Equals(id1) &&
                x.Model2Id.Equals(id2));
        }

        public virtual async Task<T> Create(T relationship)
        {
            relationship.Created = DateTime.Now;
            Context.Add(relationship);
            await Context.SaveChangesAsync();
            return relationship;
        }

        public virtual async Task Edit(T relationship)
        {
            relationship.Updated = DateTime.Now;
            Context.Entry(relationship).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id1, Guid id2)
        {
            var entity = await Context.Set<T>().SingleOrDefaultAsync(x =>
                x.Model1Id.Equals(id1) &&
                x.Model2Id.Equals(id2));
            if (entity != null)
            {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}