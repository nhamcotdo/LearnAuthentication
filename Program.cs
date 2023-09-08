// using LearnAuthentication.Configuarations.Filters;
using LearnAuthentication.Models;
using LearnAuthentication.Services;
using workspace.LearnAuthentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // options.Filters.Add(new MyFilter()); //Global Filter
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthModel, AuthModel>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
