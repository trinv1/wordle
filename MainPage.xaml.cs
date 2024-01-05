

namespace WORDLE
{
    public partial class MainPage : ContentPage
    {

        private bool gamerunning = false;
        private int currentColumnIndex;

        public MainPage()
        {
            InitializeComponent();
            InitialiseGrid();
            NavigationPage.SetHasNavigationBar(this, false);

            // Hide the GameGrid and RulesLabel initially
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesLabel.IsVisible = false;
            RulesLabel.IsEnabled = false;
        }

        public void InitialiseGrid()
        {

            for (int i = 0; i < 6; i++) // Initialize 5x6 grid
            {
                for (int j = 0; j < 5; j++)
                {
                    BoxView box = new BoxView()
                    {
                        Color = Color.FromRgb(255, 255, 255),
                        CornerRadius = 20,
                        BackgroundColor = Color.FromRgb(255, 255, 255),
                        Margin = new Thickness(5), // Add margin to create gaps
                    };
                    Grid.SetRow(box, i); // Set the row position
                    Grid.SetColumn(box, j); // Set the column position
                    GameGrid.Children.Add(box); // Add the BoxView to the Children collection
                }
            }
        }

        private void OnStartGameClicked(object sender, EventArgs e)
        {
            // Hide the Start Game button and show the game grid
            StartGameButton.IsVisible = false;
            RulesButton.IsVisible = false;
            GameGrid.IsVisible = true;
            GameGrid.IsEnabled = true;
            RulesLabel.IsVisible = false;
            RulesLabel.IsEnabled = false;

            // Show the Home button
            HomeButton.IsVisible = true;
            KeyboardLayout.IsVisible = true;
            gamerunning = true;
        }

        private void OnRulesClicked(object sender, EventArgs e)
        {
            // Hide the Rules button and show the rules label
            StartGameButton.IsVisible = false;
            RulesButton.IsVisible = false;
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesLabel.IsVisible = true;
            RulesLabel.IsEnabled = true;

            // Show the Home button
            HomeButton.IsVisible = true;
        }

        private async void OnHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(); // Navigate back to the previous page
            gamerunning = false;
            GameGrid.IsVisible = false;
            GameGrid.IsEnabled = false;
            RulesButton.IsVisible = true;
            KeyboardLayout.IsVisible = false;
            HomeButton.IsVisible = false;
            StartGameButton.IsVisible = true;
            RulesLabel.IsVisible = false;
            RulesLabel.IsEnabled = false;
        }

        private void OnLetterClicked(object sender, EventArgs e)
        {
            if (gamerunning && currentColumnIndex < 5)
            {
                if (gamerunning && currentColumnIndex < 5)
                {
                    Button button = (Button)sender;

                    if (button.Text == "Back" && currentColumnIndex > 0)
                    {
                        // Remove the last added letter from the grid
                        currentColumnIndex--;
                        GameGrid.Children.RemoveAt(GameGrid.Children.Count - 1);
                    }
                    else
                    {
                        string letter = button.Text;


                        // Create a new Label with the clicked letter
                        Label label = new Label
                        {
                            Text = letter,
                            FontSize = 24,
                            FontAttributes = FontAttributes.Bold,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        };

                        // Add the Label to the current column and row in the grid
                        Grid.SetRow(label, 0); // Always add to the top row
                        Grid.SetColumn(label, currentColumnIndex);
                        GameGrid.Children.Add(label); // Add the Label to the Children collection

                        // Increment the current column index
                        currentColumnIndex++;
                    }
                }
            }
        }

        private void StartGame()
        {
            gamerunning = true;
        }
    }
}
