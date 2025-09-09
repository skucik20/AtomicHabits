using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfApp.Wpf.Helpers.Converters
{
    /// <summary>
    /// Konwerter bool -> Visibility z opcjami:
    /// - Invert: odwraca wartość (true => Hidden/Collapsed jeśli Invert=true)
    /// - CollapseWhenFalse: jeśli true używa Collapsed zamiast Hidden dla wartości false
    /// Obsługuje nullable bool (null traktowane jako false).
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If true, will invert the boolean before converting.
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// If true, maps false -> Collapsed. Otherwise maps false -> Hidden.
        /// </summary>
        public bool CollapseWhenFalse { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = false;

            if (value is bool vb)
                b = vb;
            else if (value is bool?)
            {
                var nb = (bool?)value;
                b = nb.GetValueOrDefault(false);
            }
            else if (value == null)
                b = false;
            else
            {
                // próbujemy próbować parsować string/int itp. (opcjonalne)
                try
                {
                    b = System.Convert.ToBoolean(value, culture);
                }
                catch
                {
                    b = false;
                }
            }

            if (Invert)
                b = !b;

            if (b)
                return Visibility.Visible;

            return CollapseWhenFalse ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (value is Visibility vis)
            {
                result = (vis == Visibility.Visible);
            }
            else
            {
                // nieznany typ — próbujemy konwertować bezpiecznie
                try
                {
                    result = System.Convert.ToBoolean(value, culture);
                }
                catch
                {
                    result = false;
                }
            }

            if (Invert)
                result = !result;

            // jeśli targetType to bool? zwracamy bool lub (bool?)null? 
            // Tutaj trzymamy prostą, deterministyczną politykę: nie zwracamy null.
            if (targetType == typeof(bool) || targetType == typeof(bool?))
                return result;

            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Pozwala używać konwertera bez deklarowania zasobu.
    /// Umożliwia konfigurację przez właściwości: Invert i CollapseWhenFalse.
    /// </summary>
    public class BoolToVisibilityConverterExtension : MarkupExtension
    {
        public bool Invert { get; set; }
        public bool CollapseWhenFalse { get; set; } = true;

        private static BoolToVisibilityConverter _shared = new BoolToVisibilityConverter();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // Tworzymy instancję z ustawieniami. Jeśli nie ustawiono niczego — użyjemy singletona.
            if (!Invert && CollapseWhenFalse)
            {
                // domyślna konfiguracja — można zwrócić singleton
                return _shared;
            }

            return new BoolToVisibilityConverter
            {
                Invert = this.Invert,
                CollapseWhenFalse = this.CollapseWhenFalse
            };
        }
    }
}
