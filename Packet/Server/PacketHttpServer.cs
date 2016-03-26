using System;
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

        private readonly IServerStartupSettingsProvider _serverStartupSettingsProvider;

        private readonly ICrispRuntimeFactory _crispRuntimeFactory;

        private readonly ISymbolicExpressionSerializer _symbolicExpressionSerializer;

        /// <summary>
        /// Initializes a new instance of a Packet HTTP/1.0 server.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider for the application.</param>
        /// <param name="serverStartupSettingsProvider">The startup settings provider for the server.</param>
        /// <param name="crispRuntimeFactory">The factory to use to create Crisp runtime instances.</param>
        /// <param name="symbolicExpressionSerializer">The serializer to use to display debug output.</param>
        public PacketHttpServer(
            IConfigurationProvider configurationProvider,
            IServerStartupSettingsProvider serverStartupSettingsProvider,
            ICrispRuntimeFactory crispRuntimeFactory,
            ISymbolicExpressionSerializer symbolicExpressionSerializer)
            : base(serverStartupSettingsProvider.GetSettings().Port)
        {
            _configurationProvider = configurationProvider;
            _serverStartupSettingsProvider = serverStartupSettingsProvider;
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
            var path = Path.Combine(_serverStartupSettingsProvider.GetSettings().WebRoot, trimmed);

            if (Directory.Exists(path)) // If real path is a directory.
            {
                var files = Directory.GetFiles(path)
                    .Select(Path.GetFileName)
                    .Where(f => _configurationProvider.GetConfiguration().DirectoryIndices.Contains(f))
                    .ToArray(); // Get any configured index pages.
                if (files.Any())
                {
                    path = Path.Combine(path, files.First()); // Pass back path to configured index page.
                }
                else
                {
                    // TODO: Serve default directory index?
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
            return _configurationProvider.GetConfiguration().MimeTypeMappings.TryGetValue(extension, out value) ?
                value : "application/octet-stream"; // Default to this MIME type.
        }

        /// <summary>
        /// Returns true if the given file extension if configured to be interpreted.
        /// </summary>
        /// <param name="extension">The file extension to check, including the leading dot.</param>
        /// <returns></returns>
        private bool IsInterpretedFileExtension(string extension)
        {
            return _configurationProvider.GetConfiguration().CrispFileExtensions.Contains(extension);
        }

        private bool IsForbiddenPath(string path)
        {
            return _configurationProvider.GetConfiguration().DoNotServePatterns.Any(p => Regex.IsMatch(path, p));
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
        /// Serves the default 500 error page.
        /// </summary>
        /// <param name="processor">The <see cref="HttpProcessor"/> to write the response to.</param>
        private static void ServeDefaultInternalServerErrorPage(HttpProcessor processor)
        {
            processor.WriteResponse(500, "text/html");
            processor.OutputStream.Write(Properties.Resources.DefaultInternalServerErrorPage);
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
            var path = GetUrlPath(_configurationProvider.GetConfiguration().InternalServerErrorPage);
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
                    var args = $"\"{processor.HttpUrl}\" \"POST\" \"filename={filename}&programResult={serialized}&errorMessage={errorMessage}\"";
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
                }
                else
                {
                    // The response should consist of 3 parts.
                    var expanded = result.AsPair().Expand();
                    if (expanded.Count != 3)
                    {
                        // Resulting expression was not a response, error.
                        ServeDefaultInternalServerErrorPage(processor);
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value);
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
            var path = GetUrlPath(_configurationProvider.GetConfiguration().NotFoundErrorPage);
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
                    result = runtime.Run($"\"{processor.HttpUrl}\" \"POST\" \"filename={filename}&errorMessage={errorMessage}\"");
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
                }
                else
                {
                    // The response should consist of 3 parts.
                    var expanded = result.AsPair().Expand();
                    if (expanded.Count != 3)
                    {
                        // Resulting expression was not a response, serve default page.
                        ServeDefaultNotFoundPage(processor);
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value);
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
                    result = runtime.Run(
                        $"\"{processor.HttpUrl}\" \"GET\" nil");
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
                }
                else 
                {
                    // The response should consist of 3 parts.
                    var expanded = result.AsPair().Expand();
                    if (expanded.Count != 3)
                    {
                        // Resulting expression was not a response, error.
                        HandleInternalServerError(
                            processor,
                            "The result of program evaluation was not of the correct form.",
                            path,
                            result); 
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value), 
                        expanded[2].AsString().Value);
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
                    result = runtime.Run(
                        $"\"{processor.HttpUrl}\" \"POST\" \"{encoded}\"");
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
                }
                else
                {
                    // The response should consist of 3 parts.
                    var expanded = result.AsPair().Expand();
                    if (expanded.Count != 3)
                    {
                        // Resulting expression was not a response, error.
                        HandleInternalServerError(
                            processor,
                            "The result of program evaluation was not of the correct form.",
                            path,
                            result);
                    }

                    // Pass response back to client.
                    processor.WriteResponse(
                        Convert.ToInt32(expanded[1].AsNumeric().Value),
                        expanded[2].AsString().Value);
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
