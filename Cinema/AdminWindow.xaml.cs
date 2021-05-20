using System.Windows;
using System.Windows.Controls;
using Cinema.Pages;

namespace Cinema
{
	public partial class AdminWindow : Window
	{
		public AdminWindow()
		{
			InitializeComponent();
			Manager.adminwindow = this;
			MainFrame.Navigate(new FilmsShowAdmin());
		}
		private void Exit(object sender, RoutedEventArgs e)
		{
			Manager.Email = "";
			FirstWindow window = new FirstWindow();
			window.Show();
			this.Close();
		}

		private void FilmsOpen(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new FilmsShowAdmin());
		}

		public void PageOpen(Page page)
		{
			MainFrame.Navigate(page);
		}

		private void FormsOpen(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new FormsShowAdmin());
		}

		private void StaffOpen(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new StaffShow());
		}
	}
}
