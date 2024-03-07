using System.Security.Cryptography;

namespace Momiji{
    class Program
    {
        private const int N = 20_000_000;

        static List<string> AsyncComputeSHA256(){
            List<string> texts = new();
            List<string> results = new();
            for(int i = 0; i < N; i++){
                texts.Add($"{i:00000000}");
                results.Add("");
            }
            Console.WriteLine("領域確保");

            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = -1
            };
            Parallel.For(0, N, i => {
                results[i] = ComputeSHA256Async(texts[i]);
            });
            
            return results;
        }

        static string ComputeSHA256Async(string text){
            string hashString;
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(text);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return hashString;
        }

        static void Main()
        {
            Console.WriteLine("開始");
            DateTime startTime = DateTime.Now;
            List<string> hashes = AsyncComputeSHA256();
            DateTime endTime = DateTime.Now;
            TimeSpan time = endTime.Subtract(startTime);
            Console.WriteLine($"完了 {time}");
            Console.WriteLine($"{hashes[N-1]}");
        }
    }
}
