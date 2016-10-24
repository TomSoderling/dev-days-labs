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
    // Implement INotifyPropertyChanged
    public class SpeakersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Speakers property
        public ObservableCollection<Speaker> Speakers { get; set; }

        // command to get speakers
        public Command GetSpeakersCommand { get; set; }

        // default ctor
        public SpeakersViewModel()
        {
            // create new instance of ObservableCollection
            Speakers = new ObservableCollection<Speaker>();

            // instantiate command
            GetSpeakersCommand = new Command(async 
                                             () => await GetSpeakers(), 
                                             () => !IsBusy);
        }




        // Helper method to raise PropertyChanged event
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Create IsBusy property w/ backing field, + OnPropertyChanged();
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


        // GetSpeakers method
        private async Task GetSpeakers()
        {
            // don't grab data if we're already grabbing it
            if (IsBusy)
                return;

            // scaffolding for try/catch/finally block
            Exception error = null;
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

            // show exception message if something goes wrong
            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");

        }

    }
}
