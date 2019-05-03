using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoStreaming.Services
{
    public class StreamingService : IStreamingService
    {
        private readonly HttpClient _client;
        private readonly IFileProvider _fileProvider;

        private const string PATH_TO_VIDEOS = "/Assets/Videos/";

        public StreamingService(IFileProvider fileProvider)
        {
            _client = new HttpClient();
            _fileProvider = fileProvider;
        }

        public Stream GetVideoByName(string name)
        {
            IFileInfo file = _fileProvider.GetFileInfo(Path.Combine(PATH_TO_VIDEOS, name + ".mp4"));

            return file.Exists ? file.CreateReadStream() : throw new FileNotFoundException("Video not found");
        }

        public Stream GetRandomVideo()
        {
            IEnumerable<IFileInfo> videos = _fileProvider.GetDirectoryContents(PATH_TO_VIDEOS);

            if(!videos.Any())
                throw new FileNotFoundException("No videos to show");

            Random random = new Random();

            int randomNumber = random.Next(videos.Count() - 1);

            IFileInfo randomVideo = videos.ElementAt(randomNumber);

            return randomVideo.CreateReadStream();
        }

        public async Task<Stream> StreamFromWeb(string uri)
        {
            string[] uriParts = uri.Split('.');
            string format = uriParts[uriParts.Length - 1];

            if (uriParts.Length < 2 && string.Equals(format, "mp4", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("URI is invalid");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Head, uri);
            HttpResponseMessage response = _client.SendAsync(request).Result;

            return response.IsSuccessStatusCode ? await _client.GetStreamAsync(uri) : throw new HttpRequestException("Web resource doesn't exist");
        }

        ~StreamingService()
        {
            _client?.Dispose();
        }
    }
}
