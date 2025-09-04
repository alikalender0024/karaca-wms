using Karaca.Wms.Api.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Karaca.Wms.Api.Mapping;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// AutoMapper konfigürasyonu
builder.Services.AddAutoMapper(typeof(MappingProfile));

// DbContext ekleme
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();

// Controller’ları ekle
app.MapControllers();

app.Run();
