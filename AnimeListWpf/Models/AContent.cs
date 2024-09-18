using System.Text.Json;

namespace AnimeListWpf.Models;

public abstract class AContent
{
    public bool IsAnime { get; set; }
    public long Id { get; set; }
    public string Name { get; set; }
    public string OtherName { get; set; }
    public int Count { get; set; }
    public bool NotOut { get; set; }
    public List<string> Genres { get; set; }
    public List<string> Authors { get; set; }
    public int? Started { get; set; }
    public bool InProgress { get; set; }
    public float? Score { get; set; }


    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public abstract string Description();

    public override string ToString()
    {
        return Name;
    }
}
