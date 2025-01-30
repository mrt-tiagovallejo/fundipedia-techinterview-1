using Fundipedia.TechnicalInterview.Model.Supplier;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Fundipedia.TechnicalInterview.Data.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetSupplierAsync(Guid id);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task AddSupplierAsync(Supplier supplier);
        Task RemoveSupplierAsync(Supplier supplier);
    }
}
