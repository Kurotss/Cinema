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
using Cinema.Pages;

namespace Cinema
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(string Name)
		{
			InitializeComponent();
			Head.Text = "Добро пожаловать, " + Name +"!";
			Manager.mainwindow = this;
		}

		private void Exit(object sender, RoutedEventArgs e)
		{
			Manager.Email = "";
			FirstWindow window = new FirstWindow();
			window.Show();
			this.Close();
			foreach (Window window1 in Application.Current.Windows)
			{
				window1.Close();
			}
		}

		private void Films_open(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new FilmsShow());
		}
		public void PageOpen(Page page)
		{
			MainFrame.Navigate(page);
		}

		private void Account_open(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Account());
		}

		private void Vacancies_open(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Vacancies());
		}

		private void Contacts_open(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Contact());
		}
	}
}
