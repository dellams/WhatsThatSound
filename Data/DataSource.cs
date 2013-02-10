using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsThatSound.ViewModel;

namespace WhatsThatSound.Data
{
    public static class DataSource
    {
        public static List<HighScore> GetHighScores()
        {
            List<HighScore> scores = new List<HighScore>();

            for (int i = 0; i < 20; i++)
            {
                scores.Add(
                    new HighScore
                 {
                     Date = DateTime.Now.AddHours(i),
                     Guesses = 10 + i,
                     Name = "David Ellams" + i.ToString(),
                     Score = 100 - i,
                     SoundPlayed = 5 + i,
                     Time = 30 + i
                 });

            }

            return scores;
        }

        public static bool SaveHighScore(HighScore highscore)
        {
            return true;
        }
    }
}
