// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CommandCenter.Utilities
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.IO;

    /// <summary>
    /// Logging webhook requests for debug purposes.
    /// </summary>
    public class WebhookRequestLogger
    {
        private readonly RequestDelegate next;
        private readonly ILogger<WebhookRequestLogger> logger;
        private readonly RecyclableMemoryStreamManager memoryStreamManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookRequestLogger"/> class.
        /// </summary>
        /// <param name="next">Next request delegate.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public WebhookRequestLogger(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger<WebhookRequestLogger>();
            memoryStreamManager = new RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// Capture the request.
        /// </summary>
        /// <param name="context">Http Context.</param>
        /// <returns>void.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Path.Value.ToUpperInvariant().Contains("WEBHOOK", StringComparison.InvariantCulture))
            {
                context.Request.EnableBuffering();

                await using var requestStreamCopy = memoryStreamManager.GetStream();
                await context.Request.Body.CopyToAsync(requestStreamCopy);
                context.Request.Body.Position = 0;
                context.Request.Body.Seek(0, SeekOrigin.Begin);

                logger.LogInformation($"Webhook raw request {ReadStream(requestStreamCopy)}");
            }
        }

        private static string ReadStream(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(
                    readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            }
            while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}