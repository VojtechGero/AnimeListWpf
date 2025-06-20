namespace AnimeListWpf.Models.Content;

public class Anime : AContent
{

    public Anime() { }

    public Anime(long id, List<string> names, int? episodes, bool notOut,
                   List<string> genres, int? year, float? score)
    {
        IsAnime = true;
        Id = id;
        Name = names.Last();
        if (episodes is not null)
        {
            this.Count = (int)episodes;
        }
        this.NotOut = notOut;
        this.Genres = genres;
        Started = year;
        if (names.Count > 1)
        {
            OtherName = names.First();
        }
        else OtherName = null;
        InProgress = false;
        this.Score = score;
    }


    public override List<DescriptionSegment> Description()
    {
        List<DescriptionSegment> output = new List<DescriptionSegment>();

        if (OtherName is not null)
        {
            output.Add(new DescriptionSegment("Alternative name", OtherName));
        }
        output.Add(new DescriptionSegment("Anime", NotOut ? "Currently Airing" : "Finished Airing"));
        if (Score is not null) output.Add(new DescriptionSegment("Score", $"{Score:F2}"));
        if (Started is not null)
        {
            output.Add(new DescriptionSegment(Count == 1 ? "Aired" : "Started airing", Started.ToString()));
        }
        if (Count > 0) output.Add(new DescriptionSegment("Episodes", Count.ToString()));
        if (Genres is not null)
        {
            if (Genres.Count > 0) output.Add(new DescriptionSegment("Genres", Genres));
        }
        return output;
    }
}
