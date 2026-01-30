using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("speaker", c => c.BaseAddress = new Uri("http://localhost:8001/"));
builder.Services.AddHttpClient("light", c => c.BaseAddress = new Uri("http://localhost:8002/"));
builder.Services.AddHttpClient("curtains", c => c.BaseAddress = new Uri("http://localhost:8003/"));


builder.Services.AddScoped<SmartAppMain.Services.IoTFacade>();

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
   .WriteTo.File("log/smartAppLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
