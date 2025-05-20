using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ReservationSystem.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Web projesinin appsettings.json dosyasını okuyacağımız klasör yolunu oluştur.
        var basePath = Directory.GetCurrentDirectory();

        // ConfigurationBuilder ile JSON'u yükle
        var configuration = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

        // 3) Connection string'i oku
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // 4) OptionsBuilder ile SQL Server'ı ayarla
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
            
        // 5) Hazırlanmış options ile DbContext örneğini döndür
        return new ApplicationDbContext(optionsBuilder.Options);
    }

}
