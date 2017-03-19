using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using foodynet_api_anais.Models;

namespace foodynet_api_anais.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20170201180215_testSQL2")]
    partial class testSQL2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("foodynet_api_anais.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ingredient");

                    b.Property<string>("Name");

                    b.Property<int>("Time");

                    b.Property<string>("cooking");

                    b.HasKey("Id");

                    b.ToTable("Recipe");
                });
        }
    }
}
