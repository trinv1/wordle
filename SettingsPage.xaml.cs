using System;
using Microsoft.Maui.Controls;

namespace wordle;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
    }

    private void DarkModeButton_Clicked(object sender, EventArgs e)
    {
        // Check the current background color
        if (BackgroundColor == Color.FromRgb(255, 165, 0))
        {
            // Switch to Dark Mode
            BackgroundColor = Color.FromRgb(0, 0, 0); 
        }
        else
        {
            // Switch to Light Mode
            BackgroundColor = Color.FromRgb(255, 165, 0); 
        }
    }

    private void FontSizeButton_Clicked(object sender, EventArgs e)
    {
        // Checking the current font size of the FontSizeButton
        double currentFontSize = FontSizeButton.FontSize;

        // Defining the new font size 
        double newFontSize = currentFontSize + 2; // Increase the font size by 2 units

        // Setting the new font size to the FontSizeButton
        FontSizeButton.FontSize = newFontSize;

       // Printing new font size to console
        Console.WriteLine($"Font Size set to: {newFontSize}");
    }

    private void EasyModeButton_Clicked(object sender, EventArgs e)
    {
        // Handling Easy Mode setting here
        Console.WriteLine("Easy Mode button clicked");
    }

    private void TimeLimitButton_Clicked(object sender, EventArgs e)
    {
        // Handleing Time Limit setting here
        Console.WriteLine("Time Limit button clicked");
    }
  }
