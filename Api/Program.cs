using Api.Extensions;
using Api.SignalR;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAndConfigureSwaggerGen();
builder.Services.ConfigureCustomServices();
    
builder.Services.AddSignalR();

builder.Services.ConfigureCorsPolicy();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Minimal api registration
app.RegisterEndpointDefinitions();

app.UseAuthorization();

app.MapControllers();
app.UseCors("angular");
app.MapHub<StringHub>("hub/encoder");
app.Run();