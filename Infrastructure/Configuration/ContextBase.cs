﻿using Entities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration;

public class ContextBase : IdentityDbContext<ApplicationUser>
{
    public ContextBase(DbContextOptions<ContextBase> options) : base(options)
    {
        _connectionString = "Data Source=ENIAC\\SQLEXPRESS;Initial Catalog=SistemaLanches;Integrated Security=True;TrustServerCertificate=True";

    }

    public DbSet<Message> Message { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    private string _connectionString; 


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

        base.OnModelCreating(builder);
    }


}