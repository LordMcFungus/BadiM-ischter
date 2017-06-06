using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BadiMeischter.Model;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BadiMeischter.Pages
{
	public partial class HallenbadHomePage : ContentPage, INotifyPropertyChanged
	{
		private const string Url = "https://data.stadt-zuerich.ch/dataset/hallenbad/resource/ab6b84d2-710b-43c2-b50e-4bfa0448a4f0/download/hallenbad.json";
		private System.Collections.ObjectModel.ObservableCollection<Badi> _badiList;

		public HallenbadHomePage()
		{
			InitializeComponent();
			BindingContext = this;
			HallenbadList = new System.Collections.ObjectModel.ObservableCollection<Badi>();
			HallenbadListView.Footer = string.Empty;
		}

		public ObservableCollection<Badi> HallenbadList
		{
			get { return _badiList; }
			set
			{
				_badiList = value;
				RaisePropertyChanged();
			}
		}

		#region Overrides of Page

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var result = "";

			var client = new HttpClient();
			result = await client.GetStringAsync(Url);


			if (result == "")
				result = "[{\"Name\": \"Keine Daten vorhanden\"}]";

			var json = JsonConvert.DeserializeObject<Base>(result).Features;

			HallenbadList = new System.Collections.ObjectModel.ObservableCollection<Badi>(json);
		}

		#endregion

		#region INotifyPropertyChanged

		public new event PropertyChangedEventHandler PropertyChanged = delegate { };

		private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void OnSelection(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) return;
			Navigation.PushAsync(new BadiDetailPage { Item = (Badi)e.SelectedItem });

			((ListView)sender).SelectedItem = null;
		}

		#endregion
	}
}