using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WhatsThatSound.Controls
{
    /// <summary>
    /// A control that displays a countdown visualization
    /// and raises a CountdownComplete event when countdown is complete.
    /// Countdown duration is specified in Seconds.
    /// </summary>
    public sealed partial class CountupControl
    {
        private bool _stop;
        private bool _countingDown;
        /// <summary>
        /// Occurs when the countdown is complete.
        /// </summary>
        public event RoutedEventHandler CountdownComplete;
        public event RoutedEventHandler SecondTicked;

        #region Seconds
        /// <summary>
        /// The seconds property.
        /// </summary>
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register(
                "Seconds",
                typeof(int),
                typeof(CountupControl),
                new PropertyMetadata(
                    0,
                    OnSecondsChanged));

        /// <summary>
        /// Gets or sets the seconds to countdown.
        /// </summary>
        /// <value>
        /// The seconds.
        /// </value>
        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public void Stop()
        {
            _stop = true;
        }

        private static void OnSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CountupControl)sender;
            var oldSeconds = (int)e.OldValue;
            var newSeconds = (int)e.NewValue;
            target.OnSecondsChanged(oldSeconds, newSeconds);
        }

        private void OnSecondsChanged(int oldSeconds, int newSeconds)
        {
            if (!_countingDown && newSeconds >= 0)
            {
#pragma warning disable 4014
                StartCountdownAsync(newSeconds);
#pragma warning restore 4014
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CountdownControl" /> class.
        /// </summary>
        public CountupControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts the countdown and completes when the countdown completes.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public async Task StartCountdownAsync(int seconds)
        {
            _countingDown = true;
            this.Seconds = seconds;
            TimeSpan tsTimeTaken = TimeSpan.FromSeconds(this.Seconds);
            bool grow = true;

            while (this.Seconds >= 0 && !_stop)
            //do while (true)
            {
                var sb = new Storyboard();

                if (grow)
                {
                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);

                    // Workaround for a problem animating custom dependency properties
                    //PART_RingSlice.SetBinding(
                    //   RingSlice.EndAngleProperty,
                    //   new Binding { Source = r1, Path = new PropertyPath("Opacity"), Mode = BindingMode.OneWay });
                    //Storyboard.SetTargetProperty(da, "Opacity");
                    //Storyboard.SetTarget(sb, r1);
                    Storyboard.SetTargetProperty(da, "EndAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }
                else
                {
                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);

                    //PART_RingSlice.SetBinding(
                    //   RingSlice.StartAngleProperty,
                    //   new Binding { Source = r2, Path = new PropertyPath("Opacity"), Mode = BindingMode.OneWay });
                    //Storyboard.SetTargetProperty(da, "Opacity");
                    //Storyboard.SetTarget(sb, r2);
                    Storyboard.SetTargetProperty(da, "StartAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }


                //PART_Label.Text = this.Seconds.ToString();
                tsTimeTaken = TimeSpan.FromSeconds(this.Seconds);
                PART_Label.Text = string.Concat(tsTimeTaken.Minutes.ToString().PadLeft(2, '0'), ":", tsTimeTaken.Seconds.ToString().PadLeft(2, '0'));
                //PART_Label.Text = tsTimeTaken.ToString();

                await sb.BeginAsync();

                if (grow)
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 359.999d;
                }
                else
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 0d;
                }

                grow = !grow;
                //this.Seconds--;
                this.Seconds++;

                if (SecondTicked != null)
                    SecondTicked(this, new RoutedEventArgs());
            }

            
            PART_Label.Text = tsTimeTaken.ToString();

            if (this.CountdownComplete != null)
            {
                CountdownComplete(this, new RoutedEventArgs());
            }

            _countingDown = false;
        }
    }
}
