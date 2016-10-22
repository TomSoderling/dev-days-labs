using System;
using System.Collections.ObjectModel;
using DevDaysSpeakers.Model;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace DevDaysSpeakers
{
    public class SpeakerCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification = false;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        // This will only fire the CollectionChanged event after all the items have been added.
        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            _suppressNotification = true; // turn notifications off 

            foreach (T item in list)
                Add(item);

            _suppressNotification = false; // turn back on

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
