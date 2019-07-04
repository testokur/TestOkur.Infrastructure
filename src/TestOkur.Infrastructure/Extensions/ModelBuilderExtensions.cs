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
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();
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
		        key.Relational().Name = key.Relational().Name.ToSnakeCase();
	        }
        }

        private static void ConvertForeignKeys(IMutableEntityType entity)
        {
	        foreach (var key in entity.GetForeignKeys())
	        {
		        key.Relational().Name = key.Relational().Name.ToSnakeCase();
	        }
        }

        private static void ConvertIndexNames(IMutableEntityType entity)
        {
	        foreach (var index in entity.GetIndexes())
	        {
		        index.Relational().Name = index.Relational().Name.ToSnakeCase();
	        }
        }

        private static void ConvertColumnNames(IMutableEntityType entity)
        {
	        foreach (var property in entity.GetProperties())
	        {
		        property.Relational().ColumnName = property.Relational()
			        .ColumnName
			        .ToSnakeCase();
	        }
        }
    }
}
