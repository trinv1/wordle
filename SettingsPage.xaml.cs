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
        // Handle Dark Mode setting here
        Console.WriteLine("Dark Mode button clicked");
    }

    private void FontSizeButton_Clicked(object sender, EventArgs e)
    {
        // Handle Font Size setting here
        Console.WriteLine("Font Size button clicked");
    }

    private void EasyModeButton_Clicked(object sender, EventArgs e)
    {
        // Handle Easy Mode setting here
        Console.WriteLine("Easy Mode button clicked");
    }

    private void TimeLimitButton_Clicked(object sender, EventArgs e)
    {
        // Handle Time Limit setting here
        Console.WriteLine("Time Limit button clicked");
    }
  }

