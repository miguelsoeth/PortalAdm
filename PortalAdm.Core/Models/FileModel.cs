namespace PortalAdm.Core.Models;

public class FileModel
{
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
}