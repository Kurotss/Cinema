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
	/// <summary>
	/// Логика взаимодействия для StaffShow.xaml
	/// </summary>
	public partial class StaffShow : Page
	{
		public int maxIdPost { get; set; }
		public StaffShow()
		{
			InitializeComponent();
			CinemaContext db = new();
			StaffersTable.ItemsSource = db.staff.ToList();
			maxIdPost = db.Posts.Max(item => item.IdPost);
			StateTable.ItemsSource = db.StateCinemas.ToList();
		}
		private void StartDelete(object sender, RoutedEventArgs e)
		{
			Tip.Visibility = Visibility.Visible;
			DeleteStaff.Visibility = Visibility.Visible;
		}

		private void FinalDelete(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить эти заявки?", "Подтверждение", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				CinemaContext db = new();
				foreach (var item in StaffersTable.SelectedItems)
				{
					staff form = item as staff;
					db.staff.Remove(form);
				}
				db.SaveChanges();
				StaffersTable.ItemsSource = db.staff.ToList();
				Tip.Visibility = Visibility.Hidden;
				DeleteStaff.Visibility = Visibility.Hidden;
			}
		}

		private void EditStaff(object sender, DataGridRowEditEndingEventArgs e)
		{
			CinemaContext db = new();
			staff staff = e.Row.Item as staff;
			bool AllCorrect = true;
			if (!Regex.IsMatch(staff.Surname, @"^[a-zA-Z]+$") && !Regex.IsMatch(staff.Surname, @"^[а-яА-Я]+$"))
			{
				AllCorrect = false;
			}
			if (!Regex.IsMatch(staff.NameStaffer, @"^[a-zA-Z]+$") && !Regex.IsMatch(staff.NameStaffer, @"^[а-яА-Я]+$"))
			{
				AllCorrect = false;
			}
			if (!Regex.IsMatch(staff.NumberTelephon, @"^\+\d\(\d\d\d\)\d\d\d\-\d\d\-\d\d$"))
			{
				AllCorrect = false;
			}
			if (!Regex.IsMatch(staff.Email, @"^[^\.\ ]\S+\@[a-zA-Z0-9]\S+\.\S+$"))
			{
				AllCorrect = false;
			}
			if (!Regex.IsMatch(staff.IdPost.ToString(), @"^\d+$"))
			{
				AllCorrect = false;
			}
			else if (staff.IdPost > maxIdPost)
			{
				AllCorrect = false;
			}
			int c = db.staff.Count(u => u.IdPost == staff.IdPost);
			StateCinema stateCinema = db.StateCinemas.Find(staff.IdPost);
			if ((stateCinema.CountStaffers - c) == 0) AllCorrect = false;
			if (AllCorrect)
			{
				if (staff.IdStaffer == 0) db.staff.Add(staff);
				else
				{
					staff updateStaff = db.staff.Find(staff.IdStaffer);
					updateStaff = new (){ Surname = staff.Surname, NameStaffer = staff.NameStaffer, NumberTelephon = staff.NumberTelephon,
					Email = staff.Email, IdPost = staff.IdPost};
				}
				db.SaveChanges();
				MessageBox.Show("Данные успешно обновлены!");
			}
			else
			{
				MessageBox.Show("Ошибка добавления данных! Некорректные значения.");
			}
		}

		private void EditState(object sender, DataGridRowEditEndingEventArgs e)
		{
			CinemaContext db = new();
			StateCinema state = e.Row.Item as StateCinema;
			if (Regex.IsMatch(state.CountStaffers.ToString(), @"^\d+$"))
			{
				StateCinema updateState = db.StateCinemas.Find(state.IdPost);
				updateState.CountStaffers = state.CountStaffers;
				db.SaveChanges();
				MessageBox.Show("Данные успешно обновлены!");
			}
			else
			{
				MessageBox.Show("Ошибка добавления данных! Некорректные значения.");
			}
		}
	}
}
