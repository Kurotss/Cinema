using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Cinema.Classes;

namespace Cinema.Pages
{
	public partial class Account : Page
	{
		public string NameClient { get; set; }
		public string SurnameClient { get; set; }
		public string TelephonClient { get; set; }
		public string EmailClient { get; set; }
		public string PasswordClient { get; set; }
		public string NameChangeBox { get; set; }
		public Account()
		{
			InitializeComponent();
			CinemaContext db = new();
			var PersonalData = (from UsersName in db.UsersNames where UsersName.Email == Manager.Email select UsersName).ToList();
			UsersName user = PersonalData.FirstOrDefault();
			NameClient = user.NameClient;
			SurnameClient = user.Surname;
			TelephonClient = user.NumberTelephon;
			EmailClient = user.Email;
			PasswordClient = user.PasswordUser;
			this.DataContext = this;
			var Tickets = db.Tickets.Join(db.MoviesSorts, 
				t => t.IdMovie,
				m => m.IdMovie,
				(t, m) => new
				{
					m.NameFilm,
					m.MovieTime,
					t.ImageTicket,
					t.IdTicket,
					t.Email
				}).Where(t => t.Email == Manager.Email);
			foreach (var ticket in Tickets)
			{
				TicketPanel panel = new(ticket.NameFilm, ticket.MovieTime, ticket.ImageTicket, ticket.IdTicket);
				Border border = new() { BorderBrush = Brushes.White, BorderThickness = new Thickness(0, 0.5, 0, 0.5) };
				border.Child = panel;
				TicketsList.Children.Add(border);
			}
		}

		private void ToUpperFirstLetter(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (textBox.Text != null && textBox.Text.Length > 0)
			{
				textBox.Text = textBox.Text.Substring(0, 1).ToUpper() + textBox.Text.Substring(1).ToLower();
			}
		}

		private void SaveChanges(object sender, RoutedEventArgs e)
		{
			bool AllCorrect = true;
			foreach (FrameworkElement element in FieldsAccount.Children)
			{
				if (element is TextBox)
				{
					TextBox box = (TextBox)element;
					box.BorderBrush = Brushes.GreenYellow;
					if (box.Name == "EmailInput" && box.IsEnabled)
					{
						if (!Regex.IsMatch(box.Text, @"^[^\.\ ]\S+\@[a-zA-Z0-9]\S+\.\S+$"))
						{
							box.BorderBrush = Brushes.Red;
							AllCorrect = false;
						}
						else
						{
							box.IsEnabled = false;
							EmailClient = box.Text;
						}
					}
					else if (box.Name == "TelephonInput")
					{
						if (!Regex.IsMatch(box.Text, @"^\+\d\(\d\d\d\)\d\d\d\-\d\d\-\d\d$"))
						{
							box.BorderBrush = Brushes.Red;
							AllCorrect = false;
						}
						else TelephonClient = box.Text;
					}
					else
					{
						if (!Regex.IsMatch(box.Text, @"^[a-zA-Z]+$") && !Regex.IsMatch(box.Text, @"^[а-яА-Я]+$"))
						//^ - символы в начале строки, + - 1 и больше раз может повторяться, $ - в конце строки
						{
							box.BorderBrush = Brushes.Red;
							AllCorrect = false;
						}
						else
						{
							if (box.Name == "NameClient") NameClient = box.Text;
							else if (box.Name == "SurnameClient") SurnameClient = box.Text;
						}
					}
				}
				if (element.Name == "PasswordInput" && element.IsEnabled)
				{
					PasswordBox box = (PasswordBox)element;
					box.BorderBrush = Brushes.GreenYellow;
					if (!Regex.IsMatch(box.Password, @"^[^\@\'\ а-яА-Я]+$"))
					{
						box.BorderBrush = Brushes.Red;
						AllCorrect = false;
					}
					else
					{
						box.IsEnabled = false;
						PasswordClient = box.Password;
						box.Password = null;
					}
				}
			}
			if (AllCorrect)
			{
				CinemaContext db = new();
				User user = db.Users.Find(Manager.Email);
				Client client = db.Clients.Find(Manager.Email);
				user.Email = EmailClient;
				user.PasswordUser = PasswordClient;
				client.NameClient = NameClient;
				client.Surname = SurnameClient;
				client.NumberTelephon = TelephonClient;
				db.SaveChanges();
			}
		}

		private void LettersOnly(object sender, TextCompositionEventArgs e)
		{
			if (!Regex.IsMatch(e.Text, @"[a-zA-Z]") && !Regex.IsMatch(e.Text, @"[а-яА-Я]"))
			{
				e.Handled = true;
			}
		}

		private void ChangeImportantInfo(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			PasswordBoxConf.Visibility = Visibility.Visible;
			PasswordWatermarkConf.Visibility = Visibility.Visible;
			ButtonInput.Visibility = Visibility.Visible;
			NameChangeBox = button.Name + "Input";
		}

		private void Clear(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			button.Visibility = Visibility.Hidden;
		}

		private void CheckPassword(object sender, RoutedEventArgs e)
		{
			PasswordBoxConf.BorderBrush = Brushes.GreenYellow;
			if (PasswordBoxConf.Password == PasswordClient)
			{
				PasswordBoxConf.Visibility = Visibility.Hidden;
				PasswordWatermarkConf.Visibility = Visibility.Hidden;
				ButtonInput.Visibility = Visibility.Hidden;
				PasswordBoxConf.Password = null;
				FrameworkElement box = (FrameworkElement)FieldsAccount.FindName(NameChangeBox);
				box.IsEnabled = true;
			}
			else PasswordBoxConf.BorderBrush = Brushes.Red;
		}
	}
}
