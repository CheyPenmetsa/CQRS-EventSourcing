using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using CQRS.Core.Infrastructure;
using Lease.Common.Events;
using Lease.Query.Api.Queries;
using Lease.Query.Domain.Entities;
using Lease.Query.Domain.Repositories;
using Lease.Query.Infrastructure.Consumers;
using Lease.Query.Infrastructure.DataAccess;
using Lease.Query.Infrastructure.Dispatchers;
using Lease.Query.Infrastructure.Handlers;
using Lease.Query.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<LeaseCreatedEvent>();
BsonClassMap.RegisterClassMap<LeaseSentEvent>();
BsonClassMap.RegisterClassMap<LeaseSignedEvent>();
BsonClassMap.RegisterClassMap<LeaseEditedEvent>();
BsonClassMap.RegisterClassMap<LeaseCancelledEvent>();

// Add services to the container.
// See below we are using Lazyloading navigation properties
Action<DbContextOptionsBuilder> configureDbContext = 
    (opt => opt.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("ReadDbServer")));
builder.Services.AddDbContext<ReadDbContext>(configureDbContext);
builder.Services.AddSingleton<ReadDbContextFactory>(new ReadDbContextFactory(configureDbContext));

// Create database and tables using codefirst approach
var dbContext = builder.Services.BuildServiceProvider().GetRequiredService<ReadDbContext>();
dbContext.Database.EnsureCreated();

builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));

builder.Services.AddScoped<ILeaseRepository, LeaseRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();
builder.Services.AddScoped<IEventConsumerHandler, EventConsumerHandler>();
builder.Services.AddScoped<IEventConsumer, KafkaEventConsumer>();

//Register query handler methods and dispatcher
var queryHandler = builder.Services.BuildServiceProvider().GetRequiredService<IQueryHandler>();
var dispatcher = new QueryDispatchers();
dispatcher.RegisterQueryHandler<FindAllLeasesQuery>(queryHandler.HandleAsync);
dispatcher.RegisterQueryHandler<FindLeaseByIdQuery>(queryHandler.HandleAsync);
builder.Services.AddSingleton<IQueryDispatcher<LeaseEntity>>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddHostedService<KafkaTopicConsumerHostedService>();
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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
