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
using System.Windows.Shapes;
using Cinema.Pages;

namespace Cinema
{
	/// <summary>
	/// Логика взаимодействия для FirstWindow.xaml
	/// </summary>
	public partial class FirstWindow : Window
	{
		public FirstWindow()
		{
			InitializeComponent();
			Manager.window = this;
			MainFrame.Navigate(new Login());
		}
		private void Reg(object sender, RoutedEventArgs e)
		{
			Register register = new();
			register.Show();
		}

		private void Log(object sender, RoutedEventArgs e)
		{
			MainFrame.Navigate(new Login());
		}
	}
}
