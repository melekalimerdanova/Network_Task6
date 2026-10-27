using Network_Task6.Services;
using System.Text;

namespace Network_Task6
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            ClimateManager manager = new ClimateManager();

            while (true)
            {
                Console.Write("Zəhmət olmasa şəhər daxil edin (Çıxış: 'exit'): ");
                string userInput = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(userInput)) continue;
                if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                try
                {
                    var resultInfo = await manager.FetchClimateDataAsync(userInput);
                    if (resultInfo != null)
                    {
                        Console.WriteLine(resultInfo);
                    }
                    else
                    {
                        Console.WriteLine("Daxil edilən şəhər üzrə məlumat tapılmadı.\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Xəta baş verdi: {ex.Message}\n");
                }
            }
        }
    }
}