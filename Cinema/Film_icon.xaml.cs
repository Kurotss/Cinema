using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Cinema.Pages;

namespace Cinema
{
	public partial class Film_icon : UserControl
	{
		public Film_icon(BitmapImage image, string name, string genre, double raiting, int ageLimit, int idRole, Film film)
		{
			InitializeComponent();
			this.Poster_film = image;
			this.Name_film = name;
			this.Genres_list = genre;
			this.Raiting_film = raiting;
			this.NewName = LengthNameFilm(name);
			this.Age_limit = ageLimit;
			this.stringFilm = film;
			this.IDrole = idRole;
			this.DataContext = this;
			if (idRole == 1) Canvas.Children.Remove(Delete);
			else deleteButton = Delete;
		}
		public int IDrole { get; set; }
		public Button deleteButton { get; set; }
		public string Name_film { get; set; }
		public string Genres_list { get; set; }
		public double Raiting_film { get; set; }
		public BitmapImage Poster_film { get; set; }
		public string NewName { get; set; }
		public int Age_limit { get; set; }
		public Film stringFilm { get; set; }
		private void Info(object sender, RoutedEventArgs e)
		{
			Canvas canvas = (Canvas)sender;
			Manager.Animation(canvas, 0, 90, 0);
			Manager.Animation(canvas, canvas.Height, canvas.Height + 30, 1);
			Manager.Animation(canvas, canvas.Width, canvas.Width + 30, 2);
		}
		private void InfoLeave(object sender, RoutedEventArgs e)
		{
			Canvas canvas = (Canvas)sender;
			Manager.Animation(canvas, 90, 0, 0);
			Manager.Animation(canvas, canvas.Height + 30, canvas.Height, 1);
			Manager.Animation(canvas, canvas.Width + 30, canvas.Width, 2);
		}
		private string LengthNameFilm(string name)
		{
			if (name.Length <= 18)
			{
				name = Environment.NewLine + name;
			}
			return name;
		}
		private void OpenInfo(object sender, RoutedEventArgs e)
		{
			switch (IDrole)
			{
				case 1: Manager.mainwindow.PageOpen(new FilmInfo(Name_film));
					break;
				case 2: Manager.adminwindow.PageOpen(new FilmInfoAdmin(Name_film));
					break;
			}
		}
	}
}
