using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WORDLE;

public class WordList : INotifyPropertyChanged
{
    private static readonly string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.txt");
    private static readonly string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt";

    private HttpClient httpClient;
    private List<string> words;
    private bool isBusy;

    public event PropertyChangedEventHandler PropertyChanged;

    public WordList()
    {
        httpClient = new HttpClient();
        words = new List<string>();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task EnsureWordListIsAvailable()
    {
        if (words.Count == 0 && !File.Exists(localFilePath))
        {
            IsBusy = true;
            await DownloadFileAsync();
            IsBusy = false;
        }
        LoadWordsFromFile();
    }

    private async Task DownloadFileAsync()
    {
        try
        {
            var response = await httpClient.GetAsync(fileUrl);
            if (response.IsSuccessStatusCode)
            {
                var fileStream = await response.Content.ReadAsStreamAsync();
                using (var file = File.Create(localFilePath))
                {
                    await fileStream.CopyToAsync(file);
                }
            }
            else
            {
                Console.WriteLine("Failed to download the file.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during file download: {ex.Message}");
        }
    }

    private void LoadWordsFromFile()
    {
        if (File.Exists(localFilePath))
        {
            var lines = File.ReadAllLines(localFilePath);
            words.Clear();
            words.AddRange(lines);
        }
    }

    public List<string> Words
    {
        get => words;
        private set
        {
            words = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value)
                return;
            isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    public bool IsNotBusy => !IsBusy;

    public string GetRandomWord()
    {
        if (Words.Count == 0)
            return string.Empty;

        Random random = new Random();
        int index = random.Next(Words.Count);
        return Words[index];
    }
}
