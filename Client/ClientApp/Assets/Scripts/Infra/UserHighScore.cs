namespace Infra
{
    public class UserHighScore
    {
        public class UserScore
        {
            public int rank;
            public string id;
            public int score;
        }

        public UserScore[] highRanks;
        public UserScore myRank;
    }
}