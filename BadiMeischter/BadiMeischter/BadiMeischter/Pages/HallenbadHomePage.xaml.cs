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
		private ObservableCollection<Badi> _hallenbadList;
		private string searchText = string.Empty;
		public ObservableCollection<Badi> _hallenbadCopy;

		public HallenbadHomePage()
		{
			InitializeComponent();
			BindingContext = this;
			HallenbadList = new ObservableCollection<Badi>();
			HallenbadListView.Footer = string.Empty;
		}

		#region Overrides of Page

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			var result = "";

			var client = new HttpClient();
			result = await client.GetStringAsync(Url);

			var json = JsonConvert.DeserializeObject<Base>(result).Features;

			HallenbadList = new ObservableCollection<Badi>(json);
            _hallenbadCopy = HallenbadList;
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

		private void ApplyFilter()
		{
			var filteredHallenbad = new ObservableCollection<Badi>();
            foreach (var hallenbad in _hallenbadCopy.Where(x => x.Properties.Name.ToLowerInvariant().Contains(searchText.ToLowerInvariant())).Select(x => x))
			{
				filteredHallenbad.Add(hallenbad);
			}

			if (filteredHallenbad != null)
			{
				HallenbadList = filteredHallenbad;
			}
		}

		public ObservableCollection<Badi> HallenbadList
		{
			get { return _hallenbadList; }
			set
			{
				_hallenbadList = value;
				RaisePropertyChanged();
			}
		}

		public string SearchText
		{
			get { return searchText; }
			set
			{
				if (Equals(searchText, value)) return;
				searchText = value;
				OnPropertyChanged();

				ApplyFilter();
			}
		}
	}
}