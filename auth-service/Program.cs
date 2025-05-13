using Microsoft.EntityFrameworkCore;
using AuthService.Data;
using AuthService.Controller;
using AuthService.Services;



var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddDotNetEnv();

var config = builder.Configuration;

var connectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
 options.UseInMemoryDatabase("TestDb"));
//options.UseSqlServer(connectionString)); Easier to use in memory db Instead of adding test data to actual database at the moment


builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<TokenProvider>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auth Service API",
        Version = "v1",
        Description = "An API for authentication services"
    });
});

builder.WebHost.UseUrls("http://localhost:5000");
var app = builder.Build();


app.MapControllers();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");

}






app.Run();