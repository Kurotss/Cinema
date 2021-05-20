using System;
using System.Collections.Generic;
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

namespace Cinema.Pages
{
	public partial class AddMovie : Page
	{
		public Film film { get; set; }
		public Movie movie { get; set; }
		public int choice { get; set; }
		public AddMovie(Film arg, Movie arg1, int i)
		{
			InitializeComponent();
			this.film = arg;
			this.movie = arg1;
			this.choice = i;
			Period.Text = Period.Text + film.AirDate.ToString("dd.MM.yyyy") + "-" + film.EndDate.ToString("dd.MM.yyyy");
			if (i == 2)
			{
				DateInput.SelectedDate = movie.MovieTime;
				RoomInput.Text = movie.IdRoom.ToString();
				TimeInput.Text = movie.MovieTime.ToString("hh:mm");
				AddUpd.Content = "Обновить сеанс";
			}
		}

		private void AddNewMovie(object sender, RoutedEventArgs e)
		{
			CinemaContext db = new();
			bool AllCorrect = true;
			DateInput.BorderBrush = Brushes.GreenYellow;
			RoomInput.BorderBrush = Brushes.GreenYellow;
			TimeInput.BorderBrush = Brushes.GreenYellow;
			if (!Regex.IsMatch(DateInput.Text, @"^\d\d\.\d\d\.\d\d\d\d$"))
			{
				AllCorrect = false;
				DateInput.BorderBrush = Brushes.Red;
			}
			else if (DateInput.SelectedDate > film.EndDate || DateInput.SelectedDate < film.AirDate)
			{
				AllCorrect = false;
				DateInput.BorderBrush = Brushes.Red;
			}
			if (!Regex.IsMatch(RoomInput.Text, @"^[0-9]$"))
			{
				AllCorrect = false;
				RoomInput.BorderBrush = Brushes.Red;
			}
			else if (int.Parse(RoomInput.Text) > db.Rooms.Max(item => item.IdRoom))
			{
				AllCorrect = false;
				RoomInput.BorderBrush = Brushes.Red;
			}
			if (!Regex.IsMatch(TimeInput.Text, @"^\d\d\:\d\d$"))
			{
				AllCorrect = false;
				TimeInput.BorderBrush = Brushes.Red;
			}
			else if (double.Parse(TimeInput.Text.Substring(0,2)) > 23 || double.Parse(TimeInput.Text.Substring(3,2)) > 59)
			{
				AllCorrect = false;
				TimeInput.BorderBrush = Brushes.Red;
			}
			if (AllCorrect)
			{
				if (choice == 1)
				{
					DateTime date = (DateTime)DateInput.SelectedDate;
					Movie movie = new()
					{
						IdFilm = film.IdFilm,
						IdRoom = int.Parse(RoomInput.Text),
						MovieTime = date.AddHours(double.Parse(TimeInput.Text.Substring(0, 2))).AddMinutes(double.Parse(TimeInput.Text.Substring(3, 2)))
					};
					db.Movies.Add(movie);
					db.SaveChanges();
					MessageBox.Show("Новый сеанс успешно добавлен!");
				}
				else
				{
					Movie newMovie = db.Movies.Find(movie.IdMovie);
					newMovie.IdRoom = int.Parse(RoomInput.Text);
					DateTime date = (DateTime)DateInput.SelectedDate;
					newMovie.MovieTime = date.AddHours(double.Parse(TimeInput.Text.Substring(0, 2))).AddMinutes(double.Parse(TimeInput.Text.Substring(3, 2)));
					db.SaveChanges();
				}
			}
		}
	}
}
