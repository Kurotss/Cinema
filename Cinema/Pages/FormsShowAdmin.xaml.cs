using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	/// Логика взаимодействия для FormsShowAdmin.xaml
	/// </summary>
	public partial class FormsShowAdmin : Page
	{
		public FormsShowAdmin()
		{
			InitializeComponent();
			CinemaContext db = new();
			FormsList.ItemsSource = db.Forms.ToList();
		}

		private void StartDelete(object sender, RoutedEventArgs e)
		{
			Tip.Visibility = Visibility.Visible;
			DeleteForms.Visibility = Visibility.Visible;
		}

		private void FinalDelete(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить эти заявки?", "Подтверждение", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				CinemaContext db = new();
				foreach (var item in FormsList.SelectedItems)
				{
					Form form = item as Form;
					FormsList.Items.Remove(item);
					db.Forms.Remove(form);
				}
				db.SaveChanges();
				Tip.Visibility = Visibility.Hidden;
				DeleteForms.Visibility = Visibility.Hidden;
			}
		}
	}
}
