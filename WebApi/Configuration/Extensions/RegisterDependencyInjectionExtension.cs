using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.Services;
using Domain.Interfaces.Repositories;
using Data.Repositories;
using Logic;

namespace WebApi.Configuration.Extensions
{
    public static class RegisterDependencyInjectionExtension
    {
        public static WebApplicationBuilder ConfigureDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddSingleton<IUniversalFeesExchange>(UniversalFeesExchange.Instance);

            return builder;
        }
    }
}