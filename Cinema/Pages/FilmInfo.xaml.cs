using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Cinema.Pages
{
	/// <summary>
	/// Логика взаимодействия для FilmInfo.xaml
	/// </summary>
	public partial class FilmInfo : Page
	{
		public List<MoviesAndDates> moviesGroups;
		public List<MovieButtonInfo> buttons;
		public string nameFilm;
		public FilmInfo(string name)
		{
			InitializeComponent();
			string dateMovie = new DateTime().ToString("M");
			nameFilm = name;
			using CinemaContext db = new();
			var info = (from Info in db.FilmsRaitingsGenres where Info.NameFilm == nameFilm select Info).ToList();
			foreach (FilmsRaitingsGenre film in info)
			{
				PosterImage.Source = Manager.CreateSource(film.Poster);
				Header.Text = film.NameFilm;
				DescrBlock.Text = film.Description;
				Age.Text = Environment.NewLine + film.AgeLimit.ToString() + "+";
			}
			var movies = (from Movies in db.MoviesSorts where Movies.NameFilm == nameFilm
						  orderby Movies.Month, Movies.Day, Movies.Hour, Movies.Minute select Movies).ToList();
			buttons = new List<MovieButtonInfo>();
			moviesGroups = new List<MoviesAndDates>();
			MoviesAndDates dates = new MoviesAndDates(null);
			foreach (MoviesSort movie in movies)
			{
				String date = movie.MovieTime.ToString("dddd")  + "," + Environment.NewLine + movie.MovieTime.ToString("M");
				if (date != dateMovie)
				{
					dates = new MoviesAndDates(date);
					MoviesPanel.Children.Add(dates);
					moviesGroups.Add(dates);
					dateMovie = date;
					ListMovies.Items.Add(movie.MovieTime.ToString("dddd") + ", "+ movie.MovieTime.ToString("M"));

				}
				Button button = new();
				button.Click += OpenTicket;
				button.Content = movie.MovieTime.ToString("t") + Environment.NewLine + movie.SalaryForPlace + " ₽";
				buttons.Add(new MovieButtonInfo { buttonTime = button, IdRoom = movie.IdRoom, IdMovie = movie.IdMovie, IdFilm = movie.IdFilm,
					NameFilm = movie.NameFilm, MovieTime = movie.MovieTime, SalaryForPlace = movie.SalaryForPlace, TypeView = movie.TypeView}); ;
				dates.panel.Children.Add(button);
			}
			ListMovies.SelectedIndex = 0;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			ListMovies.SelectionChanged += SearchMovie;
		}

		private void OpenTicket(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			MoviesSort movie = buttons.Find(item => item.buttonTime == button);
			GetTickets getTickets = new(movie);
			getTickets.Show();
		}

		private void SearchMovie(object sender, SelectionChangedEventArgs e)
		{
			MoviesPanel.Children.Clear();
			foreach (MoviesAndDates dates in moviesGroups)
			{
				string[] partsForSearch = ListMovies.SelectedItem.ToString().Split(',');
				partsForSearch[1] = partsForSearch[1][1..];
				if (dates.dateForMovies.StartsWith(partsForSearch[0]) && dates.dateForMovies.EndsWith(partsForSearch[1]))
				{
					MoviesPanel.Children.Add(dates);
				}
			}
		}
		private void Rate(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			int count = Convert.ToInt32(button.Name[1]) - 48;
			int i = 0;
			foreach (FrameworkElement element in RaitingButtons.Children)
			{
				if (element is Button)
				{
					Button star = (Button)element;
					if (i != count)
					{
						Image image = new() { Source = new BitmapImage(new Uri("/Resources/starRaitingSelect.png", UriKind.Relative)) };
						star.Content = image;
						i++;
					}
					else
					{
						Image image = new() { Source = new BitmapImage(new Uri("/Resources/starRaiting.png", UriKind.Relative)) };
						star.Content = image;
					}
				}
			}
			MovieButtonInfo id = buttons.First();
			using CinemaContext db = new();
			Rating raiting = db.Ratings.Single(item => item.IdFilm == id.IdFilm);
			raiting.Rating1 = (raiting.Rating1 * raiting.CountOfPeopleInRaiting + count) / (raiting.CountOfPeopleInRaiting + 1);
			raiting.CountOfPeopleInRaiting++;
			db.SaveChanges();
		}
	}
}
