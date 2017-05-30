﻿using System;
using System.Collections.Generic;
using BadiMeischter.Model;
using Xamarin.Forms;

namespace BadiMeischter.Pages
{
    public partial class BadiDetailPage : ContentPage
    {
        public BadiDetailPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

		private BadiInfo _item;

		public BadiInfo Item
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
    }
}
