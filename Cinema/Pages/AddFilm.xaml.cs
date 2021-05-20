using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Cinema.Pages
{
	public partial class AddFilm : Page
	{
		public byte[] PosterBytes = null;
		public AddFilm()
		{
			InitializeComponent();
			CinemaContext db = new();
			foreach (AllGenre genre in db.AllGenres)
			{
				GenreInput.Items.Add(genre.Genre);
			}
		}

		private void LoadImage(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new() { Filter = "Image files(*.png)|*.png" };
			if ((bool)dialog.ShowDialog())
			{
				try
				{
					PosterInput.Text = dialog.FileName;
					System.Drawing.Image image = System.Drawing.Image.FromFile(dialog.FileName);
					ImageConverter converter = new();
					PosterBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
				}
				catch
				{
					ImageError.Visibility = Visibility.Visible;
				}
			}
		}

		private void AddNewFilm(object sender, RoutedEventArgs e)
		{
			bool AllCorrect = true;
			ImageError.Visibility = Visibility.Hidden;
			foreach (var element in grid.Children)
			{
				if (element is TextBox)
				{
					TextBox box = (TextBox)element;
					box.BorderBrush = System.Windows.Media.Brushes.GreenYellow;
					if (box.Name == "NameInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[а-яА-Яa-zA-Z0-9]+") || box.Text.Length < 2)
						{
							box.BorderBrush = System.Windows.Media.Brushes.Red;
							AllCorrect = false;
						}
					}
					else if (box.Name == "DescriptionInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[а-яА-Яa-zA-Z0-9]+") || box.Text.Length < 10)
						{
							box.BorderBrush = System.Windows.Media.Brushes.Red;
							AllCorrect = false;
						}
					}
					else if (box.Name == "AgeInput")
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
			}
			if (PosterBytes is null)
			{
				PosterInput.BorderBrush = System.Windows.Media.Brushes.Red;
				AllCorrect = false;
			}
			if (GenreInput.SelectedIndex == -1) AllCorrect = false;
			if (AllCorrect)
			{
				if (AirDate.SelectedDate >= EndDate.SelectedDate) AllCorrect = false;
				else
				{
					CinemaContext db = new();
					Film film = new()
					{
						NameFilm = NameInput.Text,
						Description = DescriptionInput.Text,
						AgeLimit = int.Parse(AgeInput.Text),
						AirDate = (DateTime)AirDate.SelectedDate,
						EndDate = (DateTime)EndDate.SelectedDate,
						Poster = PosterBytes
					};
					db.Films.Add(film);
					db.SaveChanges();
					Film IDfilm = db.Films.OrderBy(item => item.IdFilm).Last();
					Rating rating = new() { IdFilm = IDfilm.IdFilm, Rating1 = 0, CountOfPeopleInRaiting = 0 };
					Genre genre = new() { IdFilm = IDfilm.IdFilm, Genre1 = GenreInput.Text };
					db.Ratings.Add(rating);
					db.Genres.Add(genre);
					db.SaveChanges();
					MessageBox.Show("Фильм успешно добавлен!");
				}
			}
		}
	}
}
