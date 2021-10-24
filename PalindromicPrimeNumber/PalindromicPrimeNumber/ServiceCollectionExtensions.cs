using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PalindromicPrimeNumber.Services;

namespace PalindromicPrimeNumber
{
    public static class ServiceCollectionExtensions
    {
        public static void InstallPalindromicPrimeServices(this IServiceCollection serviceCollection)
        {
            AddBaseConvertors(serviceCollection);
            serviceCollection.AddScoped<IPrimeNumberValidator, PrimeNumberValidator>();
            serviceCollection.AddScoped<IPalindromicValidator, PalindromicValidator>();
            serviceCollection.AddScoped<IPalindromicPrimeNumberProvider, PalindromicPrimeNumberProvider>();
        }

        private static void AddBaseConvertors(IServiceCollection serviceCollection)
        {
            var baseConvertors = Assembly.GetAssembly(typeof(IBaseConvertor))
                .GetTypesAssignableFrom<IBaseConvertor>();

            foreach (var baseConvertor in baseConvertors)
                serviceCollection.AddScoped(typeof(IBaseConvertor), baseConvertor);
        }


        private static IEnumerable<Type> GetTypesAssignableFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypesAssignableFrom(typeof(T));
        }

        private static IEnumerable<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType)
        {
            return assembly.DefinedTypes
                .Where(type => compareType.IsAssignableFrom(type) && compareType != type);
        }
    }
}