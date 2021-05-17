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
using System.Windows.Shapes;

namespace Cinema
{
	public partial class Register : Window
	{
		public Register()
		{
			InitializeComponent();
		}

		private void RegButton(object sender, RoutedEventArgs e)
		{
			bool AllCorrect = true;
			string Password = "";
			string PasswordSecond = "";
			foreach (FrameworkElement element in RegForm.Children)
			{
				if (element is TextBox box)
				{
					box.BorderBrush = Brushes.GreenYellow;
					if (box.Name == "NameInput" || box.Name == "SurnameInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[a-zA-Z]+$") && !Regex.IsMatch(box.Text, @"^[а-яА-Я]+$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
					if (box.Name == "TelephonInput")
					{
						if (!Regex.IsMatch(box.Text, @"^\+\d\(\d\d\d\)\d\d\d\-\d\d\-\d\d$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
					if (box.Name == "EmailInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[^\.\ ]\S+\@[a-zA-Z0-9]\S+\.\S+$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
				}
				if (element is PasswordBox password)
				{
					password.BorderBrush = Brushes.GreenYellow;
					if (!Regex.IsMatch(password.Password, @"^[^\@\'\ а-яА-Я]+$"))
					{
						password.BorderBrush = Brushes.Red;
						AllCorrect = false;
					}
					else
					{
						if (password.Name == "PasswordInput") Password = password.Password;
						else if (password.Name == "PasswordSecondInput") PasswordSecond = password.Password;
					}
				}
			}
			ErrorInPasswords.Visibility = Visibility.Hidden;
			if (Password != PasswordSecond)
			{
				AllCorrect = false;
				ErrorInPasswords.Visibility = Visibility.Visible;
			}
			if (AllCorrect)
			{
				User user = new() { Email = EmailInput.Text, PasswordUser = Password, IdRole = 1};
				Client client = new() { Surname = SurnameInput.Text, NameClient = NameInput.Text, NumberTelephon = TelephonInput.Text, Email = EmailInput.Text};
				CinemaContext db = new();
				db.Users.Add(user);
				db.Clients.Add(client);
				db.SaveChanges();
				MessageBox.Show("Вы успешно зарегистрированы!");
				MainWindow main = new(NameInput.Text + " " + SurnameInput.Text);
				main.Show();
				Manager.Email = EmailInput.Text;
				this.Close();
				foreach (Window window in Application.Current.Windows)
				{
					if (window is FirstWindow) window.Close();
				}
			}
		}
	}
}
