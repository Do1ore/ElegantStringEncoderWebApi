using Api.Extensions;
using Application.SignalR;

var builder = WebApplication.CreateBuilder(args);

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
//Minimal api registry
app.RegisterEndpointDefinitions();

app.UseAuthorization();

app.MapControllers();
app.UseCors("angular");
app.MapHub<StringHub>("hub/encoder");
app.Run();