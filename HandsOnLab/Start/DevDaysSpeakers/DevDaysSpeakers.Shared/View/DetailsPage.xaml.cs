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

            // speak button click handler
            ButtonSpeak.Clicked += ButtonSpeak_Clicked;

            // website button click handler. do anonymous function this time.
            ButtonWebsite.Clicked += (sender, e) =>
            {
                if (speaker.Website.StartsWith("http"))
                Device.OpenUri(new Uri(speaker.Website));
            };

            ButtonSpecial.Clicked += ButtonSpecial_Clicked;
        }

        void ButtonSpeak_Clicked(object sender, EventArgs e)
        {
            CrossTextToSpeech.Current.Speak(this.speaker.Description);
        }





























        private async void ButtonSpecial_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Display happiness level
                var level = await EmotionService.GetAverageHappinessScoreAsync(this.speaker.Avatar);

                var buttonText = "YAY!";
                if (level < Config.HAPPINESS_LEVEL_THRESHOLD)
                    buttonText = "Awww";

                await DisplayAlert("Happiness Level", EmotionService.GetHappinessMessage(level), buttonText);



                 //Display more emotion scores
                //var scores = await EmotionService.GetAverageEmotionScoresAsync(this.speaker.Avatar);

                //var buttonText = "YAY!";
                //if (scores[0] < Config.HAPPINESS_LEVEL_THRESHOLD) // score[0] is happiness
                //    buttonText = "Awww";

                //await DisplayAlert("Emotions Analysis", EmotionService.GetEmotionsMessage(scores), buttonText);
            }
            catch (Exception faceException)
            {
                await DisplayAlert("Oops!", faceException.Message, "Rats");
            }
        }
    }
}
