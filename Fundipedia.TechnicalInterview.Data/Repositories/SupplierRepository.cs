using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Data.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SupplierContext _context;

        public SupplierRepository(SupplierContext context)
        {
            _context = context;
        }

        public async Task<Supplier> GetSupplierAsync(Guid id)
        {
            return await _context.Suppliers
                .Include(x => x.Emails)
                .Include(x => x.Phones)
                .FirstOrDefaultAsync(supplier => supplier.Id == id);
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Include(x => x.Emails)
                .Include(x => x.Phones)
                .ToListAsync();
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
