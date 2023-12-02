using System.Text;
using System.Text.Json.Serialization;
using lunarwatch.backend.Infra;
using lunarwatch.backend.Models;
using lunarwatch.backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Processors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnvConfig.load(dotenv);
string connectionString = "server=" + Environment.GetEnvironmentVariable("DATABASE_HOST") + ";database=" + Environment.GetEnvironmentVariable("DATABASE_NAME") + ";user=" + Environment.GetEnvironmentVariable("DATABASE_USER") + ";password=" + Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type=ReferenceType.SecurityScheme,
          Id="Bearer"
        }
      },
      new string[]{}
    }
  });
}
);

Console.WriteLine(connectionString);

builder.Services.AddImageSharp(options =>
{
  options.OnParseCommandsAsync = c =>
  {
    if (c.Commands.Count == 0)
    {
      return Task.CompletedTask;
    }

    uint width = c.Parser.ParseValue<uint>(
    c.Commands.GetValueOrDefault(ResizeWebProcessor.Width),
    c.Culture);

    List<uint> allowedSizes = new List<uint> {200, 400, 600, 1200, 2400};
    if (!allowedSizes.Any(x => x == width))
    {
      c.Commands.Remove(ResizeWebProcessor.Width);
    }
    c.Commands.Remove(ResizeWebProcessor.Height);
    return Task.CompletedTask;
  };
});

builder.Services.AddDbContext<DatabaseContext>(options =>
  options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
  .AddEntityFrameworkStores<DatabaseContext>()
  .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<IdentityRole<int>>>();

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.SaveToken = true;
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuer = false,
    ValidateAudience = false,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")))
  };
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddScoped<ProfileService, ProfileService>();
builder.Services.AddScoped<ImageUploaderService, ImageUploaderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseImageSharp();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
