using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace BitcoinProcessor.GPU
{
    public static class GpuProcessor
    {
        public static string ProcessHeaderOnGpu(string headerMinusN, List<uint> nonceList)
        {
            GpuSha256.ValidHeader = "";
            GpuSha256.CompletedGpus = 0;
            GpuSha256.NonceList = nonceList.ToArray();

            using var context = Context.CreateDefault();
            var totalDevices = context.Devices.Where(x => x.Name.Contains("NVIDIA")).ToList();
            
            for(var x = 0; x < totalDevices.Count(); x++)
            {
                var index = x;
                Thread thread = new Thread(delegate ()
                {
                    var device = totalDevices[index];
                    using var accelerator = device.CreateAccelerator(context);
                    GpuSha256.ProcessGpuSha256(accelerator, headerMinusN,
                        index, totalDevices.Count(), (int)Math.Floor(device.MaxNumThreads * .85));
                });
                thread.Start();
            }

            while(GpuSha256.CompletedGpus < context.Devices.Where(x => x.Name.Contains("NVIDIA")).Count())
            {
                Thread.Sleep(10000);
            }

            return GpuSha256.ValidHeader;
        }
    }
}
