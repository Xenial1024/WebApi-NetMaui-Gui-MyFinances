namespace MyFinances.Core.Response
{
    public class Error(string source, string message)
    {
        public string Source { get; set; } = source;
        public string Message { get; set; } = message;
    }
}
