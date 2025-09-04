using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Karaca.Wms.Api.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Migration çalıştırdığın dizini al
            var currentDirectory = Directory.GetCurrentDirectory();

            // appsettings.Development.json yolunu çöz
            // Projenin tam yolunu bul
            var projectPath = Path.Combine(currentDirectory, "src", "Karaca.Wms.Api");

            if (!File.Exists(Path.Combine(projectPath, "appsettings.Development.json")))
            {
                // Eğer dosya yoksa fallback: belki migration src içinde çalıştırıldı
                projectPath = currentDirectory;
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Connection string bulunamadı! Kontrol et: {Path.Combine(projectPath, "appsettings.Development.json")}");
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
