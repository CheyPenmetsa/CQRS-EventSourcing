using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Lease.Cmd.Api.Commands;
using Lease.Cmd.Domain.Aggregates;
using Lease.Cmd.Infrastructure.Dispatchers;
using Lease.Cmd.Infrastructure.Handlers;
using Lease.Cmd.Infrastructure.Producers;
using Lease.Cmd.Infrastructure.Repositories;
using Lease.Cmd.Infrastructure.Settings;
using Lease.Cmd.Infrastructure.Stores;
using Lease.Common.Events;
using MongoDB.Bson.Serialization;

var builder = WebApplication.CreateBuilder(args);

//MongoDb cannot apply serialization to abstract classes
BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<LeaseCreatedEvent>();
BsonClassMap.RegisterClassMap<LeaseSentEvent>();
BsonClassMap.RegisterClassMap<LeaseSignedEvent>();
BsonClassMap.RegisterClassMap<LeaseEditedEvent>();
BsonClassMap.RegisterClassMap<LeaseCancelledEvent>();

// Add services to the container.
// This will load the MongoDb settings
builder.Services.Configure<EventStoreDatabaseSettings>(builder.Configuration.GetSection(nameof(EventStoreDatabaseSettings)));
// This will load kafka setting for producerconfig
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));

// Creates unique instance for each HTTP request
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, KafkaEventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<LeaseAggregate>, LeaseAggregateEventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//Register command handler methods and dispatcher
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterCommandHandler<CreateLeaseCommand>(commandHandler.HandleAsync);
dispatcher.RegisterCommandHandler<SendLeaseCommand>(commandHandler.HandleAsync);
dispatcher.RegisterCommandHandler<SignLeaseCommand>(commandHandler.HandleAsync); 
dispatcher.RegisterCommandHandler<EditLeaseCommand>(commandHandler.HandleAsync);
dispatcher.RegisterCommandHandler<CancelLeaseCommand>(commandHandler.HandleAsync);
dispatcher.RegisterCommandHandler<ReplayEventsCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
