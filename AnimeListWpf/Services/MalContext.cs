using AnimeListWpf.Models;
using AnimeListWpf.Services;
using JikanDotNet;
using JikanDotNet.Config;
using JikanDotNet.Exceptions;
using Anime = AnimeListWpf.Models.Anime;
using Manga = AnimeListWpf.Models.Manga;

namespace ContentList.Services;

public class MalContext
{
    IJikan jikan;
    public MalContext()
    {
        JikanClientConfiguration config = new JikanClientConfiguration()
        {
            LimiterConfigurations = TaskLimiterConfiguration.None
        };
        jikan = new Jikan(config);
    }

    private List<string> getTitles(ICollection<TitleEntry> entries)
    {
        List<string> titles = new List<string>();
        if (entries.Count > 1)
        {
            string english, def;
            def = english = "";
            foreach (TitleEntry entry in entries)
            {
                if (entry.Type == "English") english = entry.Title;
                if (entry.Type == "Default") def = entry.Title;
            }
            if (english == def)
            {
                titles.Add(def);
            }
            else
            {
                if (def == "")
                {
                    titles.Add(english);
                }
                else if (english == "")
                {
                    titles.Add(def);
                }
                else
                {
                    titles.Add(def);
                    titles.Add(english);
                }
            }
        }
        else
        {
            titles.Add(entries.First().Title);
        }
        return titles;
    }

    private List<string> getGerners(ICollection<MalUrl> toParse)
    {
        var gerners = new List<string>();
        if (toParse.Count > 0)
        {
            foreach (var g in toParse)
            {
                gerners.Add(g.Name);
            }
            return gerners;
        }
        return null;
    }

    private Anime toAnime(JikanDotNet.Anime input)
    {
        var genres = getGerners(input.Genres);
        int? year = null;
        if (input.Aired.From is not null)
        {
            year = input.Aired.From.Value.Year; ;
        }
        Anime anime = new Anime(
                id: (long)input.MalId,
                names: getTitles(input.Titles),
                episodes: input.Episodes,
                notOut: input.Airing,
                genres: genres,
                year: year,
                score: (float?)input.Score);
        return anime;
    }

    private Manga toManga(JikanDotNet.Manga input)
    {
        var genres = getGerners(input.Genres);
        int? year = null;
        if (input.Published.From is not null)
        {
            year = input.Published.From.Value.Year; ;
        }
        Manga manga = new Manga(
                id: input.MalId,
                names: getTitles(input.Titles),
                count: input.Chapters,
                notOut: input.Publishing,
                genres: genres,
                authors: getAuthors(input.Authors),
                year: year,
                score: (float?)input.Score);
        return manga;
    }

    public List<string> getAuthors(ICollection<MalUrl> C)
    {
        var authors = new List<string>();
        foreach (var url in C)
        {
            authors.Add(StringOps.formatName(url.Name));
        }
        return authors;
    }

    public async Task<Anime> GetAnimeId(long id)
    {
        try
        {
            var res = await jikan.GetAnimeAsync(id);
            return toAnime(res.Data);
        }
        catch (JikanRequestException)
        {
            return await GetAnimeId(id);
        }
        catch (JikanValidationException)
        {
            return null;
        }
    }

    public async Task<Manga> GetMangaId(long id)
    {
        try
        {
            var res = await jikan.GetMangaAsync(id);
            return toManga(res.Data);
        }
        catch (JikanRequestException)
        {
            return await GetMangaId(id);
        }
        catch (JikanValidationException)
        {
            return null;
        }
    }

    public async Task<List<AContent>> searchAnime(string query)
    {
        try
        {
            var animes = await jikan.SearchAnimeAsync(query);
            if (animes is null)
            {
                return null;
            }
            List<AContent> animeList = new List<AContent>();
            int i = 0;
            foreach (var item in animes.Data)
            {
                if (i > 4) break;
                i++;
                if (item is not null)
                {
                    Anime a = toAnime(item);
                    animeList.Add(a);
                }
            }
            return animeList;
        }
        catch (JikanRequestException)
        {
            return await searchAnime(query);
        }
    }

    public async Task<List<AContent>> searchManga(string query)
    {
        try
        {
            var mangas = await jikan.SearchMangaAsync(query);
            if (mangas is null)
            {
                return null;
            }
            List<AContent> mangaList = new List<AContent>();
            int i = 0;
            foreach (var item in mangas.Data)
            {
                if (i > 4) break;
                i++;
                Manga a = toManga(item);
                mangaList.Add(a);
            }
            return mangaList;
        }
        catch (JikanRequestException)
        {
            return await searchManga(query);
        }
    }
}
