namespace AnimeListWpf.Models;

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


    public override string Description()
    {
        string output = "";
        const string tab = "    ";
        if (OtherName is not null)
        {
            output += $"({OtherName})\n";
        }
        output += "Anime" + tab;
        if (NotOut) output += "Currently Airing\n";
        else output += "Finished Airing\n";
        if (Score is not null) output += $"Score: {Score:F2}\n";
        if (Started is not null)
        {
            if (Count == 1) output += $"Aired: {Started}\n";
            else output += $"Started airing: {Started}\n";
        }
        if (Count > 0) output += $"Episodes: {Count}\n";
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
