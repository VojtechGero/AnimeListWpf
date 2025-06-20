namespace AnimeListWpf.Models;

public class DescriptionSegment
{
    public string Title { get; set; }
    public List<string> Description { get; set; } = new List<string>();
    public DescriptionSegment(string title, IEnumerable<string> description)
    {
        Title = title; Description = description.ToList();
    }
    public DescriptionSegment(string title, string description)
    {
        Title = title;
        Description.Add(description);
    }

}
