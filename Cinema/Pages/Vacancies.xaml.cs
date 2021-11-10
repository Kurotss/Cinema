using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cinema.Pages
{
	public partial class Vacancies : Page
	{
		public Vacancies()
		{
			InitializeComponent();
			CinemaContext db = new();
			var vacancies = from staffers in db.staff
							join posts in db.Posts on staffers.IdPost equals posts.IdPost
							join state in db.StateCinemas on posts.IdPost equals state.IdPost
							group staffers by new { posts.NamePost, state.CountStaffers } into g
							select new
							{
								NamePost = g.Key.NamePost,
								CountStaffers = g.Count(),
								AllCount = g.Key.CountStaffers
							};
			foreach (var t in vacancies)
			{
				if (t.AllCount - t.CountStaffers != 0)
				{
					ListVacancies.Children.Add(new TextBlock() { Text = t.NamePost.ToString() });
					ListForChoice.Items.Add(t.NamePost);
				}
			}
		}

		private void UpperFirstLetter(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (textBox.Text != null && textBox.Text.Length > 0)
			{
				textBox.Text = textBox.Text.Substring(0, 1).ToUpper() + textBox.Text[1..].ToLower();
			}
		}

		private void SendForm(object sender, RoutedEventArgs e)
		{
			int PostId = 0;
			bool AllCorrect = true;
			foreach (FrameworkElement element in Form.Children)
			{
				if (element is TextBox)
				{
					TextBox box = (TextBox)element;
					box.BorderBrush = Brushes.GreenYellow;
					if (box.Name == "NameInput" || box.Name == "SurnameInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[a-zA-Z]+$") && !Regex.IsMatch(box.Text, @"^[а-яА-Я]+$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
					if (box.Name == "TelephonInput")
					{
						if (!Regex.IsMatch(box.Text, @"^\+\d\(\d\d\d\)\d\d\d\-\d\d\-\d\d$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
					if (box.Name == "AgeInput")
					{
						if (Regex.IsMatch(box.Text, @"^\d+$"))
						{
							if (int.Parse(box.Text) < 18)
							{
								AllCorrect = false;
								box.BorderBrush = Brushes.Red;
							}
						}
						else
						{
							MessageBox.Show(box.Text);
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
					if (box.Name == "EmailInput")
					{
						if (!Regex.IsMatch(box.Text, @"^[^\.\ ]\S+\@[a-zA-Z0-9]\S+\.\S+$"))
						{
							AllCorrect = false;
							box.BorderBrush = Brushes.Red;
						}
					}
				}
				if (element is ComboBox)
				{
					ComboBox box = (ComboBox)element;
					if (box.SelectedIndex == -1)
					{
						AllCorrect = false;
						box.BorderBrush = Brushes.Red;
					}
					else
					{
						CinemaContext db = new();
						var posts = from post in db.Posts where post.NamePost == ListForChoice.Text select post.IdPost;
						PostId = posts.FirstOrDefault();
					}
				}
			}
			if (AllCorrect)
			{
				CinemaContext db = new();
				Form form = new() { NameClient = NameInput.Text, Surname = SurnameInput.Text, Telephon = TelephonInput.Text,
					Age = int.Parse(AgeInput.Text), Email = EmailInput.Text, IdPost = PostId };
				db.Forms.Add(form);
				db.SaveChanges();
			}
		}
	}
}
