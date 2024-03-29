﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls.Primitives;
using Cinema.Classes;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Cinema
{
	public partial class GetTickets : Window
	{

		public List<ButtonPlaces> listPlaces;
		public List<ButtonPlaces> selectedPlaces;
		public int cost = 0;
		public int IdMovieTicket;
		public int IdRoomTicket;
		public string nameOfFilm;
		public DateTime movieTime;
		public GetTickets(MoviesSort movie)
		{
			InitializeComponent();
			listPlaces = new List<ButtonPlaces>();
			selectedPlaces = new List<ButtonPlaces>();
			using CinemaContext db = new();
			int row = 0;
			IdMovieTicket = (int)movie.IdMovie;
			IdRoomTicket = (int)movie.IdRoom;
			var poster = db.Films.FromSqlRaw("SELECT * FROM dbo.Films WHERE (Name_film = {0})", movie.NameFilm);
			foreach (Film film in poster)
			{
				PosterFilm.Source = Manager.CreateSource(film.Poster);
			}
			Header.Text = movie.NameFilm;
			nameOfFilm = movie.NameFilm;
			movieTime = movie.MovieTime;
			Time.Text = "Зал " + movie.IdRoom + ", " + movie.TypeView + ", " + movie.MovieTime.ToString("M") + ", " +
				movie.MovieTime.ToString("t");
			SqlParameter parameter = new("@id_movie", movie.IdMovie);
			var places = db.AvailablePlaces.FromSqlRaw("RoomPlaces @id_movie", parameter).ToList();
			foreach (AvailablePlace place in places)
			{
				TextBlock textBlock = new()
				{
					Text = place.NumberRow + " ряд, " + place.NumberPlace + " место" + Environment.NewLine + movie.SalaryForPlace + " ₽",
					TextAlignment = TextAlignment.Center,
					FontSize = 15
				};
				Button ellipse = new();
				if (place.IdTicket is null)
				{
					ellipse.Style = (Style)FindResource("Place_button");
					ellipse.Click += ChoosePlace;
				}
				else ellipse.Style = (Style)FindResource("Non-free_place_button");
				ToolTip toolTip = new()
				{
					Content = textBlock,
					PlacementTarget = ellipse,
					Placement = PlacementMode.Top
				};
				ellipse.ToolTip = toolTip;
				row = place.NumberRow;
				if (place.NumberRow < 8)
				{
					wrappanelFirst.Children.Add(ellipse);
				}
				else if (place.NumberRow >= 8)
				{
					wrappanelSecond.Children.Add(ellipse);
				}
				listPlaces.Add(new ButtonPlaces { place = ellipse, numberPlace = place.NumberPlace, row = place.NumberRow, salary = (int)movie.SalaryForPlace});
			}
			if (row == 9)
			{
				TextBlock textblock = new()
				{
					Text = row.ToString()
				};
				TextBlock textBlock2 = new()
				{
					Text = row.ToString()
				};
				NumberRowLeft.Children.Add(textblock);
				NumberRowRight.Children.Add(textBlock2);
			}
		}

		private void Close(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void ChoosePlace(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			ButtonPlaces place = listPlaces.Find(item => item.place == button);
			if (selectedPlaces.Find(item => item.place == button) is null)
			{
				selectedPlaces.Add(place);
				button.Background = System.Windows.Media.Brushes.Red;
				if (CostButton.Visibility == Visibility.Hidden)
				{
					CostButton.Visibility = Visibility.Visible;
				}
				cost += place.salary;
				CostButton.Content = cost + " ₽";
			}
			else
			{
				selectedPlaces.Remove(place);
				button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 226, 108));
				if (selectedPlaces.Count == 0)
				{
					CostButton.Visibility = Visibility.Hidden;
				}
				cost -= place.salary;
				CostButton.Content = cost + " ₽";
			}
		}
		private void FormCheque()
		{
			Tickets.Children.Clear();
			foreach (ButtonPlaces ticket in selectedPlaces)
			{
				TicketCheque ticketCheque = new(ticket.row + " ряд, " + ticket.numberPlace + " место", ticket.salary.ToString() + " ₽");
				Tickets.Children.Add(ticketCheque);
			}
		}
		private void OpenConf(object sender, MouseButtonEventArgs e)
		{
			FormCheque();
			TotalCost.Text = cost.ToString() + " ₽";
		}

		private void OpenCheque(object sender, RoutedEventArgs e)
		{
			FormCheque();
			Conf.Visibility = Visibility.Visible;
			tabControl.SelectedIndex = 1;
			NameFilm.Text = Header.Text;
			TimeCheque.Text = Time.Text;
			TotalCost.Text = cost.ToString() + " ₽";
		}

		private void OpenPay(object sender, RoutedEventArgs e)
		{
			tabControl.SelectedIndex = 2;
			Payment.Visibility = Visibility.Visible;
		}

		private void CreateTickets(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (Regex.IsMatch(NumberCard.Text, @"^\d\d\d\d\ \d\d\d\d\ \d\d\d\d\ \d\d\d\d$"))
			{
				FinishText.Visibility = Visibility.Visible;
				Finish.Visibility = Visibility.Visible;
				using CinemaContext db = new();
				foreach (ButtonPlaces ticket in selectedPlaces)
				{
					RealTicket newTicket = new(nameOfFilm, movieTime, IdRoomTicket, ticket.row, ticket.numberPlace, cost) { Width = 800, Height = 400,
					HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top};
					newTicket.Measure(new System.Windows.Size(newTicket.Width, newTicket.Height));
					newTicket.Arrange(new Rect(new System.Windows.Point(0, 0), newTicket.DesiredSize));
					RenderTargetBitmap bmp = new RenderTargetBitmap((int)newTicket.ActualWidth, (int)newTicket.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
					bmp.Render(newTicket);
					MemoryStream stream = new MemoryStream();
					BitmapEncoder encoder = new BmpBitmapEncoder();
					encoder.Frames.Add(BitmapFrame.Create(bmp));
					encoder.Save(stream);
					Bitmap bitmap = new Bitmap(stream);
					bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
					db.Tickets.Add(new Ticket { Email = Manager.Email, IdMovie = IdMovieTicket,
							NumberPlace = ticket.numberPlace, NumberRow = ticket.row, ImageTicket = stream.ToArray()});
				}
				db.SaveChanges();
				button.IsEnabled = false;
			}
		}
	}
}