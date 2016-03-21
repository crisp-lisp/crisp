using System;
using System.IO;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Types;

namespace Packet.Server
{
    internal class PacketHttpServer : HttpServer
    {
        private readonly IServerStartupSettingsProvider _serverStartupSettingsProvider;

        private readonly ICrispRuntimeFactory _crispRuntimeFactory;

        public PacketHttpServer(
            IServerStartupSettingsProvider serverStartupSettingsProvider,
            ICrispRuntimeFactory crispRuntimeFactory)
            : base(serverStartupSettingsProvider.GetSettings().Port)
        {
            _serverStartupSettingsProvider = serverStartupSettingsProvider;
            _crispRuntimeFactory = crispRuntimeFactory;
        }

        public override void HandleGetRequest(HttpProcessor processor)
        {
            if (processor.HttpUrl.EndsWith(".csp"))
            {
                var runtime = _crispRuntimeFactory.GetCrispRuntime("public-www/" + processor.HttpUrl);
                var result = runtime.Run($"\"{processor.HttpUrl}\" \"GET\" nil");

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
