using DevExpress.UI.Xaml.Layout;
using System;
using System.Threading.Tasks;
using WhatsThatSound.Data;
using WhatsThatSound.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WhatsThatSound.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayScreen : DXPage
    {
        int _noSoundPlayed = 0;
        int _noGuesses = 0;
        int _score = 1000;
        private string _answer;

        public PlayScreen()
        {
            InitializeComponent();
            _answer = "Yay!";
            timer.SecondTicked += timer_SecondTicked;
        }

        void timer_SecondTicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _score -= 1;
            txtScore.Text = _score.ToString();
        }


        private void btnPlaySound_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _noSoundPlayed++;
            _score -= 1;

            txtSoundPlayed.Text = _noSoundPlayed.ToString();
            txtScore.Text = _score.ToString();
        }

        private async Task GuessSound()
        {


            DataSource.SaveHighScore(new HighScore
            {
                Name = txtName.Text,
                Time = timer.Seconds,
                SoundPlayed = _noSoundPlayed,
                Guesses = _noGuesses,
                Date = DateTime.Now,
                Score = _score
            });

            //goto highscores...
            this.Frame.Navigate(typeof(HighScores));

            if (txtGuesses.Text.ToUpper() == _answer.ToUpper())
            {
                txtStatus.Text = "Correct, Well Done!";
                txtStatus.Foreground = new SolidColorBrush(Colors.Green);
                timer.Stop();
            }
            else
            {
                txtStatus.Text = "Wrong, Try Again!";
                txtStatus.Foreground = new SolidColorBrush(Colors.Red);

                _score -= 5;
                _noGuesses++;
                txtGuesses.Text = _noGuesses.ToString();
                txtScore.Text = _score.ToString();
            }

            Storyboard sbShowStatus = txtStatus.FindName("sbShowStatus") as Storyboard;

            if (sbShowStatus != null)
            {
                //await sbShowStatus.BeginAsync();
                sbShowStatus.Completed += sbShowStatus_Completed;
                sbShowStatus.Begin();

                if (txtGuesses.Text.ToUpper() == _answer.ToUpper())
                {
                    Storyboard sbShowNameBox = txtName.FindName("sbShowNameBox") as Storyboard;

                    if (sbShowNameBox != null)
                        await sbShowNameBox.BeginAsync();
                }
            }
        }

        void sbShowStatus_Completed(object sender, object e)
        {
            
        }

        private async void btnGuessSound_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await GuessSound();
        }


        private void btnSubmitHighScore_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DataSource.SaveHighScore(new HighScore
            {
                Name = txtName.Text,
                Time = timer.Seconds,
                SoundPlayed = _noSoundPlayed,
                Guesses = _noGuesses,
                Date = DateTime.Now,
                Score = _score
            });

            //goto highscores...
            this.Frame.Navigate(typeof(GridPage));
            
        }

     
        private void txtGuess_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                GuessSound();
        }
    }
}
