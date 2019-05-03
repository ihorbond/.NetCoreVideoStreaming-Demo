﻿using System;
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

        /// <summary>
        /// Stream video from server by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>mp4 stream</returns>
        [HttpGet("{name}")]
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
        public async Task<FileStreamResult> GetStreamFromWeb([FromQuery] string uri)
        {
            Stream stream = await _streamingService.StreamFromWeb(uri).ConfigureAwait(false);

            return new FileStreamResult(stream, "video/mp4");
        }

    }
}
