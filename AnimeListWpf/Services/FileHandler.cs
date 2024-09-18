using AnimeListWpf.Models;
using System.IO;
using System.Text.Json;

namespace AnimeListWpf.Services;

public class FileHandler
{
    string file;
    List<string> data;
    private static readonly string[] start = { "[", "]" };

    FileHandler(string file)
    {
        this.file = file;
        data = new List<string>();
        ReadContent();
    }

    public static FileHandler OpenFile()
    {
        string file = @".\AnimeList.json";
        if (!File.Exists(file))
        {
            File.AppendAllLines(file, start);
        }
        return new FileHandler(file);
    }

    public static List<string> GetLines(string path)
    {
        return File.ReadAllLines(path).ToList();
    }

    void ReadContent()
    {
        data = File.ReadAllLines(file).ToList();
    }

    public List<AContent> GetContent()
    {
        List<AContent> contents = new List<AContent>();
        foreach (string s in data)
        {
            string temp = s.Trim().Trim(',');
            if (temp == "[" || temp == "]" || string.IsNullOrWhiteSpace(temp)) continue;
            UnclassifiedContent content = JsonSerializer.Deserialize<UnclassifiedContent>(temp);
            if (content.IsAnime)
            {
                contents.Add(ContentMapper.Map<Anime>(content));
            }
            else
            {
                contents.Add(ContentMapper.Map<Manga>(content));
            }
        }
        return contents;
    }

    public void WriteContent(AContent a)
    {
        string newLine = a.ToJson();
        if (data.Count - 2 > 0)
        {
            data[data.Count - 2] = data[data.Count - 2] + ",";
        }
        data.Insert(data.Count - 1, newLine);
        WriteAll();
    }

    private void WriteAll()
    {
        File.WriteAllLines(file, data);
    }

    public void UpdateLines(List<int> indices, List<AContent> contents)
    {
        foreach (int i in indices)
        {
            int t = i + 1;
            if (t == data.Count - 2)
            {
                data[t] = contents[t - 1].ToJson();
            }
            else
            {
                data[t] = contents[t - 1].ToJson() + ",";
            }
        }
        WriteAll();
    }

    public void UpdateLine(int index, AContent content)
    {
        int t = index + 1;
        if (t == data.Count - 2)
        {
            data[t] = content.ToJson();
        }
        else
        {
            data[t] = content.ToJson() + ",";
        }
        WriteAll();
    }

    public void RemoveContents(List<int> indices)
    {
        foreach (int i in indices)
        {
            int t = i + 1;
            if (t == data.Count - 2 && t > 2)
            {
                data[t - 1] = data[t - 1].Remove(data[t - 1].Length - 1);
            }
            data.RemoveAt(t);
        }
        WriteAll();
    }

    public void CopyJson(string path)
    {
        string[] newText = File.ReadAllLines(path);
        for (int i = 1; i < newText.Length - 1; i++)
        {
            if (data.Count - 2 > 0 && data[data.Count - 2].Last() != ',')
            {
                data[data.Count - 2] = data[data.Count - 2] + ",";
            }
            if (newText[i].Last() != ',' && i != newText.Length - 2) newText[i] += ',';
            data.Insert(data.Count - 1, newText[i]);
        }
        WriteAll();
    }

    public void ExportJson(string path)
    {
        if (!data.Any()) throw new InvalidOperationException();
        string filePath = path + @"\AnimeList.json";
        int i = 1;
        while (File.Exists(filePath))
        {
            filePath = path + $@"\AnimeList({i}).json";
            i++;
        }
        File.WriteAllLines(filePath, data);
    }

    public void ExportTxt(string path, IEnumerable<string> names)
    {
        string filePath = path + @"\AnimeList.txt";
        int i = 1;
        while (File.Exists(filePath))
        {
            filePath = path + $@"\AnimeList({i}).txt";
            i++;
        }
        File.WriteAllLines(filePath, names);
    }
}
