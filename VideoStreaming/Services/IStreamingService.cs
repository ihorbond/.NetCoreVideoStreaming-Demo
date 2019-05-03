using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoStreaming.Services
{
    public interface IStreamingService
    {
        Stream GetVideoByName(string name);

    }
}
