namespace AnimeListWpf.Models.Content;

public class Manga : AContent
{

    public Manga() { }

    public Manga(long id, List<string> names, int? count, bool notOut,
                   List<string> genres, List<string> authors, int? year, float? score)
    {
        IsAnime = false;
        Id = id;
        Name = names.Last();
        if (count is not null)
        {
            this.Count = (int)count;
        }
        this.NotOut = notOut;
        this.Genres = genres;
        if (authors.Count > 3)
        {
            List<string> a = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                a.Add(authors[i]);
            }
            this.Authors = a;
        }
        else this.Authors = authors;
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
        output.Add(new DescriptionSegment("Manga", NotOut ? "Currently Publishing" : "Finished Publishing"));
        if (Score is not null) output.Add(new DescriptionSegment("Score", $"{Score:F2}"));
        if (Started is not null) output.Add(new DescriptionSegment("Started Publishing", Started.ToString()));
        if (Count > 0) output.Add(new DescriptionSegment("Chapters", Count.ToString()));
        if (Authors is not null)
        {
            var count = Authors.Count;
            if (count > 0) output.Add(new DescriptionSegment(count > 1 ? "Author" : "Authors", Authors));
        }
        if (Genres is not null)
        {
            if (Genres.Count > 0) output.Add(new DescriptionSegment("Genres", Genres));
        }
        return output;
    }
}
