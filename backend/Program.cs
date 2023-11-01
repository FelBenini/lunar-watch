using coffeebeans.backend.Infra;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnvConfig.load(dotenv);
string connectionString = "server=" + Environment.GetEnvironmentVariable("DATABASE_HOST") + ";database=" + Environment.GetEnvironmentVariable("DATABASE_NAME") + ";user=" + Environment.GetEnvironmentVariable("DATABASE_USER") + ";password=" + Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine(connectionString);

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
