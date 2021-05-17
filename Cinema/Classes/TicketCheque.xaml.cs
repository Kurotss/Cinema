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
	/// Логика взаимодействия для TicketCheque.xaml
	/// </summary>
	public partial class TicketCheque : UserControl
	{
		public TicketCheque(string number, string cost)
		{
			InitializeComponent();
			NumberPlace.Text = number;
			Cost.Text = cost;
		}
	}
}
