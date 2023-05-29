using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderAPI.Data;
using OrderAPI.Messaging;
using OrderAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

var optionBuilder = new DbContextOptionsBuilder<OrderAppDbContext>();
var dbContextOptions = optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).Options;
builder.Services.AddSingleton(dbContextOptions); // Register DbContextOptions<OrderAppDbContext> as singleton

builder.Services.AddDbContext<OrderAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Configure the connection string
});

builder.Services.AddHostedService<RabbitMqConsumer>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

