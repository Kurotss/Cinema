using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cinema
{
	public partial class Login : Page
	{
		public Login()
		{
			InitializeComponent();
		}

		private void Log(object sender, RoutedEventArgs e)
		{
			using (CinemaContext db = new CinemaContext())
			{
				SqlParameter login = new("@login", LoginBox.Text);
				SqlParameter password = new("@password", Password.Password);
				var users = db.UsersNames.FromSqlRaw("LogIN @login, @password", login, password).ToList();
				if (users.Count == 0)
				{
					MessageBox.Show("Такого пользователя не существует");
				}
				else
				foreach (UsersName user in users)
				{
					switch (user.IdRole)
					{
						case 1:
							MainWindow main = new MainWindow(user.NameClient + " " + user.Surname);
							main.Show();
							Manager.window.Close();
							Manager.Email = user.Email;
							break;
						case 2:
							AdminWindow window = new AdminWindow();
							window.Show();
							Manager.window.Close();
							break;
					}
				}
			}
		}
	}
}
