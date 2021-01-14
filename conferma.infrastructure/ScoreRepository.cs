using conferma.core.Domain;
using conferma.core.Interfaces;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace conferma.infrastructure
{
    public class ScoreRepository : IScoreRepository
    {
        private string pathToFile;

        public ScoreRepository(IConfiguration config)
        {
            pathToFile = config.GetValue<string>("ScoreFilePath");
        }

        public List<Score> GetHighScores()
        {
            if (!File.Exists(Path.Combine(pathToFile, "high-scores.csv")))
            {
                File.CreateText(Path.Combine(pathToFile, "high-scores.csv")).Close();
            }

            List<Score> scores;
            using (var reader = new StreamReader(Path.Combine(pathToFile, "high-scores.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                scores = csv.GetRecords<Score>().ToList();
            }

            return scores.OrderByDescending(x => x.Points).ThenBy(y => y.TimeTaken).ToList();
        }

        public void SaveScore(Score score)
        {
            var currentHighScores = GetHighScores();

            currentHighScores.Add(score);
            var newHighScores = currentHighScores.OrderByDescending(x => x.Points).ThenBy(y => y.TimeTaken).Take(3).ToList();

            using (var writer = new StreamWriter(Path.Combine(pathToFile, "high-scores.csv")))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(newHighScores);
            }
        }
    }
}
