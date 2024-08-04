using ILGPU;
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
            foreach (var device in context.Devices.Where(x => x.Name.Contains("NVIDIA")))
            {
                Thread thread = new Thread(delegate ()
                {
                    using var accelerator = device.CreateAccelerator(context);
                    GpuSha256.ProcessGpuSha256(accelerator, headerMinusN, 0, 0);
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
