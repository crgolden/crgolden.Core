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
        Task<T> Create(T relationship);
        Task Edit(T relationship);
        Task Delete(Guid id1, Guid id2);
    }
}