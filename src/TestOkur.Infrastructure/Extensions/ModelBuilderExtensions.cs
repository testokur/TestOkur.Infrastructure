namespace TestOkur.Infrastructure.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public static class ModelBuilderExtensions
    {
        public static void ToSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());
                ConvertColumnNames(entity);
                ConvertRelationalNames(entity);
                ConvertForeignKeys(entity);
                ConvertIndexNames(entity);
            }
        }

        private static void ConvertRelationalNames(IMutableEntityType entity)
        {
            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName().ToSnakeCase());
            }
        }

        private static void ConvertForeignKeys(IMutableEntityType entity)
        {
            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
            }
        }

        private static void ConvertIndexNames(IMutableEntityType entity)
        {
            foreach (var index in entity.GetIndexes())
            {
                index.SetName(index.GetName().ToSnakeCase());
            }
        }

        private static void ConvertColumnNames(IMutableEntityType entity)
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName().ToSnakeCase());
            }
        }
    }
}
