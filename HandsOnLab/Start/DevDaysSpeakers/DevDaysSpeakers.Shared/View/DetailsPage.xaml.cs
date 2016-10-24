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
            }
            catch (Exception faceException)
            {
                await DisplayAlert("Oops!", faceException.Message, "Rats");


                // 10/24/2016 - getting this exception on UWP each time:
                //     Message: Exception of type 'Microsoft.ProjectOxford.Common.ClientException' was thrown.
                // Stack Trace:
                /*
               at Microsoft.ProjectOxford.Common.ServiceClient.<SendAsync>d__22`2.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at Microsoft.ProjectOxford.Common.ServiceClient.<PostAsync>d__20`2.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at Microsoft.ProjectOxford.Emotion.EmotionServiceClient.<RecognizeAsync>d__6.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at Microsoft.ProjectOxford.Emotion.EmotionServiceClient.<RecognizeAsync>d__5.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
               at DevDaysSpeakers.EmotionService.<GetEmotionsAsync>d__0.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
               at DevDaysSpeakers.EmotionService.<GetAverageHappinessScoreAsync>d__1.MoveNext()
            --- End of stack trace from previous location where exception was thrown ---
               at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
               at System.Runtime.CompilerServices.TaskAwaiter`1.GetResult()
               at DevDaysSpeakers.View.DetailsPage.<ButtonSpecial_Clicked>d__3.MoveNext()
               */
            }
        }
    }
}
