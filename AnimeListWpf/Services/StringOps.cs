using AnimeListWpf.Models;

namespace AnimeListWpf.Services;

internal class StringOps
{
    internal static List<AContent> sortSearch(List<AContent> list, string query)
    {
        return list
        .OrderByDescending(s => relevance(s, query))
        .ToList();
    }

    private static int relevance(AContent content, string query)
    {
        string name;
        if (content.OtherName is null)
        {
            name = content.Name;
        }
        else
        {
            if (value(content.Name, query) > value(content.OtherName, query))
            {
                name = content.Name;
            }
            else name = content.OtherName;
        }
        return value(name, query);
    }

    private static int value(string name, string query)
    {
        return 50 * relevanceByWords(name, query) - distance(name, query);
    }

    private static int distance(string s, string t)
    {
        if (string.IsNullOrEmpty(s))
        {
            if (string.IsNullOrEmpty(t))
                return 0;
            return t.Length;
        }
        if (string.IsNullOrEmpty(t))
        {
            return s.Length;
        }
        s = s.ToLower();
        t = t.ToLower();
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];
        for (int i = 0; i <= n; d[i, 0] = i++) ;
        for (int j = 1; j <= m; d[0, j] = j++) ;
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                int min1 = d[i - 1, j] + 1;
                int min2 = d[i, j - 1] + 1;
                int min3 = d[i - 1, j - 1] + cost;
                d[i, j] = Math.Min(Math.Min(min1, min2), min3);
            }
        }
        return d[n, m];
    }

    static int relevanceByWords(string str, string query)
    {
        str = str.ToLower();
        query = query.ToLower();
        string[] wordsInQuery = query.Split(' ');
        int count = 0;
        foreach (string word in wordsInQuery)
        {
            if (str.Contains(word)) count++;
        }
        return count;
    }

    internal static string formatName(string name)
    {
        if (name.Contains(", "))
        {
            string[] s = name.Split(", ");
            name = s[1] + " " + s[0];
        }
        return name;
    }

    internal static string GetLink(AContent content)
    {
        string def = "https://myanimelist.net/";
        if (content.IsAnime)
        {
            def += "anime";
        }
        else def += "manga";
        def += $"/{content.Id}";
        return def;
    }

    internal static string getFileName(string filePath)
    {
        string[] strings = filePath.Split(@"\");
        return strings.Last();
    }
}

