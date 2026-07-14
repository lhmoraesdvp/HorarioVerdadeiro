using System.Net;
using System.Net.Sockets;
using HorarioVerdadeiro.Api.Models;
using System.Diagnostics;

namespace HorarioVerdadeiro.Api.Services;

public class NtpSyncService
{
    public async Task<DateTime> GetNetworkTimeAsync()
    {
        const string ntpServer = "pool.ntp.org";

        var ntpData = new byte[48];
        ntpData[0] = 0x1B;

        var addresses = await Dns.GetHostEntryAsync(ntpServer);
        var ipEndPoint = new IPEndPoint(addresses.AddressList[0], 123);

        using var socket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Dgram,
            ProtocolType.Udp);

        socket.Connect(ipEndPoint);

        await socket.SendAsync(ntpData, SocketFlags.None);
        await socket.ReceiveAsync(ntpData, SocketFlags.None);

        const byte serverReplyTime = 40;

        ulong intPart = BitConverter.ToUInt32(
            ntpData.Skip(serverReplyTime).Take(4).Reverse().ToArray(), 0);

        ulong fractPart = BitConverter.ToUInt32(
            ntpData.Skip(serverReplyTime + 4).Take(4).Reverse().ToArray(), 0);

        var milliseconds =
            (intPart * 1000) +
            ((fractPart * 1000) / 0x100000000L);

        var networkDateTime = new DateTime(1900, 1, 1)
            .AddMilliseconds((long)milliseconds);

        return networkDateTime;
    }

    public async Task<double> GetOffsetMillisecondsAsync()
    {
        var ntpTime = await GetNetworkTimeAsync();
        var localTime = DateTime.UtcNow;

        return (ntpTime - localTime).TotalMilliseconds;
    }
}