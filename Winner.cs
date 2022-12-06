namespace Add_Em_Up
{
    public class Winner
    {
        #region Enums
        public enum Face
        {
            A = 1, J = 11, Q = 12, K = 13
        }

        public enum Suit
        {
            C = 1, D = 2, H = 3, S = 4
        }
        #endregion

        #region File Handling
        // method to read data from text file and store in memory
        public void WriteToFile(string filename)
        {

            using (StreamWriter writer = File.CreateText(filename))
            {
                // write to file 
            }
        }

        // method to write data to text file that is stored in memory 
        public void ReadFromFile(string fileName)
        {
            string[] rawData = File.ReadAllLines(fileName);

            SplitNameAndHand(rawData);

        }

        // organises the data into name and cards in hand 
        public void SplitNameAndHand(string[] data)
        {
            foreach (var line in data)
            {
                string[] splitNameAndCards = line.ToString().Split(':');
                string[] splitHandIntoSingles = splitNameAndCards[1].Split(',').ToArray();

                int faceTotalScore = 0, suitTotalScore = 0;

                foreach (var cardInHand in splitHandIntoSingles)
                {

                    if (cardInHand.Length == 3)
                    {
                        faceTotalScore += GetValueOfFace(cardInHand.Substring(0, 2));
                        suitTotalScore += GetValueOfSuit(cardInHand.Last().ToString());
                    }
                    else if (cardInHand.Length >= 2)
                    {
                        faceTotalScore += GetValueOfFace(cardInHand.First().ToString());
                        suitTotalScore += GetValueOfSuit(cardInHand.Last().ToString());
                    }
                }

                lstPlayerHand.Add(new PlayerHandModel
                {
                    Name = splitNameAndCards[0],
                    CardsInHand = splitHandIntoSingles,
                    FaceScore = faceTotalScore,
                    SuitScore = suitTotalScore
                });
            }
        }


        //// splits the single card into face and suit
        //public void SplitSinglesIntoFaceSuit(string[] sorted)
        //{
        //    int faceTotalScore, suitTotalScore;

        //    foreach (var cardInHand in sorted)
        //    {
        //        if (cardInHand.Length >= 2)
        //        {
        //            faceTotalScore += GetValueOfFace(cardInHand.First());
        //            suitTotalScore += GetValueOfSuit(cardInHand.Last());
        //        }
        //    }
        //}

        public static int GetValueOfFace(string card)
        {

            foreach (Face face in Enum.GetValues(typeof(Face)))
            {
                if (card.Equals(face.ToString()))
                {
                    return (int)(object)face;
                }
            }
            return Convert.ToInt32(card);
        }

        public static int GetValueOfSuit(string card)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (card.Equals(suit.ToString()))
                {
                    return (int)(object)suit;
                }

                // else error
            }
            return 0;
        }

        #endregion

        // variables
        const string inputFilename = "--in abc.txt";
        const string outputFilename = "--out xyz.txt";

        // collection
        static List<PlayerHandModel> lstPlayerHand = new List<PlayerHandModel>();



    }

    public class PlayerHandModel
    {
        public string? Name { get; set; }
        public string[]? CardsInHand { get; set; }
        public int FaceScore { get; set; }
        public int SuitScore { get; set; }
    }

}
