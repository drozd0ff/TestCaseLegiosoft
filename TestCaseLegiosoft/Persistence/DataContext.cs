using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Persistence
{
    public class DataContext : DbContext
    {
        public DbSet<TransactionModel> TransactionModels { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var transactionStatusConverter = new EnumToStringConverter<TransactionStatus>();
            var transactionTypeConverter = new EnumToStringConverter<TransactionType>();

            modelBuilder.Entity<TransactionModel>()
                .Property(x => x.TransactionStatus)
                .HasConversion(transactionStatusConverter);

            modelBuilder.Entity<TransactionModel>()
                .Property(x => x.TransactionType)
                .HasConversion(transactionTypeConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
