using System.Windows;
using System.Windows.Controls;

namespace M3U8Downloader.MainModule.Controls
{
    /// <summary>
    /// Interaction logic for StartOrStopButton.xaml
    /// </summary>
    public partial class StartOrStopButton : Button
    {
        public StartOrStopButton()
        {
            InitializeComponent();
        }

        public ButtonState State
        {
            get
            {
                return (ButtonState)GetValue(StateProperty);
            }
            set
            {
                SetValue(StateProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(ButtonState), typeof(StartOrStopButton), new PropertyMetadata(ButtonState.Stopped, StateChanged));

        private static void StateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var self = (StartOrStopButton)obj;
            var newValue = (ButtonState)e.NewValue;
            switch (newValue)
            {
                case ButtonState.Started:
                    var stopButtonIcon = self.Resources["StopButtonIcon"];
                    self.Content = stopButtonIcon;
                    break;
                case ButtonState.Stopped:
                    var startButtonIcon = self.Resources["StartButtonIcon"];
                    self.Content = startButtonIcon;
                    break;
            }
            var oldValue = (ButtonState)e.OldValue;
            var args = new RoutedPropertyChangedEventArgs<ButtonState>(oldValue, newValue)
            {
                RoutedEvent = ButtonStateChangedEvent
            };
            self.RaiseEvent(args);
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            State = State.Switch();
            e.Handled = true;
        }


        public static readonly RoutedEvent ButtonStateChangedEvent = EventManager.RegisterRoutedEvent("ButtonStateChangedEvent", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<ButtonState>), typeof(StartOrStopButton));

        public event RoutedPropertyChangedEventHandler<ButtonState> ButtonStateChanged
        {
            add => AddHandler(ButtonStateChangedEvent, value);
            remove => RemoveHandler(ButtonStateChangedEvent, value);

        }
    }

    public enum ButtonState
    {
        Started, Stopped
    }

    public static class ButtonStateHelper
    {
        public static ButtonState Switch(this ButtonState state)
        {
            return state == ButtonState.Started ? ButtonState.Stopped : ButtonState.Started;
        }
    }
}
