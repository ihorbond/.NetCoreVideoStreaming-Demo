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
    public class VideosController : ControllerBase
    {
        private readonly IStreamingService _streamingService;

        public VideosController(IStreamingService streamingService)
        {
            _streamingService = streamingService;
        }

        /// <summary>
        /// Stream random video from server
        /// </summary>
        /// <returns>mp4 stream</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public FileStreamResult GetRandomVideo()
        {
            Stream stream = _streamingService.GetRandomVideo();

            return new FileStreamResult(stream, "video/mp4");
        }

        /// <summary>
        /// Stream video from server by name without extension
        /// </summary>
        /// <param name="name"></param>
        /// <returns>mp4 stream</returns>
        [HttpGet("local")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public FileStreamResult GetVideoByName([FromQuery] string name)
        {
            Stream stream = _streamingService.GetVideoByName(name);

            return new FileStreamResult(stream, "video/mp4");
        }

        /// <summary>
        /// Stream video at provided web uri. Accepted formats: *.mp4
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
