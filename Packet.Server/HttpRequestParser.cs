﻿using System;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public abstract class HttpRequestParser : IHttpRequestParser
    {
        public IHttpRequestParser Successor { get; }

        protected HttpRequestParser(IHttpRequestParser successor)
        {
            Successor = successor;
        }

        protected static bool ValidateUrl(string url)
        {
            // Validate URL, must be relative.
            Uri validUrl;
            return Uri.TryCreate(url, UriKind.Relative, out validUrl);
        }

        protected abstract IHttpRequest AttemptParse(byte[] request);

        public IHttpRequest Parse(byte[] request)
        {
            return AttemptParse(request) ?? Successor?.Parse(request);
        }
    }
}