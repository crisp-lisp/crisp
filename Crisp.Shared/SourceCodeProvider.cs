namespace Crisp.Shared
{
    public class SourceCodeProvider : Provider<string>, ISourceCodeProvider
    {
        public SourceCodeProvider(string sourceCode) : base(sourceCode)
        {
        }
    }
}
