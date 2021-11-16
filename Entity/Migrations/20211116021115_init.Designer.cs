﻿// <auto-generated />
using System;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Entity.Migrations
{
    [DbContext(typeof(CompanyContext))]
    [Migration("20211116021115_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Entity.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Entity.ConsumableInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(32)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("Money")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Num")
                        .HasColumnType("int");

                    b.Property<string>("Specification")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("WarningNum")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ConsumableInfo");
                });

            modelBuilder.Entity("Entity.ConsumableRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ConsumableId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Creator")
                        .HasColumnType("varchar(36)");

                    b.Property<int>("Num")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ConsumableRecord");
                });

            modelBuilder.Entity("Entity.DepartmentInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("LeaderId")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<string>("ParentId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("DepartmentInfo");
                });

            modelBuilder.Entity("Entity.MenuInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Href")
                        .HasColumnType("varchar(128)");

                    b.Property<string>("Icon")
                        .HasColumnType("varchar(32)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("ParentId")
                        .HasColumnType("varchar(36)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<string>("Target")
                        .HasColumnType("varchar(16)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.ToTable("MenuInfo");
                });

            modelBuilder.Entity("Entity.R_RoleInfo_MenuInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("MenuId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("RRoleInfoMenuInfo");
                });

            modelBuilder.Entity("Entity.R_UserInfo_RoleInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("Id");

                    b.ToTable("RUserInfoRoleInfo");
                });

            modelBuilder.Entity("Entity.RoleInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(36)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.ToTable("RoleInfo");
                });

            modelBuilder.Entity("Entity.UserInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("varchar(16)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeleteTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepartmentId")
                        .HasColumnType("varchar(36)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(32)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("varchar(32)");

                    b.Property<string>("PhoneNum")
                        .HasColumnType("varchar(16)");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("isAdmin")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
