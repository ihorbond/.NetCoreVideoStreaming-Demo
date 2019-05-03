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

        public StreamingService(IFileProvider fileProvider)
        {
            _client = new HttpClient();
            _fileProvider = fileProvider;
        }

        public Stream GetVideoByName(string name)
        {
            IFileInfo file = _fileProvider.GetFileInfo($"/Assets/Videos/{name}");

            return file.Exists ? file.CreateReadStream() : throw new FileNotFoundException();
        }

        ~StreamingService()
        {
            _client?.Dispose();
        }
    }
}
