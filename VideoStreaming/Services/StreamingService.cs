using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            IFileInfo file = _fileProvider.GetFileInfo(Path.Combine(PATH_TO_VIDEOS, name));

            return file.Exists ? file.CreateReadStream() : throw new FileNotFoundException();
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

        ~StreamingService()
        {
            _client?.Dispose();
        }
    }
}
