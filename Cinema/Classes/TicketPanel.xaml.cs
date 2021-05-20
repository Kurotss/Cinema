using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;


namespace Cinema.Classes
{
	public partial class TicketPanel : UserControl
	{
		public string TicketText { get; set; }
		public BitmapImage TicketImage { get; set; }
		public int IdTicket { get; set; }
		public TicketPanel(string name, DateTime date, byte[] image, int id)
		{
			InitializeComponent();
			TicketImage = Manager.CreateSource(image);
			TicketText = "\"" + name + "\"" + " " + date.ToString("M") + ", " +
					date.ToString("dddd") + ", " + date.ToString("t");
			IdTicket = id;
			if (date < DateTime.Now)
			{
				Refund.IsEnabled = false;
				Refund.Opacity = 0.5;
			}
			this.DataContext = this;
		}
		private void OpenTicket(object sender, RoutedEventArgs e)
		{
			ViewTicket viewTicket = new(TicketImage);
			viewTicket.Show();
		}

		private void DownloadTicket(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new() { Filter = "Image files(*.png)|*.png"};
			saveFileDialog.FileName = "TicketCinema";
			saveFileDialog.DefaultExt = ".png";
			if ((bool)saveFileDialog.ShowDialog())
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(TicketImage));
				using var stream = saveFileDialog.OpenFile();
				encoder.Save(stream);
			}
		}

		private void RefundTicket(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Вы точно хотите вернуть билет?", "Подтверждение", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				CinemaContext db = new();
				var tickets = (from Ticket in db.Tickets where Ticket.IdTicket == IdTicket select Ticket).ToList();
				Ticket ticket = tickets.FirstOrDefault();
				db.Tickets.Remove(ticket);
				db.SaveChanges();
				this.IsEnabled = false;
				this.Opacity = 0.5;
			}
		}
	}
}
