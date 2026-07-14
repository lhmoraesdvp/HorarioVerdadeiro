namespace HorarioVerdadeiro.Api.Models;

public class NtpResult
{
    public DateTime NetworkTimeUtc { get; set; }

    public double OffsetMilliseconds { get; set; }

    public double LatencyMilliseconds { get; set; }

    public double AccuracyMilliseconds { get; set; }

    public DateTime SyncTimeUtc { get; set; }

    public string Source { get; set; } = "";
}