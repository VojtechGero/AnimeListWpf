namespace AnimeListWpf.Models;

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

    public override string Description()
    {
        string output = "";
        const string tab = "    ";
        if (OtherName is not null)
        {
            output += $"({OtherName})\n";
        }
        output += "Manga" + tab;
        if (NotOut) output += "Currently Publishing\n";
        else output += "Finished Publishing\n";
        if (Score is not null) output += $"Score: {Score:F2}\n";
        if (Started is not null)
        {
            output += $"Started publishing: {Started}\n";
        }
        if (Count > 0) output += $"Chapters: {Count}\n";
        if (Authors is not null)
        {
            int c = Authors.Count;
            if (c > 0)
            {
                output += "Author:\n";
                foreach (string author in Authors)
                {
                    output += $"{tab}{author}\n";
                }
            }
        }
        if (Genres is not null)
        {
            if (Genres.Count > 0) output += "Genres:\n";
            foreach (string g in Genres)
            {
                output += $"{tab}{g}\n";
            }
        }
        return output;
    }
}
