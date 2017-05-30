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
    }
}
