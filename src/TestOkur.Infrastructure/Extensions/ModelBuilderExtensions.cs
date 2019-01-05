namespace TestOkur.Infrastructure.Extensions
{
	using System;
	using Microsoft.EntityFrameworkCore;

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
    }
}
