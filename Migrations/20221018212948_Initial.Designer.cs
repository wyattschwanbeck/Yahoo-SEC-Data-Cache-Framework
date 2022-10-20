﻿// <auto-generated />
using System;
using EdgarCacheFramework.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EdgarCacheFramework.Migrations
{
    [DbContext(typeof(FinancialStatements))]
    [Migration("20221018212948_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.30");

            modelBuilder.Entity("EdgarCacheFramework.Models.FinancialStatementInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float?>("Assets")
                        .HasColumnType("REAL");

                    b.Property<float?>("Cash")
                        .HasColumnType("REAL");

                    b.Property<long?>("CommonStockSharesOutstanding")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("CurrentAssets")
                        .HasColumnType("REAL");

                    b.Property<float?>("CurrentLiabilities")
                        .HasColumnType("REAL");

                    b.Property<float?>("DividendsPaid")
                        .HasColumnType("REAL");

                    b.Property<float?>("Equity")
                        .HasColumnType("REAL");

                    b.Property<float?>("FinancingCashFlows")
                        .HasColumnType("REAL");

                    b.Property<float?>("InvestingCashFlows")
                        .HasColumnType("REAL");

                    b.Property<float?>("Liabilities")
                        .HasColumnType("REAL");

                    b.Property<float?>("NetIncome")
                        .HasColumnType("REAL");

                    b.Property<float?>("OperatingCashFlows")
                        .HasColumnType("REAL");

                    b.Property<float?>("OperatingIncome")
                        .HasColumnType("REAL");

                    b.Property<float?>("PaymentsOfDebt")
                        .HasColumnType("REAL");

                    b.Property<long>("PeriodEnd")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PeriodStart")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("ProceedsFromIssuanceOfDebt")
                        .HasColumnType("REAL");

                    b.Property<long>("PullTime")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("ResearchAndDevelopmentExpense")
                        .HasColumnType("REAL");

                    b.Property<float?>("RetainedEarnings")
                        .HasColumnType("REAL");

                    b.Property<float?>("Revenue")
                        .HasColumnType("REAL");

                    b.Property<float?>("SellingGeneralAndAdministrativeExpense")
                        .HasColumnType("REAL");

                    b.Property<string>("Ticker")
                        .HasColumnType("TEXT");

                    b.Property<string>("xml")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FinancialStatementsInstances");
                });
#pragma warning restore 612, 618
        }
    }
}
