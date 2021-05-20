using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace Cinema.Pages
{
	public partial class FilmInfoAdmin : Page
	{
		public byte[] PosterBytes;
		public List<MoviesAndDates> moviesGroups;
		public List<MovieButtonInfo> buttons;
		public List<MovieButtonInfo> DeleteList;
		public string nameFilm;
		public int IDfilm;
		public FilmInfoAdmin(string name)
		{
			InitializeComponent();
			using CinemaContext db = new();
			foreach (AllGenre genre in db.AllGenres)
			{
				Genre1.Items.Add(genre.Genre);
				Genre2.Items.Add(genre.Genre);
			}
			string dateMovie = new DateTime().ToString("M");
			nameFilm = name;
			var info = (from Info in db.Films where Info.NameFilm == nameFilm select Info).ToList();
			foreach (Film film in info)
			{
				IDfilm = film.IdFilm;
				PosterImage.Source = Manager.CreateSource(film.Poster);
				Header.Text = film.NameFilm;
				DescrBlock.Text = film.Description;
				Age.Text = film.AgeLimit.ToString();
				PosterBytes = film.Poster;
				AirDate.SelectedDate = film.AirDate;
				EndDate.SelectedDate = film.EndDate;
			}
			Genres.Text = db.FilmsRaitingsGenres.Find(IDfilm).Genre;
			var movies = (from Movies in db.MoviesSorts
						  where Movies.NameFilm == nameFilm
						  orderby Movies.Month, Movies.Day, Movies.Hour, Movies.Minute
						  select Movies).ToList();
			buttons = new List<MovieButtonInfo>();
			moviesGroups = new List<MoviesAndDates>();
			DeleteList = new List<MovieButtonInfo>();
			MoviesAndDates dates = new MoviesAndDates(null);
			foreach (MoviesSort movie in movies)
			{
				String date = movie.MovieTime.ToString("dddd") + "," + Environment.NewLine + movie.MovieTime.ToString("M");
				if (date != dateMovie)
				{
					dates = new MoviesAndDates(date);
					MoviesPanel.Children.Add(dates);
					moviesGroups.Add(dates);
					dateMovie = date;
					ListMovies.Items.Add(movie.MovieTime.ToString("dddd") + ", " + movie.MovieTime.ToString("M"));

				}
				Button button = new();
				button.Click += ClickMovie;
				button.Content = movie.MovieTime.ToString("t") + Environment.NewLine + movie.SalaryForPlace + " ₽";
				buttons.Add(new MovieButtonInfo
				{
					buttonTime = button,
					IdRoom = movie.IdRoom,
					IdMovie = movie.IdMovie,
					IdFilm = movie.IdFilm,
					NameFilm = movie.NameFilm,
					MovieTime = movie.MovieTime,
					SalaryForPlace = movie.SalaryForPlace,
					TypeView = movie.TypeView,
					stringMovie = new Movie() { IdMovie = movie.IdMovie, IdFilm = movie.IdFilm,
						IdRoom = movie.IdRoom, MovieTime = movie.MovieTime },
					panel = dates.panel
				});
				dates.panel.Children.Add(button);
			}
			ListMovies.SelectedIndex = 0;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			ListMovies.SelectionChanged += SearchMovie;
		}

		private void LoadImage(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new() { Filter = "Image files(*.png)|*.png" };
			if ((bool)dialog.ShowDialog())
			{
				try
				{
					Poster.Text = dialog.FileName;
					System.Drawing.Image image = System.Drawing.Image.FromFile(dialog.FileName);
					ImageConverter converter = new();
					PosterBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
				}
				catch
				{
					MessageBox.Show("Ошибка добавления изображения!");
				}
			}
		}

		private void UpdateNewFilm(object sender, RoutedEventArgs e)
		{
			bool AllCorrect = true;
			int i = 0;
			foreach (var element in stack.Children)
			{
				if (element is TextBox)
				{
					TextBox box = (TextBox)element;
					box.BorderBrush = System.Windows.Media.Brushes.GreenYellow;
					if (box.Name == "Header")
					{
						if (!Regex.IsMatch(box.Text, @"^[а-яА-Яa-zA-Z0-9]+") || box.Text.Length < 2)
						{
							box.BorderBrush = System.Windows.Media.Brushes.Red;
							AllCorrect = false;
						}
					}
					else if (box.Name == "DescrBlock")
					{
						if (!Regex.IsMatch(box.Text, @"^[а-яА-Яa-zA-Z0-9]+") || box.Text.Length < 10)
						{
							box.BorderBrush = System.Windows.Media.Brushes.Red;
							AllCorrect = false;
						}
					}
					else if (box.Name == "Age")
					{
						if (!Regex.IsMatch(box.Text, @"^[0-9]+$"))
						{
							box.BorderBrush = System.Windows.Media.Brushes.Red;
							AllCorrect = false;
						}
					}
				}
				if (element is DatePicker)
				{
					DatePicker date = (DatePicker)element;
					date.BorderBrush = System.Windows.Media.Brushes.GreenYellow;
					if (!Regex.IsMatch(date.Text, @"^\d\d\.\d\d\.\d\d\d\d$"))
					{
						date.BorderBrush = System.Windows.Media.Brushes.Red;
						AllCorrect = false;
					}
					else if (date.SelectedDate < DateTime.Now)
					{
						date.BorderBrush = System.Windows.Media.Brushes.Red;
						AllCorrect = false;
					}
				}
				if (element is ComboBox)
				{
					ComboBox box = (ComboBox)element;
					box.BorderBrush = System.Windows.Media.Brushes.GreenYellow;
					if (box.SelectedIndex == -1) i++;
				}
			}
			if (i == 2) AllCorrect = false;
			if (AllCorrect)
			{
				if (AirDate.SelectedDate >= EndDate.SelectedDate) AllCorrect = false;
				else
				{
					CinemaContext db = new();
					Film film = db.Films.Find(IDfilm);
					film.NameFilm = Header.Text;
					film.Description = DescrBlock.Text;
					film.AgeLimit = int.Parse(Age.Text);
					film.AirDate = (DateTime)AirDate.SelectedDate;
					film.EndDate = (DateTime)EndDate.SelectedDate;
					film.Poster = PosterBytes;
					if (i == 1)
					{
						Genre genre = db.Genres.Find(IDfilm);
						if (Genre1.SelectedIndex != -1) genre.Genre1 = Genre1.Text;
						else if (Genre2.SelectedIndex != -1) genre.Genre1 = Genre2.Text;
					}
					else if (i == 2)
					{
						var genres = db.Genres.Where(item => item.IdFilm == IDfilm);
						foreach (Genre genre in genres)
						{
							db.Genres.Remove(genre);
						}
						db.Genres.Add(new Genre() { IdFilm = IDfilm, Genre1 = Genre1.Text });
						db.Genres.Add(new Genre() { IdFilm = IDfilm, Genre1 = Genre2.Text });
					}
					db.SaveChanges();
					MessageBox.Show("Данные обновлены");
				}
			}
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

		private void StartDeleteMovies(object sender, RoutedEventArgs e)
		{
			DeleteMovies.Visibility = Visibility.Visible;
			Hint.Text = "Выберите сеансы для удаления";
		}

		private void ClickMovie(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (DeleteMovies.Visibility == Visibility.Visible)
			{
				MovieButtonInfo movie = DeleteList.Find(item => item.buttonTime == button);
				if (movie is null)
				{
					MovieButtonInfo movieForDelete = buttons.Find(item => item.buttonTime == button);
					button.Background = System.Windows.Media.Brushes.Red;
					DeleteList.Add(movieForDelete);
				}
				else
				{
					DeleteList.Remove(movie);
					button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 226, 108));
				}
			}
			else
			{
				CinemaContext db = new();
				MovieButtonInfo info = buttons.Find(item => item.buttonTime == (Button)sender);
				Manager.adminwindow.MainFrame.Navigate(new AddMovie(db.Films.Find(IDfilm), info.stringMovie, 2));
			}
		}

		private void FinalDeleteMovies(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить эти сеансы?", "Подтверждение", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				CinemaContext db = new();
				foreach (MovieButtonInfo movie in DeleteList)
				{
					db.Movies.Remove(movie.stringMovie);
					buttons.Remove(movie);
					movie.panel.Children.Remove(movie.buttonTime);
				}
				db.SaveChanges();
				DeleteList.Clear();
				DeleteMovies.Visibility = Visibility.Hidden;
				Hint.Text = "Для изменения информации о сеансе нажмите на его кнопку";
			}
		}

		private void AddNewMovie(object sender, RoutedEventArgs e)
		{
			CinemaContext db = new();
			Manager.adminwindow.MainFrame.Navigate(new AddMovie(db.Films.Find(IDfilm), db.Movies.FirstOrDefault(), 1));
		}

		private void GoBack(object sender, RoutedEventArgs e)
		{
			Manager.adminwindow.MainFrame.Navigate(new FilmsShowAdmin());
		}
	}
}
