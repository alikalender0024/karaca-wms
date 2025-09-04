using Karaca.Wms.Api.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Karaca.Wms.Api.Mapping;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Serilog'u yapılandır
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MappingProfile));

// DbContext ekleme
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS policy ekle
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Veritabanını başlat ve seed işlemini yap
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS middleware’i HTTPS redirection’dan önce ekle
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();

// Controller’ları ekle
app.MapControllers();

app.Run();
