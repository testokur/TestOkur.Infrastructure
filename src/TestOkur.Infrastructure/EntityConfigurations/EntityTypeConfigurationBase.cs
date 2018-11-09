namespace TestOkur.Infrastructure.EntityConfigurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;

    public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property<DateTime>("CreatedOnUTC");
            builder.Property<DateTime>("UpdateOnUTC");
            builder.Property<int>("CreatedBy");
            builder.Property<int>("UpdatedBy");
            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }
}
