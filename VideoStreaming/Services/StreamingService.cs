using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoStreaming.Services
{
    public class StreamingService : IStreamingService
    {
        public Task<Stream> GetVideoByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
