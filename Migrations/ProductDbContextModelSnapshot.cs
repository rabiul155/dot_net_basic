using Dot_Net_Basic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dot_Net_Basic.Migrations;

[DbContext(typeof(ProductDbContext))]
partial class ProductDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.7")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("Dot_Net_Basic.Models.Product", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer");

            b.Property<string>("Description")
                .HasColumnType("text");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.Property<decimal>("Price")
                .HasColumnType("numeric");

            b.Property<int>("Quantity")
                .HasColumnType("integer");

            b.HasKey("Id");

            b.ToTable("Products");
        });
#pragma warning restore 612, 618
    }
}
