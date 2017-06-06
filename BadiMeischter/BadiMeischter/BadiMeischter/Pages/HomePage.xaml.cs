﻿﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using BadiMeischter.Model;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace BadiMeischter.Pages
{
    public partial class HomePage : ContentPage, INotifyPropertyChanged
    {
        private const string Url = "https://data.stadt-zuerich.ch/dataset/freibad/resource/c9d56476-344e-4081-af86-0b38a3cc8ccd/download/freibad.json";
        private ObservableCollection<Badi> _badiList;
        private string searchText = string.Empty;
        public ObservableCollection<Badi> _badiCopy;

        public HomePage()
        {
            InitializeComponent();
            BindingContext = this;
            BadiList = new System.Collections.ObjectModel.ObservableCollection<Badi>();
            BadiListView.Footer = string.Empty;
        }

        #region Overrides of Page

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var result = "";

            var client = new HttpClient();
            result = await client.GetStringAsync(Url);

            var json = JsonConvert.DeserializeObject<Base>(result).Features;

            BadiList = new ObservableCollection<Badi>(json);
            _badiCopy = new ObservableCollection<Badi>(BadiList);
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
            Navigation.PushAsync(new BadiDetailPage { Item = (Badi) e.SelectedItem });

            ((ListView)sender).SelectedItem = null;
        }

		#endregion

		private void ApplyFilter()
		{
			var filteredBadi = new ObservableCollection<Badi>();
            foreach (var badi in _badiCopy.Where(x => x.Properties.Name.ToLowerInvariant().Contains(searchText.ToLowerInvariant())).Select(x => x))
			{
                filteredBadi.Add(badi);
			}

            if (filteredBadi != null)
            {
                BadiList = filteredBadi;
            }
        }

		public ObservableCollection<Badi> BadiList
		{
			get { return _badiList; }
			set
			{
				_badiList = value;
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
