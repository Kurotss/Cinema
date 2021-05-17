using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Drawing;

namespace Cinema
{
	class Manager
	{
		public static FirstWindow window { get; set; }
		public static MainWindow mainwindow { get; set; }
		public static AdminWindow adminwindow { get; set; }
		public static string Email { get; set; }
		public static BitmapImage CreateSource(byte[] bytes)
		{
			var bi = new BitmapImage();
			MemoryStream stream = new MemoryStream(bytes);
			Bitmap bmp = (Bitmap)System.Drawing.Image.FromStream(stream);
			using (var ms = new MemoryStream())
			{
				bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				ms.Position = 0;
				bi.BeginInit();
				bi.CacheOption = BitmapCacheOption.OnLoad;
				bi.StreamSource = ms;
				bi.EndInit();
			}
			return bi;
		}
		public static void Animation(Canvas canvas, double From, double To, int i)
		{
			DoubleAnimation doubleAnimation = new();
			doubleAnimation.From = From;
			doubleAnimation.To = To;
			doubleAnimation.Duration = TimeSpan.FromSeconds(0.25);
			foreach (FrameworkElement element in canvas.Children)
			{
				if (element is Button && i == 0 && element.Name == "RatButton")
				{
					element.BeginAnimation(Button.HeightProperty, doubleAnimation);
				}
				else if (element is System.Windows.Controls.Image && i == 1)
				{
					element.BeginAnimation(System.Windows.Controls.Image.HeightProperty, doubleAnimation);
				}
				else if (element is System.Windows.Controls.Image && i == 2)
				{
					element.BeginAnimation(System.Windows.Controls.Image.WidthProperty, doubleAnimation);
				}
			}
		}
	}
}
