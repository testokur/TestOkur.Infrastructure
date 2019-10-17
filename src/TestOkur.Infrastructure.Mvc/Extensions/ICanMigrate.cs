namespace TestOkur.Infrastructure.Mvc.Extensions
{
    using System.Threading.Tasks;

    public interface ICanMigrate
    {
        void Migrate();

        Task MigrateAsync();
    }
}