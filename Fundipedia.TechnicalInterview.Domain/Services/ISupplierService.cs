﻿using Fundipedia.TechnicalInterview.Model.Supplier;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Domain.Services;

public interface ISupplierService
{
    Task<List<Supplier>> GetSuppliers();

    Task<Supplier> GetSupplier(Guid id);

    Task<ValidationResult> InsertSupplier(Supplier supplier);

    Task<Supplier> DeleteSupplier(Guid id);
}