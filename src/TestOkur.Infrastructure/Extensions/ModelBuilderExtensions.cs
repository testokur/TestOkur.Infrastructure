namespace Microsoft.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ModelBuilderExtensions
    {
        public static void ToSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Relational()
                        .ColumnName
                        .ToSnakeCase();
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                }
            }
        }

        public static void ApplyAllConfigurationsFromCurrentAssembly(this ModelBuilder modelBuilder, string configNamespace = "")
        {
            var applyMethod = GetApplyConfigurationMethod();

            foreach (var type in GetTypes(configNamespace))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsConstructedGenericType ||
                        iface.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
                    {
                        continue;
                    }

                    var applyConcreteMethod =
                        applyMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                    applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                }
            }
        }

        private static IEnumerable<Type> GetTypes(string configNamespace)
        {
            var types = Assembly
                .GetCallingAssembly()
                .GetTypes()
                .Where(c => c.IsClass &&
                            !c.IsAbstract &&
                            !c.ContainsGenericParameters);

            return string.IsNullOrEmpty(configNamespace)
                ? types
                : types.Where(c => c.Namespace == configNamespace);
        }

        private static MethodInfo GetApplyConfigurationMethod()
        {
            return typeof(ModelBuilder)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name == "ApplyConfiguration" &&
                            m.GetParameters().Any(p => p.ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));
        }
    }
}
