using System;
using System.IO;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Runtime;
using Crisp.Core.Types;

using Packet.Configuration;

namespace Packet.Server
{
    internal class PacketHttpServer : HttpServer
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IServerStartupSettingsProvider _serverStartupSettingsProvider;

        private readonly ICrispRuntimeFactory _crispRuntimeFactory;

        public PacketHttpServer(
            IConfigurationProvider configurationProvider,
            IServerStartupSettingsProvider serverStartupSettingsProvider,
            ICrispRuntimeFactory crispRuntimeFactory)
            : base(serverStartupSettingsProvider.GetSettings().Port)
        {
            _configurationProvider = configurationProvider;
            _serverStartupSettingsProvider = serverStartupSettingsProvider;
            _crispRuntimeFactory = crispRuntimeFactory;
        }

        private string GetUrlPath(string url)
        {
            var trimmed = url.TrimStart('/'); // Need to remove slash so it's not considered an absolute path.
            return Path.Combine(_serverStartupSettingsProvider.GetSettings().WebRoot, trimmed);
        }

        private ICrispRuntime GetRuntimeForFileAtUrl(string url)
        {
            return _crispRuntimeFactory.GetCrispRuntime(GetUrlPath(url));
        }

        private string GetMimeTypeForExtension(string extension)
        {
            return _configurationProvider.GetConfiguration().MimeTypeMappings[extension];
        }

        private bool IsInterpretedFileExtension(string extension)
        {
            return _configurationProvider.GetConfiguration().CrispFileExtensions.Contains(extension);
        }

        private void HandleError(HttpProcessor processor)
        {
            
        }

        public override void HandleGetRequest(HttpProcessor processor)
        {
            // Check if file should be interpreted.
            var extension = Path.GetExtension(processor.HttpUrl);
            if (IsInterpretedFileExtension(extension))
            {
                var runtime = GetRuntimeForFileAtUrl(processor.HttpUrl);
                var result = runtime.Run($"\"{processor.HttpUrl}\" \"GET\" nil");

                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // TODO: Error page.
                }
                else 
                {
                    var expanded = result.AsPair().Expand();
                    if (expanded.Count != 3)
                    {
                        // TODO: Error page.
                    }
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value), 
                        expanded[2].AsString().Value);
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                var path = GetUrlPath(processor.HttpUrl);
                if (File.Exists(GetUrlPath(processor.HttpUrl)))
                {
                    using (var fileStream = File.Open(path, FileMode.Open))
                    {
                        processor.WriteResponse(200, GetMimeTypeForExtension(extension));
                        processor.OutputStream.Flush();
                        fileStream.CopyTo(processor.OutputStream.BaseStream);
                        processor.OutputStream.BaseStream.Flush();
                    }
                }
            }
        }

        public override void HandlePostRequest(HttpProcessor processor, StreamReader inputStream)
        {
            // Get post data.
            var data = inputStream.ReadToEnd();
            var npqs = processor.HttpUrl.Split('?').First();
            var ext = npqs.Split('.').Last();

            if (ext == "csp")
            {
                var runtime = _crispRuntimeFactory.GetCrispRuntime("public-www/" + npqs);
                var result = runtime.Run($"\"{processor.HttpUrl}\" \"POST\" \"{data}\"");

                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // TODO: Error page.
                }
                else
                {
                    var expanded = result.AsPair().Expand();
                    processor.WriteResponse(Convert.ToInt32(expanded[1].AsNumeric().Value), expanded[2].AsString().Value);
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                // TODO: Serve static files or whatever.
            }
        }
    }
} 
