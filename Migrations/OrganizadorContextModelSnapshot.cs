﻿// <auto-generated />
using System;
using Concessionaria.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Concessionaria.Migrations
{
    [DbContext(typeof(OrganizadorContext))]
    partial class OrganizadorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Concessionaria.Entities.Moto", b =>
                {
                    b.Property<Guid>("IdMoto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Fuel")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MotoBrand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("StoreIdStore")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdMoto");

                    b.HasIndex("StoreIdStore");

                    b.ToTable("Motos");
                });

            modelBuilder.Entity("Concessionaria.Entities.Store", b =>
                {
                    b.Property<int>("IdStore")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFull")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdStore");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Concessionaria.Entities.User", b =>
                {
                    b.Property<Guid>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdUser");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Concessionaria.Entities.Moto", b =>
                {
                    b.HasOne("Concessionaria.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreIdStore")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Concessionaria.Entities.Auditable", "Auditable", b1 =>
                        {
                            b1.Property<Guid>("MotoIdMoto")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("AlterationUserId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("CreateUserId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTimeOffset?>("DateUpdate")
                                .HasColumnType("TEXT");

                            b1.Property<DateTimeOffset>("DateUpload")
                                .HasColumnType("TEXT");

                            b1.HasKey("MotoIdMoto");

                            b1.ToTable("Motos");

                            b1.WithOwner()
                                .HasForeignKey("MotoIdMoto");
                        });

                    b.Navigation("Auditable")
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("Concessionaria.Entities.User", b =>
                {
                    b.OwnsOne("Concessionaria.Entities.Auditable", "Auditable", b1 =>
                        {
                            b1.Property<Guid>("UserIdUser")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("AlterationUserId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid?>("CreateUserId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTimeOffset?>("DateUpdate")
                                .HasColumnType("TEXT");

                            b1.Property<DateTimeOffset>("DateUpload")
                                .HasColumnType("TEXT");

                            b1.HasKey("UserIdUser");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserIdUser");
                        });

                    b.Navigation("Auditable")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
