using conferma.core.Domain;
using System.Collections.Generic;

namespace conferma.core.Interfaces
{
    public interface IScoreRepository
    {
        public List<Score> GetHighScores();
        public void SaveScore(Score score);
    }
}
