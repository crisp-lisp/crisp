using System.IO;

namespace Crisp.Core.Preprocessing
{
    public class RequirePathTransformer : IRequirePathTransformer
    {
        private readonly IDirectoryPathProvider _interpreterDirectoryPathProvider;

        public string Transform(string path)
        {
            var filename = path.StartsWith("~/")
                ? _interpreterDirectoryPathProvider.GetPath() + path.TrimStart('~')
                : path;
            
            var fileInfo = new FileInfo(filename);
            return Path.IsPathRooted(filename)
                ? filename
                : Path.Combine(fileInfo.DirectoryName ?? string.Empty, filename);
        }

        public RequirePathTransformer(IDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
