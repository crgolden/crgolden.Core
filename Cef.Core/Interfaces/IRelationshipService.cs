namespace Cef.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Relationships;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IRelationshipService<T, T1, T2>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2: BaseModel
    {
        IEnumerable<T> Index();
        Task<T> Details(Guid id1, Guid id2);
        Task<T> Create(T relationship, DateTime? created = null);
        Task<List<T>> Create(List<T> relationships, DateTime? created = null);
        Task Edit(T relationship, DateTime? updated = null);
        Task Edit(List<T> relationships, DateTime? updated = null);
        Task Delete(Guid id1, Guid id2);
    }
}