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

namespace Cinema
{
	/// <summary>
	/// Логика взаимодействия для MoviesAndDates.xaml
	/// </summary>
	public partial class MoviesAndDates : UserControl
	{
		public string dateForMovies { get; set; }
		public WrapPanel panel;
		public MoviesAndDates(string date)
		{
			InitializeComponent();
			this.dateForMovies = date;
			this.DataContext = this;
			panel = ButtonMovies;
		}
	}
}
