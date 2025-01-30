using Fundipedia.TechnicalInterview.Data.Repositories;
using Fundipedia.TechnicalInterview.Domain.Validators;
using Fundipedia.TechnicalInterview.Model.Extensions;
using Fundipedia.TechnicalInterview.Model.Supplier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Domain.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly ISupplierValidator _supplierValidator;

    public SupplierService(ISupplierRepository supplierRepository, ISupplierValidator supplierValidator)
    {
        _supplierRepository = supplierRepository;
        _supplierValidator = supplierValidator;
    }

    public async Task<Supplier> GetSupplier(Guid id)
    {
        return await _supplierRepository.GetSupplierAsync(id);
    }

    public async Task<List<Supplier>> GetSuppliers()
    {
        return await _supplierRepository.GetAllSuppliersAsync();
    }

    public async Task<ValidationResult> InsertSupplier(Supplier supplier)
    {
        var result = _supplierValidator.Validate(supplier);

        if (!result.IsValid)
        {
            return result;
        }

        await _supplierRepository.AddSupplierAsync(supplier);

        return result;
    }

    public async Task<Supplier> DeleteSupplier(Guid id)
    {
        var supplier = await _supplierRepository.GetSupplierAsync(id);
        if (supplier != null)
        {
            if (supplier.IsActive())
            {
                throw new Exception($"Supplier {id} is active, can't be deleted");
            }

            await _supplierRepository.RemoveSupplierAsync(supplier);
        }

        return supplier;
    }
}