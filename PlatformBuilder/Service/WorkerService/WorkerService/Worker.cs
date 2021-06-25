using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken stoppingToken)
        {
            foreach (LibPcapLiveDevice dev in CaptureDeviceList.Instance)
            {
                this._logger.LogInformation(dev.Name +" "+ dev.Description);
                dev.Open();
                dev.OnPacketArrival += Device_OnPacketArrival;
                dev.OnCaptureStopped += Device_OnCaptureStopped;
                dev.StartCapture();

                Console.WriteLine("Press Enter for next device");
                Console.ReadLine();
                dev.StopCapture();
                dev.Close();
            
            }
        }

        private void Device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            this._logger.LogInformation("Device Stopped " + status.ToString());
        }

        void Device_OnPacketArrival(object s, PacketCapture e)
        {
            Packet packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
            if (packet == null)
                return;

            string data = string.Join(string.Empty, BitConverter.ToString(e.GetPacket().Data).Split('-'));
            DateTime time = e.Header.Timeval.Date;

            this._logger.LogInformation($"Recieve new packet with {e.Data.Length} bytes");
            Console.WriteLine($"[{e.Device.Description}]  {time.Hour}:{time.Minute}:{time.Second} {e.GetPacket().LinkLayerType.ToString()} ");
            Console.WriteLine($"{data}");
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
          
        }
    }
}
