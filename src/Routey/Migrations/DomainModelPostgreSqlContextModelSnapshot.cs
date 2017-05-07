using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Routey.Models;

namespace Routey.Migrations
{
    [DbContext(typeof(DomainModelPostgreSqlContext))]
    partial class DomainModelPostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Routey.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("AddressConcat");

                    b.Property<string>("AddressDisplay");

                    b.Property<string>("City");

                    b.Property<string>("GoogleId");

                    b.Property<string>("Latitude");

                    b.Property<string>("LocationType");

                    b.Property<string>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("RouteId");

                    b.Property<bool>("Save");

                    b.Property<string>("State");

                    b.Property<string>("YelpId");

                    b.Property<string>("Zip");

                    b.HasKey("LocationId");

                    b.HasIndex("RouteId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Routey.Models.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<bool>("SaveRoute");

                    b.HasKey("RouteId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Routey.Models.Location", b =>
                {
                    b.HasOne("Routey.Models.Route", "Route")
                        .WithMany("Locations")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
