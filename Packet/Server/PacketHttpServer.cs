using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using Crisp.Core;
using Crisp.Core.Types;
using Crisp.Visualization;

using Packet.Configuration;

namespace Packet.Server
{
    /// <summary>
    /// Represents a Packet HTTP/1.0 server.
    /// </summary>
    internal class PacketHttpServer : HttpServer
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly IServerSettingsProvider _serverSettingsProvider;

        private readonly ICrispRuntimeFactory _crispRuntimeFactory;

        private readonly ISymbolicExpressionSerializer _symbolicExpressionSerializer;
        
        /// <summary>
        /// Initializes a new instance of a Packet HTTP/1.0 server.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider for the application.</param>
        /// <param name="serverSettingsProvider">The startup settings provider for the server.</param>
        /// <param name="crispRuntimeFactory">The factory to use to create Crisp runtime instances.</param>
        /// <param name="symbolicExpressionSerializer">The serializer to use to display debug output.</param>
        /// <param name="logger">The logger to use for logging server events.</param>
        public PacketHttpServer(
            IConfigurationProvider configurationProvider,
            IServerSettingsProvider serverSettingsProvider,
            ICrispRuntimeFactory crispRuntimeFactory,
            ISymbolicExpressionSerializer symbolicExpressionSerializer,
            ILogger logger)
            : base(configurationProvider.Get().BindingIpAddress,
                  serverSettingsProvider.Get().Port, logger)
        {
            _configurationProvider = configurationProvider;
            _serverSettingsProvider = serverSettingsProvider;
            _crispRuntimeFactory = crispRuntimeFactory;
            _symbolicExpressionSerializer = symbolicExpressionSerializer;
        }

        /// <summary>
        /// Creates a physical path from the given URL relative to the web root.
        /// </summary>
        /// <param name="url">The URL of the requested resource.</param>
        /// <returns></returns>
        private string GetUrlPath(string url)
        {
            // Need to remove slash so it's not considered an absolute path.
            var trimmed = url.TrimStart('/');
            if (trimmed.Contains('?'))
            {
                trimmed = trimmed.Split('?').First(); // Remove query string.
            }

            // Compute real path.
            var path = Path.Combine(_serverSettingsProvider.Get().WebRoot, trimmed);

            // If real path is a directory.
            if (Directory.Exists(path)) 
            {
                // Get any configured index pages.
                var files = Directory.GetFiles(path)
                    .Select(Path.GetFileName)
                    .Where(f => _configurationProvider.Get().DirectoryIndices.Contains(f))
                    .ToArray(); 
                if (files.Any())
                {
                    // Pass back path to configured index page.
                    path = Path.Combine(path, files.First()); 
                }
            }

            return path;
        }
        
        /// <summary>
        /// Gets the MIME type for the given file extension.
        /// </summary>
        /// <param name="extension">The file extension to return the MIME type for.</param>
        /// <returns></returns>
        private string GetMimeTypeForExtension(string extension)
        {
            string value;
            return _configurationProvider.Get().MimeTypeMappings.TryGetValue(extension, out value) ?
                value : "application/octet-stream"; // Default to this MIME type.
        }

        /// <summary>
        /// Returns true if the given file extension if configured to be interpreted.
        /// </summary>
        /// <param name="extension">The file extension to check, including the leading dot.</param>
        /// <returns></returns>
        private bool IsInterpretedFileExtension(string extension)
        {
            return _configurationProvider.Get().CrispFileExtensions.Contains(extension);
        }

        /// <summary>
        /// Returns true if the given path matches a 'do not serve' rule. Otherwise returns false.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns></returns>
        private bool IsForbiddenPath(string path)
        {
            return _configurationProvider.Get().DoNotServePatterns.Any(p => Regex.IsMatch(path, p));
        }

