using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using ModernWpf.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace WpfApp.Wpf.ViewModels.Shared
{
    public class OnFocusTextSelectedTextBox : TextBox
    {

        static OnFocusTextSelectedTextBox()
        {
            // Dzięki temu kontrolka będzie brała style z TextBox
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(OnFocusTextSelectedTextBox),
                new FrameworkPropertyMetadata(typeof(TextBox)));
        }

        public OnFocusTextSelectedTextBox()
        {
            AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            AddHandler(GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText), true);
            AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox)) {
                parent = VisualTreeHelper.GetParent(parent);    
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if(!textBox.IsKeyboardFocused)
                {
                    // if the text box is not yet focusser, give it the focus and 
                    // stop further processing of this click event
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null) { 
                textBox.SelectAll();
            }
        }
    }
}
