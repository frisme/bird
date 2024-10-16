﻿namespace bird;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
	}

	public void ClicaCavalo(object Page, EventArgs e)
	{
		Navigation.PushAsync(new GamePage());
	}
}