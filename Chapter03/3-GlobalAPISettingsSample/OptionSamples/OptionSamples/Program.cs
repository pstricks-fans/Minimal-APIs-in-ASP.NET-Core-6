using Microsoft.Extensions.Options;
using OptionSamples;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<OptionBasic>(builder.Configuration.GetSection("OptionBasic"));
builder.Services.Configure<OptionMonitor>(builder.Configuration.GetSection("OptionMonitor"));
builder.Services.Configure<OptionSnapshot>(builder.Configuration.GetSection("OptionSnapshot"));
builder.Services.Configure<OptionCustomName>("CustomName1", builder.Configuration.GetSection("CustomName1"));
builder.Services.Configure<OptionCustomName>("CustomName2", builder.Configuration.GetSection("CustomName2"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/read/options", (IOptions<OptionBasic> optionsBasic,
    IOptionsMonitor<OptionMonitor> optionsMonitor,
    IOptionsSnapshot<OptionSnapshot> optionsSnapshot,
    IOptionsFactory<OptionCustomName> optionsFactory) =>
{

    return Results.Ok(new
    {
        Basic = optionsBasic.Value,
        Monitor = optionsMonitor.CurrentValue,
        Snapshot = optionsSnapshot.Value,
        Custom1 = optionsFactory.Create("CustomName1"),
        Custom2 = optionsFactory.Create("CustomName2")
    });
})
.WithName("ReadOptions");

app.Run();

namespace OptionSamples
{
    public class OptionBasic
    {
        public string? Value { get; init; }
    }

    public class OptionSnapshot
    {
        public string? Value { get; init; }
    }

    public class OptionMonitor
    {
        public string? Value { get; init; }
    }

    public class OptionCustomName
    {
        public string? Value { get; init; }
    }
}