using StoreOnline.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StoreOnline.Infrastructure.DataSource.ModelConfig;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Car>
{
    // Si necesitamos db constrains, este es el lugar 
    public void Configure(EntityTypeBuilder<Car> builder)
    {
           
    }
}

