using Api.Extensions;
using Api.SignalR;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAndConfigureSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddTransient<IStringEncoder, StringEncoder>();
builder.Services.ConfigureCorsPolicy();

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
app.UseCors("angular");
app.MapHub<StringHub>("hub/encoder");
app.Run();