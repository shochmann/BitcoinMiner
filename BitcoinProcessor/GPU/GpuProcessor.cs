using ILGPU;
using System.Reflection.PortableExecutable;

namespace BitcoinProcessor.GPU
{
    public static class GpuProcessor
    {
        public static string ProcessHeaderOnGpu(List<string> headers)
        {
            GpuSha256.ValidHeader = "";
            GpuSha256.CompletedGpus = 0;

            using var context = Context.CreateDefault();
            var totalDevices = context.Devices.Where(x => x.Name.Contains("NVIDIA")).ToList();
            
            for(var x = 0; x < totalDevices.Count(); x++)
            {
                var index = x;
                Thread thread = new Thread(delegate ()
                {
                    var device = totalDevices[index];
                    using var accelerator = device.CreateAccelerator(context);
                    GpuSha256.ProcessGpuSha256(accelerator, headers,
                        index, totalDevices.Count(), (int)Math.Floor(device.MaxNumThreads * .85));
                });
                thread.Start();
            }

            while(GpuSha256.CompletedGpus < context.Devices.Where(x => x.Name.Contains("NVIDIA")).Count()
                && string.IsNullOrEmpty(GpuSha256.ValidHeader))
            {
                Thread.Sleep(10000);
            }

            return GpuSha256.ValidHeader;
        }
    }
}
