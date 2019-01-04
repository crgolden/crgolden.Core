namespace Cef.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IRelationshipService<T, T1, T2>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2: BaseModel
    {
        Task<IEnumerable<T>> Index();
        Task<T> Details(Guid id1, Guid id2);
        Task<T> Create(T relationship);
        Task<List<T>> CreateRange(List<T> relationships);
        Task Edit(T relationship);
        Task EditRange(List<T> relationships);
        Task Delete(Guid id1, Guid id2);
    }
}