﻿using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Data.Repositories;
using Fundipedia.TechnicalInterview.Domain.Services;
using Fundipedia.TechnicalInterview.Domain.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fundipedia.TechnicalInterview;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<SupplierContext>(options =>
            options.UseInMemoryDatabase(databaseName: "SupplierDatabase"));

        // Services
        services.AddTransient<ISupplierService, SupplierService>();
        services.AddTransient<ISupplierRepository, SupplierRepository>();

        // Validators
        services.AddTransient<ISupplierValidator, SupplierValidator>();
        services.AddTransient<IEmailValidator, EmailValidator>();
        services.AddTransient<IPhoneValidator, PhoneValidator>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(builder =>
        {
            builder.MapControllers();
        });
    }
}