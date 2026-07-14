using HorarioVerdadeiro.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<NtpSyncService>();

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/api/ntp", async (NtpSyncService ntp) =>
{
    var ntpTime = await ntp.GetNetworkTimeAsync();
    var offset = await ntp.GetOffsetMillisecondsAsync();

    return Results.Ok(new
    {
        NetworkTimeUtc = ntpTime,
        OffsetMilliseconds = offset
    });
});

app.Run();