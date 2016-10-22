using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DevDaysSpeakers.Model;
using DevDaysSpeakers.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace DevDaysSpeakers.ViewModel
{
    // TODO: 02.) Implement INotifyPropertyChanged
    public class SpeakersViewModel : INotifyPropertyChanged
    {
        // TODO: 05.) Speakers property
        public ObservableCollection<Speaker> Speakers { get; set; }

        // TODO: 15.) command to get speakers
        public Command GetSpeakersCommand { get; set; }
        
        // TODO: 06.) default ctor
        public SpeakersViewModel()
        {
            // TODO: 07.) create new instance of ObservableCollection
            Speakers = new ObservableCollection<Speaker>();

            // TODO: 16.) instantiate command
            GetSpeakersCommand = new Command(async () => await GetSpeakers(), () => !IsBusy);
        }


        // DELETE THIS LATER
        public event PropertyChangedEventHandler PropertyChanged;
        // DELETE THIS LATER


        // TODO: 03.) Helper method to raise PropertyChanged event
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // TODO: 04.) Create IsBusy property w/ backing field
        // DELETE THIS LATER
        bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }

            set
            {
                isBusy = value;
                OnPropertyChanged();
                GetSpeakersCommand.ChangeCanExecute();
            }
        }

        // TODO: 17.) update the can execute - move up to setter
        //GetSpeakersCommand.ChangeCanExecute();

        // TODO: 08.) GetSpeakers method
        private async Task GetSpeakers()
        {
            // TODO: 09.) don't grab data if we're already grabbing it
            if (IsBusy)
                return;

            // TODO: 10.) scaffolding for try/catch/finally block
            Exception error = null;
            //try
            //{
            //    // call to server
            //    IsBusy = true;

            //    // TODO: 11.) grab JSON from server
            //    using (var client = new HttpClient())
            //    {
            //        var json = await client.GetStringAsync("http://demo4404797.mockable.io/speakers");

            //        // TODO: 12.) deserialize the JSON to list of speakers
            //        var speakers = JsonConvert.DeserializeObject<List<Speaker>>(json);

            //        // TODO: 13.) load speakers into ObservableCollection
            //        Speakers.Clear();
            //        foreach (var speaker in speakers)
            //            Speakers.Add(speaker);
            //    }
            //}
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                var items = await service.GetSpeakers();

                Speakers.Clear();
                foreach (var item in items)
                    Speakers.Add(item);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            // TODO: 14.) show exception message if something goes wrong
            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");

            // TODO: 35.) "try" getting speakers from Azure instead
            //try
            //{
            //    IsBusy = true;

            //    var service = DependencyService.Get<AzureService>();
            //    var items = await service.GetSpeakers();

            //    Speakers.Clear();
            //    foreach (var item in items)
            //        Speakers.Add(item);
            //}
        }
    }
}
