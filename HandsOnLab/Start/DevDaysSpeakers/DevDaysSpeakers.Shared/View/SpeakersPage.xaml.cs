﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.ViewModel;


namespace DevDaysSpeakers.View
{
    public partial class SpeakersPage : ContentPage
    {
        SpeakersViewModel vm;
        public SpeakersPage()
        {
            InitializeComponent();

            //Create the view model and set as binding context
            vm = new SpeakersViewModel();
            BindingContext = vm;

            // TODO: 23.) wire up ItemSelected event on the ListView
            //ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;
        }




        // TODO: 24.) implementation: navigate to details page
        //var speaker = e.SelectedItem as Speaker;
        //if (speaker == null)
        //    return;

        //await Navigation.PushAsync(new DetailsPage(speaker));

        //ListViewSpeakers.SelectedItem = null;
    }
}
