using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Domain.Validators;
using Fundipedia.TechnicalInterview.Model.Extensions;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Domain.Services;

public class SupplierService : ISupplierService
{
    private readonly SupplierContext _context;
    private readonly ISupplierValidator _supplierValidator;

    public SupplierService(SupplierContext context, ISupplierValidator supplierValidator)
    {
        _context = context;
        _supplierValidator = supplierValidator;
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

    public async Task<ValidationResult> InsertSupplier(Supplier supplier)
    {
        var result = _supplierValidator.Validate(supplier);

        if (!result.IsValid)
        {
            return result;
        }

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        return result;
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
            await _context.SaveChangesAsync();
        }

        return supplier;
    }
}