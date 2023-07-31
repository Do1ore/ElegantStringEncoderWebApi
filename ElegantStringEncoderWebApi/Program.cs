using ElegantStringEncoderWebApi;
using ElegantStringEncoderWebApi.Extensions;
using ElegantStringEncoderWebApi.Services;
using ElegantStringEncoderWebApi.SignalR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAndConfigureSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddTransient<IStringEncoder, StringEncoder>();


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

app.MapHub<StringHub>("hub/encoder");
app.Run();