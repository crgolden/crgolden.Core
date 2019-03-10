namespace Clarity.Core
{
    public abstract class FileModel : Model
    {
        public string ContentType { get; set; }

        public string Extension { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string Uri { get; set; }
    }
}
