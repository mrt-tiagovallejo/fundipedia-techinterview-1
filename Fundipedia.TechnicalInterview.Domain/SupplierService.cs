using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Model.Extensions;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Domain;

public class SupplierService : ISupplierService
{
    private readonly SupplierContext _context;

    public SupplierService(SupplierContext context)
    {
        _context = context;
    }

    public async Task<Supplier> GetSupplier(Guid id)
    {
        var supplier = await _context.Suppliers
            .Include(x => x.Emails)
            .Include(x => x.Phones)
            .FirstOrDefaultAsync(supplier => supplier.Id == id);
        
        return supplier;
    }

    public async Task<List<Supplier>> GetSuppliers()
    {
        return await _context.Suppliers
            .Include(x => x.Emails)
            .Include(x => x.Phones)
            .ToListAsync();
    }

    public async Task InsertSupplier(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
    }

    public async Task<Supplier> DeleteSupplier(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            if (supplier.IsActive())
            {
                throw new Exception($"Supplier {id} is active, can't be deleted");
            }

            _context.Suppliers.Remove(supplier);
        }

        return supplier;
    }
}