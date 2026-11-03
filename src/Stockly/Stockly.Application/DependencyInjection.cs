using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Stockly.Application.Common.Interfaces;

namespace Stockly.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IUnitOfWork>();

        services.Scan(scan => scan
            .FromAssemblyOf<IUnitOfWork>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("UseCase")), publicOnly: false)
            .AsSelf()
            .WithScopedLifetime());

        return services;
    }
}
