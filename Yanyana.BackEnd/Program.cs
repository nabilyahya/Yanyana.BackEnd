using Yanyana.BackEnd.Api;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

// Add DbContext with your connection string
// builder.Services.AddDbContext<YanDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRoleManager, UserRoleManager>();

// Configure Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware and HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run the application
app.Run();

// Declare an empty public partial class to allow extension in other files
public partial class Program { }
