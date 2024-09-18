using AnimeListWpf.Models;

namespace AnimeListWpf.Services;

public static class ContentMapper
{
    public static T Map<T>(AContent content) where T : AContent, new()
    {
        T mappedContent = new T();

        mappedContent.IsAnime = content.IsAnime;
        mappedContent.Id = content.Id;
        mappedContent.Name = content.Name;
        mappedContent.OtherName = content.OtherName;
        mappedContent.Count = content.Count;
        mappedContent.NotOut = content.NotOut;
        mappedContent.Genres = new List<string>();
        if (content.Genres != null)
        {
            mappedContent.Genres.AddRange(content.Genres);
        }
        mappedContent.Authors = new List<string>();
        if (content.Authors != null)
        {
            mappedContent.Authors.AddRange(content.Authors);
        }
        mappedContent.Started = content.Started;
        mappedContent.InProgress = content.InProgress;
        mappedContent.Score = content.Score;

        return mappedContent;
    }

}
