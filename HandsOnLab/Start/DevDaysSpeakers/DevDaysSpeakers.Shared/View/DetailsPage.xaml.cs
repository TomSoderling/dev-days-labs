using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using DevDaysSpeakers.Model;
using Plugin.TextToSpeech;
using DevDaysSpeakers.ViewModel;

namespace DevDaysSpeakers.View
{
    public partial class DetailsPage : ContentPage
    {
        Speaker speaker;
        public DetailsPage(Speaker speaker)
        {
            InitializeComponent();
            
            //Set local instance of speaker and set BindingContext
            this.speaker = speaker;
            BindingContext = this.speaker;

            // TODO: 28.) speak button click handler
            ButtonSpeak.Clicked += ButtonSpeak_Clicked;

            // TODO: 30.) website button click handler. do anonymous function.
            ButtonWebsite.Clicked += (sender, e) =>
            {
                // DELETE ALL THIS UNCOMMENTED STUFF
                if (speaker.Website.StartsWith("http"))
                    Device.OpenUri(new Uri(speaker.Website));
            };

            //if (speaker.Website.StartsWith("http"))
            //    Device.OpenUri(new Uri(speaker.Website));
        }

        // DELETE THIS METHOD
        void ButtonSpeak_Clicked(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak(this.speaker.Description);
        }


        // TODO: 29.) speak!
        //CrossTextToSpeech.Current.Speak(this.speaker.Description);
    }
}
