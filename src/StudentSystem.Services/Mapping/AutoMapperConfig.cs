using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using AutoMapper;

namespace StudentSystem.Services.Mapping
{
    public class AutoMapperConfig
    {
        public static void RegisterAutomapper()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var myAssemblies = new List<Assembly>();

            foreach (Assembly assembly in assemblies)
            {
                if (assembly.GetName().Name.Contains("StudentSystem."))
                {
                    myAssemblies.Add(assembly);
                }
            }

            Mapper.Initialize(x => RegisterMappings(x, myAssemblies));
        }

        private static void RegisterMappings(IMapperConfigurationExpression config, IEnumerable<Assembly> assemblies)
        {
            // TODO remove DependencyResolver because it has dependency to MVC
            config.ConstructServicesUsing(t => DependencyResolver.Current.GetService(t));

            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetExportedTypes());
            }

            LoadStandardMappings(config, types);
        }

        private static void LoadStandardMappings(IMapperConfigurationExpression config, IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMap<>) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select new
                        {
                            Source = i.GetGenericArguments()[0],
                            Destination = t
                        }).ToArray();

            foreach (var map in maps)
            {
                config.CreateMap(map.Source, map.Destination);
                config.CreateMap(map.Destination, map.Source);
            }
        }
    }
}