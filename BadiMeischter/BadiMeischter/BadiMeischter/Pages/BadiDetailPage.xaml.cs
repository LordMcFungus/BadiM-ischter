﻿using System;
using System.Collections.Generic;
using BadiMeischter.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace BadiMeischter.Pages
{
    public partial class BadiDetailPage : ContentPage
    {
        public BadiDetailPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

		private Badi _item;

		public Badi Item
		{
			get { return _item; }
			set
			{
				if (_item == value)
					return;
				_item = value;
				OnPropertyChanged();
			}
		}

        protected override void OnAppearing()
        {
            var position = new Position(Item.Geometry.Coordinates[1], Item.Geometry.Coordinates[0]);
			BadiMap.MoveToRegion(
					MapSpan.FromCenterAndRadius(
						new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1)));
			var pin = new Pin
			{
				Type = PinType.Place,
				Position = position,
                Label = Item.Properties.Name,
                Address = Item.Properties.Adresse
			};
			BadiMap.Pins.Add(pin);

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += (s, e) =>
			{
                Device.OpenUri(new Uri(Item.Properties.Www));
			};

			var mapStackLayout_tap = new TapGestureRecognizer();
			mapStackLayout_tap.Tapped += (s, e) =>
			{
                var name = Uri.EscapeUriString(Item.Properties.Name);
                var loc = string.Format("{0},{1}", Item.Geometry.Coordinates[1], Item.Geometry.Coordinates[0]);

				Device.OnPlatform(
					// iOS doesn't like %s or spaces in their URLs, so manually replace spaces with +
					iOS: () => Device.OpenUri(new Uri(string.Format("http://maps.apple.com/maps?q={0}Badeanstalt&sll={1}", name.Replace(' ', '+'), loc))),

					// pass the address to Android if we have it
					Android: () => Device.OpenUri(new Uri(string.Format("geo:0,0?q={0}({1})", loc, name)))
				);

			};

		}
    }
}
