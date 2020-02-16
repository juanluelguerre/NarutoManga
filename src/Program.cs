using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace NarutoManga
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            await DownloadImages();

            CreateVolumesZip();
            await Task.CompletedTask;
        }       

        static void CreateVolumesZip()
        {
            Console.WriteLine("Zipping...");

            var maxVolume = 1;
            for (int volume = 1; volume <= 700; volume++)
            {                
                var strVolume = $"{volume}".PadLeft(3, '0');
                var files = Directory.GetFiles("/Users/juanlu/Desktop/Naruto Shippuden Manga/",
                    $"Naruto Shippuden {strVolume} *.jpg");

                Console.WriteLine($"Zipping... {volume}/{maxVolume}...");

                CreateZipFile($"Naruto Shippuden {strVolume}.cbz", files);
            }
        }

        static void CreateZipFile(string fileName, IEnumerable<string> files)
        {
            // Create and open a new ZIP file
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
            }
            // Dispose of the object when we are done
            zip.Dispose();
        }

        static async Task DownloadImages()
        {
            Console.WriteLine("Downloading...");

            var httpClient = new HttpClient();

            var maxVolume = 700;
            // for (int volume = 1; volume <= maxVolume; volume++)
            // for (int volume = 643; volume <= maxVolume; volume++)
            // for (int volume = 651; volume <= maxVolume; volume++)
            // for (int volume = 652; volume <= maxVolume; volume++)
            for (int volume = 1; volume <= maxVolume; volume++)
            {
                Console.WriteLine($"Downloading {volume}/{maxVolume}...");

                for (int page = 1; page <= 100; page++)
                {
                    string url1 = $"https://img.mangaenlinea.com/sc/{volume}/{page}.jpg";
                    string url2 = $"https://img.mangaenlinea.com/1/{volume}PB/{page}.jpg";
                    string url3 = $"https://img.mangaenlinea.com/1/{volume}v2p/{page}.jpg";
                    string url4 = $"https://img.mangaenlinea.com/1/{volume}/{page}.jpg";
                    string url5 = $"https://img.mangaenlinea.com/1/{volume}bw/{page}.jpg";
                    string url6 = $"https://img.mangaenlinea.com/1/{volume}nfnf/{page}.jpg";
                    string url7 = $"https://img.mangaenlinea.com/1/{volume}v3/{page}.jpg";
                    string url8 = $"https://img.mangaenlinea.com/1/{volume}v2/{page}.jpg";
                    string url9 = $"https://img.mangaenlinea.com/1/{volume}ak/{page}.jpg";

                    var img = await GetImage(httpClient, url1);
                    if (img == null)
                        img = await GetImage(httpClient, url2);
                    if (img == null)
                        img = await GetImage(httpClient, url3);
                    if (img == null)
                        img = await GetImage(httpClient, url4);
                    if (img == null)
                        img = await GetImage(httpClient, url5);
                    if (img == null)
                        img = await GetImage(httpClient, url6);
                    if (img == null)
                        img = await GetImage(httpClient, url7);
                    if (img == null)
                        img = await GetImage(httpClient, url8);
                    if (img == null)
                        img = await GetImage(httpClient, url9);

                    if (img != null)
                    {
                        var strVolume = $"{volume}".PadLeft(3,'0');
                        var strPage = $"{page}".PadLeft(2,'0');

                        var path = $"Naruto Shippuden {strVolume} {strPage}.jpg";
                        await File.WriteAllBytesAsync(path, img);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("Pulse INTRO para finalizar...");
            Console.ReadLine();
        }

        static async Task<byte[]> GetImage(HttpClient httpClient, string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
