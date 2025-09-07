using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp.Wpf.Helpers.Extensions;


public static class CalendarHelper
{
    // Stan przypisany do konkretnego Calendara
    private class CalendarBindingInfo
    {
        public Calendar Calendar { get; }
        public ObservableCollection<DateTime> Collection { get; private set; }
        public bool IsSyncing { get; private set; }

        public NotifyCollectionChangedEventHandler CollectionChangedHandler { get; }
        public EventHandler<SelectionChangedEventArgs> CalendarSelectionChangedHandler { get; }

        public CalendarBindingInfo(Calendar calendar, ObservableCollection<DateTime> collection)
        {
            Calendar = calendar;
            Collection = collection;
            CollectionChangedHandler = OnCollectionChanged;
            CalendarSelectionChangedHandler = OnCalendarSelectedDatesChanged;
        }

        public void Subscribe()
        {
            if (Collection != null)
                Collection.CollectionChanged += CollectionChangedHandler;

            Calendar.SelectedDatesChanged += CalendarSelectionChangedHandler;
            SyncFromVm(); // wstępna synchronizacja
        }

        public void Unsubscribe()
        {
            if (Collection != null)
                Collection.CollectionChanged -= CollectionChangedHandler;

            Calendar.SelectedDatesChanged -= CalendarSelectionChangedHandler;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SyncFromVm();
        }

        private void OnCalendarSelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsSyncing) return;
            if (Collection == null) return;

            // Aktualizujemy kolekcję VM na wątku UI
            Calendar.Dispatcher.Invoke(() =>
            {
                try
                {
                    IsSyncing = true;
                    Collection.CollectionChanged -= CollectionChangedHandler;

                    Collection.Clear();
                    foreach (DateTime d in Calendar.SelectedDates)
                        Collection.Add(d);
                }
                finally
                {
                    Collection.CollectionChanged += CollectionChangedHandler;
                    IsSyncing = false;
                }
            }, DispatcherPriority.Background);
        }

        public void SyncFromVm()
        {
            // Bezpiecznie na wątku UI: ustawiamy SelectedDates zgodnie z kolekcją VM
            Calendar.Dispatcher.Invoke(() =>
            {
                if (IsSyncing) return;
                try
                {
                    IsSyncing = true;
                    Calendar.SelectedDates.Clear();
                    if (Collection != null)
                    {
                        foreach (var d in Collection)
                            Calendar.SelectedDates.Add(d);

                        // opcjonalnie ustawiamy DisplayDate, żeby widoczny miesiąc zawierał jakieś zaznaczone dni
                        if (Collection.Count > 0)
                            Calendar.DisplayDate = Collection[0];
                    }
                }
                finally
                {
                    IsSyncing = false;
                }
            }, DispatcherPriority.Render);
        }

        public void SetCollection(ObservableCollection<DateTime> newCollection)
        {
            if (Collection != null)
                Collection.CollectionChanged -= CollectionChangedHandler;

            Collection = newCollection;

            if (Collection != null)
                Collection.CollectionChanged += CollectionChangedHandler;

            SyncFromVm();
        }
    }

    // Prywatne DP trzymające stan per-Calendar
    private static readonly DependencyProperty CalendarStateProperty =
        DependencyProperty.RegisterAttached("CalendarState", typeof(CalendarBindingInfo), typeof(CalendarHelper), new PropertyMetadata(null));

    private static void SetCalendarState(DependencyObject element, CalendarBindingInfo value) => element.SetValue(CalendarStateProperty, value);
    private static CalendarBindingInfo GetCalendarState(DependencyObject element) => (CalendarBindingInfo)element.GetValue(CalendarStateProperty);

    // Publiczne DP do bindowania kolekcji
    public static readonly DependencyProperty BindableSelectedDatesProperty =
        DependencyProperty.RegisterAttached(
            "BindableSelectedDates",
            typeof(ObservableCollection<DateTime>),
            typeof(CalendarHelper),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableSelectedDatesChanged));

    public static void SetBindableSelectedDates(DependencyObject element, ObservableCollection<DateTime> value)
        => element.SetValue(BindableSelectedDatesProperty, value);

    public static ObservableCollection<DateTime> GetBindableSelectedDates(DependencyObject element)
        => (ObservableCollection<DateTime>)element.GetValue(BindableSelectedDatesProperty);

    private static void OnBindableSelectedDatesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Calendar calendar) return;

        var oldState = GetCalendarState(calendar);
        var newCollection = e.NewValue as ObservableCollection<DateTime>;

        if (oldState == null && newCollection == null)
        {
            // nic do zrobienia
            return;
        }

        if (oldState == null && newCollection != null)
        {
            var state = new CalendarBindingInfo(calendar, newCollection);
            SetCalendarState(calendar, state);
            state.Subscribe();
            return;
        }

        if (oldState != null && newCollection == null)
        {
            // odpinamy i czyścimy
            oldState.Unsubscribe();
            SetCalendarState(calendar, null);
            calendar.SelectedDates.Clear();
            return;
        }

        if (oldState != null && newCollection != null)
        {
            // podmiana kolekcji: odpinamy stare -> podpinamy nowe
            oldState.Unsubscribe();
            var state = new CalendarBindingInfo(calendar, newCollection);
            SetCalendarState(calendar, state);
            state.Subscribe();
            return;
        }
    }
}