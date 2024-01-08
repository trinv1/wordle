using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WORDLE
{
    public class WordList : INotifyPropertyChanged
    {
        // File paths and URL for word list
        private static readonly string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "words.txt");
        private static readonly string fileUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt";

        // HttpClient for downloading
        private HttpClient httpClient;

        // List to store words
        private List<string> words;

        // Flag to indicate if the list is busy
        private bool isBusy;

        // Event for property change notification
        public event PropertyChangedEventHandler PropertyChanged;

        // Constructor
        public WordList()
        {
            httpClient = new HttpClient();
            words = new List<string>();
        }

        // Method to raise the PropertyChanged event
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Method to ensure the word list is available
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

        // Method to download the word list file
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

        // Method to load words from the local file
        private void LoadWordsFromFile()
        {
            if (File.Exists(localFilePath))
            {
                var lines = File.ReadAllLines(localFilePath);
                words.Clear();
                words.AddRange(lines);
            }
        }

        // Property to get the list of words
        public List<string> Words
        {
            get => words;
            private set
            {
                words = value;
                OnPropertyChanged();
            }
        }

        // Property to get or set the busy state
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

        // Property to get the opposite of IsBusy
        public bool IsNotBusy => !IsBusy;

        // Method to get a random word from the list
        public string GetRandomWord()
        {
            if (Words.Count == 0)
                return string.Empty;

            Random random = new Random();
            int index = random.Next(Words.Count);
            return Words[index];
        }
    }
}
