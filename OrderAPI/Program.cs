using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderAPI.Data;
using OrderAPI.GmailSender;
using OrderAPI.Messaging;
using OrderAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

var optionBuilder = new DbContextOptionsBuilder<OrderAppDbContext>();
var dbContextOptions = optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).Options;
builder.Services.AddSingleton(dbContextOptions); // Register DbContextOptions<OrderAppDbContext> as singleton

builder.Services.AddDbContext<OrderAppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure();
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
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
app.UseCors();

app.MapControllers();

app.Run();

