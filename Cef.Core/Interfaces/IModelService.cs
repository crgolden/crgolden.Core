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
        IEnumerable<T> Index();
        Task<T> Details(Guid id);
        Task<T> Create(T model, DateTime? created = null);
        Task<List<T>> Create(List<T> models, DateTime? created = null);
        Task Edit(T model, DateTime? updated = null);
        Task Edit(List<T> models, DateTime? updated = null);
        Task Delete(Guid id);
    }
}