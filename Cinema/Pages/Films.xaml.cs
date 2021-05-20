using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cinema
{
	public partial class FilmsShow : Page
	{
		public List<Film_icon> film_Icons;
		public string SelectGenre = "";
		public DateTime SelectedDate = DateTime.Now;
		public int i = 0;
		public FilmsShow()
		{
			InitializeComponent();
			film_Icons = new List<Film_icon>();
			using CinemaContext db = new();
			var films = db.FilmsRaitingsGenres.ToList();
			foreach (FilmsRaitingsGenre film in films)
			{
				if (film.EndDate > DateTime.Now)
				{
					Film filmString = db.Films.Find(film.IdFilm);
					Film_icon film_Icon = new(Manager.CreateSource(film.Poster), film.NameFilm, film.Genre,
						Math.Round(film.Rating, 1).ToString(), (int)film.AgeLimit, 1, filmString);
					film_Icon.Margin = new Thickness(5, 5, 5, 5);
					film_Icons.Add(film_Icon);
					wrappanel.Children.Add(film_Icon);
				}
			}
			var genres = db.AllGenres.ToList();
			foreach (AllGenre genre in genres)
			{
				RadioButton radioButton = new RadioButton();
				radioButton.Content = genre.Genre;
				radioButton.Checked += GenreChecked;
				Films_genre.Children.Add(radioButton);
			}
		}
		private void GenreChecked(object sender, RoutedEventArgs e)
		{
			if (sender is RadioButton item)
			{
				SelectGenre = item.Content.ToString();
				Search();
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Search_box.TextChanged += InputSearch;
		}

		private void Clear(object sender, RoutedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			textBox.FontStyle = FontStyles.Normal;
			textBox.Foreground = new SolidColorBrush(Color.FromRgb(59, 59, 59));
			textBox.Text = "";
		}

		private void Search()
		{
			wrappanel.Children.Clear();
			using CinemaContext db = new();
			SqlParameter isDate = new("@filters", "0");
			if (i != 0) isDate = new("@filters", "1");
			SqlParameter MovieDate = new("@date_movie", SelectedDate);
			var films = db.Films.FromSqlRaw("SearchMovies @date_movie, @filters", MovieDate, isDate).ToList();
			foreach (Film film in films)
			{
				Film_icon film_Icon = film_Icons.Find(item => item.Name_film == film.NameFilm);
				if (film_Icon is not null)
				{
					if (Search_box.Text == "" || Search_box.FontStyle == FontStyles.Italic|| film_Icon.Name_film.ToLower().StartsWith(Search_box.Text.ToLower()))
					{
						if (Age.Text == "" || film_Icon.Age_limit == int.Parse(Age.Text))
						{
							if (SelectGenre == "" || film_Icon.Genres_list.Contains(SelectGenre))
							{
								wrappanel.Children.Add(film_Icon);
							}
						}
					}
				}
			}
		}

		private void InputSearch(object sender, TextChangedEventArgs e)
		{
			Search();
		}

		private void CalendarSearch(object sender, SelectionChangedEventArgs e)
		{
			Calendar calendar = (Calendar)sender;
			SelectedDate = (DateTime)calendar.SelectedDate;
			i++;
			Search();
		}

		private void OnlyNumbers(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			if (e.Text[0] < '0' || e.Text[0] > '9')
			{
				e.Handled = true;
			}
		}

		private void ChangeAge(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			if (Age.Text == "") Age.Text = "0";
			switch (button.Content.ToString()[0])
			{
				case '+':
					if (Int32.Parse(Age.Text) + 1 <= 21)
					{
						Age.Text = (Int32.Parse(Age.Text) + 1).ToString();
						Search();
					}
					break;
				case '-':
					if (Int32.Parse(Age.Text) - 1 >= 0)
					{
						Age.Text = (Int32.Parse(Age.Text) - 1).ToString();
						Search();
					}
					break;
			}
		}
		private void InputAge(object sender, TextChangedEventArgs e)
		{
			Search();
		}

		private void ClearSearch(object sender, RoutedEventArgs e)
		{
			Age.Text = "";
			Search_box.Text = "";
			CalendarForSearch.DisplayDate = DateTime.Now;
			i = 0;
			foreach (FrameworkElement element in Films_genre.Children)
			{
				if (element is RadioButton)
				{
					RadioButton radio = (RadioButton)element;
					if ((bool)radio.IsChecked) radio.IsChecked = false;
					SelectGenre = "";
				}
			}
			Search();
		}
	}
}