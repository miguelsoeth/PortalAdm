namespace PortalAdm.SharedKernel.Util;

public static class FileUtil
{
    public static async Task<List<string>> GetDocumentsAsync(Stream fileStream)
    {
        var values = new List<string>();

        using (var reader = new StreamReader(fileStream))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var columns = line.Split(',');
                    if (columns.Length > 0)
                    {
                        values.Add(columns[0]);
                    }
                }
            }
        }

        return values;
    }
}