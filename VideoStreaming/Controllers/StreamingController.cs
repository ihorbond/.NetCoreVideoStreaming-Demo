using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamingController : ControllerBase
    {
        private readonly IStreamingService _streamingService;

        public StreamingController(IStreamingService streamingService)
        {
            _streamingService = streamingService;
        }


        [HttpGet("{name}")]
        public FileStreamResult GetVideoByName(string name)
        {
            Stream stream = _streamingService.GetVideoByName(name);

            return new FileStreamResult(stream, "video/mp4");
        }

        [HttpGet("random")]
        public FileStreamResult GetRandomVideo()
        {
            Stream stream = _streamingService.GetRandomVideo();

            return new FileStreamResult(stream, "video/mp4");
        }

    }
}
