using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoStreaming.Services;

namespace VideoStreaming.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StreamingController : ControllerBase
    {
        private readonly IStreamingService _streamingService;

        public StreamingController(IStreamingService streamingService)
        {
            _streamingService = streamingService;
        }

        /// <summary>
        /// Stream video from server by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>mp4 stream</returns>
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public FileStreamResult GetVideoByName(string name)
        {
            Stream stream = _streamingService.GetVideoByName(name);

            return new FileStreamResult(stream, "video/mp4");
        }

        /// <summary>
        /// Stream random video from server
        /// </summary>
        /// <returns>mp4 stream</returns>
        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public FileStreamResult GetRandomVideo()
        {
            Stream stream = _streamingService.GetRandomVideo();

            return new FileStreamResult(stream, "video/mp4");
        }

        /// <summary>
        /// Stream video from at provided web uri
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>mp4 stream</returns>
        [HttpGet("web")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<FileStreamResult> GetStreamFromWeb([FromQuery] string uri)
        {
            Stream stream = await _streamingService.StreamFromWeb(uri).ConfigureAwait(false);

            return new FileStreamResult(stream, "video/mp4");
        }

    }
}
