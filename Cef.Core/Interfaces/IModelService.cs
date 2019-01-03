namespace Cef.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IModelService<T> where T : BaseModel
    {
        Task<IEnumerable<T>> Index();
        Task<T> Details(Guid id);
        Task<T> Create(T model);
        Task<List<T>> CreateRange(List<T> models);
        Task Edit(T model);
        Task EditRange(List<T> models);
        Task Delete(Guid id);
    }
}