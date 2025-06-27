using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using FraudSys.Services;
using FraudSys.Repositories;
using FraudSys.Services.Interfaces;
using FraudSys.Repositories.Interfaces;
using FraudSys.Validators;
using FraudSys.Middleware;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Configuração do AWS DynamoDB
var awsOptions = new AWSOptions
{
    Region = RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"]),
    Credentials = new BasicAWSCredentials(
        builder.Configuration["AWS:AccessKey"],
        builder.Configuration["AWS:SecretKey"])
};
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<DynamoDBContext>();

builder.Services.AddScoped<ServicoDeMensagens>();
builder.Services.AddScoped<IContaService, ContaService>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<ILimiteService, LimiteService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IContaRepository, ContaRepository>();
builder.Services.AddScoped<AtualizarSaldoDTOValidator>();
builder.Services.AddScoped<ContaInDTOValidator>();
builder.Services.AddScoped<LimiteDTOValidator>();
builder.Services.AddScoped<TransacaoInDTOValidator>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorExceptionHandlingMiddleware>();

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
