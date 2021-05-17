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

namespace Cinema.Classes
{
	/// <summary>
	/// Логика взаимодействия для RealTicket.xaml
	/// </summary>
	public partial class RealTicket : UserControl
	{
		public RealTicket()
		{
			InitializeComponent();
		}
		public RealTicket(string nameFilm, DateTime movieTime, int room, int row, int place, int cost)
		{
			InitializeComponent();
			Title1.Text = Title2.Text = nameFilm;
			Date.Text = movieTime.ToString("ddd") + ", "+ movieTime.ToString("M");
			Time.Text = movieTime.ToString("t");
			DateAndTime.Text = movieTime.ToString("M") + ", " + movieTime.ToString("t");
			Room1.Text = Room2.Text = room.ToString();
			Row1.Text = Row2.Text = row.ToString();
			Place1.Text = Place2.Text = place.ToString();
			Price1.Text = Price2.Text = cost.ToString() + " ₽";
		}
	}
}
