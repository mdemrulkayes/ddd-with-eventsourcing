using Application;
using Infrastracture;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();

//builder.Services.AddScoped<ServiceFactory>(ctx => ctx.GetRequiredService);
//builder.Services.AddScoped<IMediator, Mediator>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddEvents(builder.Configuration["EventStore:ConnectionString"]);
builder.Services.AddInfrastructure();

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