        /// <summary>
        /// Serves the default 404 error page.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        private static void ServeDefaultNotFoundPage(HttpProcessor processor)
        {
            processor.WriteResponse(404, "text/html");
            processor.OutputStream.Write(Properties.Resources.DefaultNotFoundErrorPage);
        }

        /// <summary>
        /// Serves a static file.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        private void ServeStaticFile(HttpProcessor processor)
        {
            // Get physical path and extension.
            var path = GetUrlPath(processor.HttpUrl);
            var extension = Path.GetExtension(path);

            // Write file to output.
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                processor.WriteResponse(200, GetMimeTypeForExtension(extension));
                processor.OutputStream.Flush();
                fileStream.CopyTo(processor.OutputStream.BaseStream);
                processor.OutputStream.BaseStream.Flush();
            }
        }

        /// <summary>
        /// Converts a dictionary of HTTP headers to a serialized Crisp name-value collection.
        /// </summary>
        /// <param name="headers">The header dictionary to convert.</param>
        /// <returns></returns>
        private static string TransformHeadersForCrisp(Dictionary<string, string> headers)
        {
            var serializer = new LispSerializer(); // We need to serialize to valid Crisp.
            return serializer.Serialize(
                headers.Select(header => new Pair(new StringAtom(header.Key), new StringAtom(header.Value)))
                    .Cast<SymbolicExpression>()
                    .ToArray()
                    .ToProperList());
        }

        /// <summary>
        /// Converts a name-value collection of HTTP headers passed back by a Crisp webpage 
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private static Dictionary<string, string> TransformHeadersForPacket(SymbolicExpression headers)
        {
            if (headers.Type == SymbolicExpressionType.Nil)
            {
                return new Dictionary<string, string>();
            }

            var expanded = headers.AsPair().Expand();
            return expanded.ToDictionary(p => p.AsPair().Head.AsString().Value, p => p.AsPair().Tail.AsString().Value);
        }

        /// <summary>
        /// Serves the default 500 error page.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        private static void ServeDefaultInternalServerErrorPage(HttpProcessor processor)
        {
            processor.WriteResponse(500, "text/html");
            processor.OutputStream.Write(Properties.Resources.DefaultInternalServerErrorPage);
        }

        private static bool IsNameValueCollection(SymbolicExpression expression)
        {
            // Check we've got a pair.
            var casted = expression as Pair;
            if (casted == null)
            {
                return false;
            }

            // Check we only have name-value collection entries in list.
            return casted.Expand().All(e =>
            {
                var entry = e as Pair;
                return entry != null
                       && entry.Head.Type == SymbolicExpressionType.String
                       && entry.Tail.Type == SymbolicExpressionType.String;
            });
        }

        private static bool IsValidResult(IList<SymbolicExpression> expandedResult)
        {
            return expandedResult.Count == 4
                   && expandedResult[0].Type == SymbolicExpressionType.String
                   && expandedResult[1].Type == SymbolicExpressionType.Numeric
                   && expandedResult[2].Type == SymbolicExpressionType.String
                   && (IsNameValueCollection(expandedResult[3])
                       || expandedResult[3].Type == SymbolicExpressionType.Nil);
        }

        /// <summary>
        /// Handles a 500 internal server error.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        /// <param name="errorMessage">A human-readable error message.</param>
        /// <param name="filename">The filename of the source file that caused the error.</param>
        /// <param name="programResult">The program result, if any.</param>
        private void HandleInternalServerError(
            HttpProcessor processor, 
            string errorMessage, 
            string filename,
            SymbolicExpression programResult)
        {
            // Get file path of 500 error page.
            var path = GetUrlPath(_configurationProvider.Get().InternalServerErrorPage);
            var extension = Path.GetExtension(path);

            // Serve default 500 error page if configured error page doesn't exist.
            if (!File.Exists(path))
            {
                ServeDefaultInternalServerErrorPage(processor);
                return;
            }

            // Check for forbidden.
            if (IsForbiddenPath(path))
            {
                ServeDefaultForbiddenPage(processor);
                return;
            }

            // Check if file should be interpreted.
            if (IsInterpretedFileExtension(extension))
            {
                // Try to evaluate program.
                SymbolicExpression result;
                try
                {
                    var runtime = _crispRuntimeFactory.GetCrispRuntime(path);
                    var serialized = HttpUtility.UrlEncode(_symbolicExpressionSerializer.Serialize(programResult));
                    var headers = TransformHeadersForCrisp(processor.Headers);
                    var args = $"\"{processor.HttpUrl}\" \"POST\" \"filename={filename}&programResult={serialized}" +
                               $"&errorMessage={errorMessage}\" {headers}";
                    result = runtime.Run(args);
                }
                catch
                {
                    // If configured 500 error page throws an exception, use default.
                    ServeDefaultInternalServerErrorPage(processor);
                    return;
                }

                // Resulting expression should be a list.
                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // Resulting expression was not a list, error.
                    ServeDefaultInternalServerErrorPage(processor);
                    return;
                }
                else
                {
                    // The response should be valid.
                    var expanded = result.AsPair().Expand();
                    if (!IsValidResult(expanded))
                    {
                        // Resulting expression was not a response, error.
                        ServeDefaultInternalServerErrorPage(processor);
                        return;
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value, TransformHeadersForPacket(expanded[3]));
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                // Error page is a static file.
                ServeStaticFile(processor);
            }
        }

        /// <summary>
        /// Handles a 404 not found error.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        /// <param name="errorMessage">A human-readable error message.</param>
        /// <param name="filename">The filename of the file that could not be found.</param>
        private void HandleNotFoundError(HttpProcessor processor, string errorMessage, string filename)
        {
            // Get file path of 404 error page.
            var path = GetUrlPath(_configurationProvider.Get().NotFoundErrorPage);
            var extension = Path.GetExtension(path);

            // Serve default 404 error page if configured error page doesn't exist.
            if (!File.Exists(path))
            {
                ServeDefaultNotFoundPage(processor);
                return;
            }

            // Check for forbidden.
            if (IsForbiddenPath(path))
            {
                ServeDefaultForbiddenPage(processor);
                return;
            }

            // Check if file should be interpreted.
            if (IsInterpretedFileExtension(extension))
            {
                // Try to evaluate program.
                SymbolicExpression result;
                try
                {
                    var runtime = _crispRuntimeFactory.GetCrispRuntime(path);
                    var headers = TransformHeadersForCrisp(processor.Headers);
                    result = runtime.Run($"\"{processor.HttpUrl}\" \"POST\" \"filename={filename}" +
                                         $"&errorMessage={errorMessage}\" {headers}");
                }
                catch
                {
                    // Serve default 404 error page if configured error page throws an error.
                    ServeDefaultNotFoundPage(processor);
                    return;
                }
                
                // Resulting expression should be a list.
                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // Resulting expression was not a list, serve default page.
                    ServeDefaultNotFoundPage(processor);
                    return;
                }
                else
                {
                    // The response should be valid.
                    var expanded = result.AsPair().Expand();
                    if (!IsValidResult(expanded))
                    {
                        // Resulting expression was not a response, serve default page.
                        ServeDefaultNotFoundPage(processor);
                        return;
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value, TransformHeadersForPacket(expanded[3]));
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                // Error page is a static file.
                ServeStaticFile(processor);
            }
        }

        /// <summary>
        /// Serves the default 403 error page.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        private static void ServeDefaultForbiddenPage(HttpProcessor processor)
        {
            processor.WriteResponse(403, "text/html");
            processor.OutputStream.Write(Properties.Resources.DefaultForbiddenPage);
        }

        public override void HandleGetRequest(HttpProcessor processor)
        {
            // Get file path of requested resource.
            var path = GetUrlPath(processor.HttpUrl);
            var extension = Path.GetExtension(path);

            // Serve configured 404 page if file doesn't exist.
            if (!File.Exists(path))
            {
                HandleNotFoundError(processor, "The file was not found on the server.", path);
                return;
            }

            // Check for forbidden.
            if (IsForbiddenPath(path))
            {
                ServeDefaultForbiddenPage(processor);
                return;
            }

            // Check if file should be interpreted.
            if (IsInterpretedFileExtension(extension))
            {
                // Try to evaluate program.
                SymbolicExpression result;
                try
                {
                    var runtime = _crispRuntimeFactory.GetCrispRuntime(path);
                    var headers = TransformHeadersForCrisp(processor.Headers);
                    result = runtime.Run(
                        $"\"{processor.HttpUrl}\" \"GET\" nil {headers}");
                }
                catch (Exception ex)
                {
                    // Throw a 500 for problems executing the program.
                    HandleInternalServerError(processor, ex.Message, path, new Nil());
                    return;
                }

                // Resulting expression should be a list.
                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // Resulting expression was not a list, error.
                    HandleInternalServerError(
                        processor, 
                        "The result of program evaluation was not of the correct type.",
                        path,
                        result);
                    return;
                }
                else
                {
                    // The response should be valid.
                    var expanded = result.AsPair().Expand();
                    if (!IsValidResult(expanded))
                    {
                        // Resulting expression was not a response, error.
                        HandleInternalServerError(
                            processor,
                            "The result of program evaluation was not of the correct form.",
                            path,
                            result);
                        return;
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value), 
                        expanded[2].AsString().Value, TransformHeadersForPacket(expanded[3]));
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                // Client asked for a static file.
                ServeStaticFile(processor);
            }
        }

        public override void HandlePostRequest(HttpProcessor processor, StreamReader inputStream)
        {
            // Read posted data.
            var posted = inputStream.ReadToEnd();

            // Get file path of requested resource.
            var path = GetUrlPath(processor.HttpUrl);
            var extension = Path.GetExtension(path);

            // Serve configured 404 page if file doesn't exist.
            if (!File.Exists(path))
            {
                HandleNotFoundError(processor, "The file was not found on the server.", path);
                return;
            }

            // Check for forbidden.
            if (IsForbiddenPath(path))
            {
                ServeDefaultForbiddenPage(processor);
                return;
            }

            // Check if file should be interpreted.
            if (IsInterpretedFileExtension(extension))
            {
                // Try to evaluate program.
                SymbolicExpression result;
                try
                {
                    var runtime = _crispRuntimeFactory.GetCrispRuntime(path);
                    var encoded = HttpUtility.UrlEncode(posted);
                    var headers = TransformHeadersForCrisp(processor.Headers);
                    result = runtime.Run(
                        $"\"{processor.HttpUrl}\" \"POST\" \"{encoded}\" {headers}");
                }
                catch (Exception ex)
                {
                    // Throw a 500 for problems executing the program.
                    HandleInternalServerError(processor, ex.Message, path, new Nil());
                    return;
                }

                // Resulting expression should be a list.
                if (result.Type != SymbolicExpressionType.Pair)
                {
                    // Resulting expression was not a list, error.
                    HandleInternalServerError(
                        processor,
                        "The result of program evaluation was not of the correct type.",
                        path,
                        result);
                    return;
                }
                else
                {
                    // The response should be valid.
                    var expanded = result.AsPair().Expand();
                    if (!IsValidResult(expanded))
                    {
                        // Resulting expression was not a response, error.
                        HandleInternalServerError(
                            processor,
                            "The result of program evaluation was not of the correct form.",
                            path,
                            result);
                        return;
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value, TransformHeadersForPacket(expanded[3]));
                    processor.OutputStream.Write(expanded[0].AsString().Value);
                }
            }
            else
            {
                // Client asked for a static file.
                ServeStaticFile(processor);
            }
        }
    }
} 
