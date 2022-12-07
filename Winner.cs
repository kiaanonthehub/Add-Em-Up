﻿namespace Add_Em_Up
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
    public class Winner
    {
        #region Declaration

        // variables
        string inputFilename = "--in abc.txt";
        string outputFilename = "--out xyz.txt";
        // collection
        static List<PlayerHandModel> lstPlayerHand = new List<PlayerHandModel>();

        #endregion
        public Winner()
        {
            ReadFromFile(inputFilename);
            FindWinner();
        }


        #region File Handling

        // method to read data from text file and store in memory
        public void WriteToFile(string data)
        {
            using (StreamWriter writer = File.CreateText(outputFilename))
            { writer.Write(data); }
        }

        // method to write data to text file that is stored in memory 
        public async void ReadFromFile(string fileName)
        {
            // checks if the file exists or not
            if (!File.Exists(fileName))
            {
                await PopulateDataAsync();
            }

            // reads data from file into array
            string[] rawData = File.ReadAllLines(fileName);
            SplitNameAndHand(rawData);
          
        }

        public async Task PopulateDataAsync()
        {
            string[] arrPlayerHands = {
            "Name1:AH,3C,8C,2S,JD",
            "Name2:KD,QH,10C,4C,AC",
            "Name3:6S,8D,3D,JH,2D",
            "Name4:5H,3S,KH,AS,9D",
            "Name5:JS,3H,2H,2C,4D" };

            await File.WriteAllLinesAsync(outputFilename, arrPlayerHands);
        }
            #endregion

            #region Logic & Data Manipulation 

            // splits csv and organises the data into name and cards in hand 
            public void SplitNameAndHand(string[] data)
            {
                foreach (var line in data)
                {
                    // decalre and initialise variables
                    int faceTotalScore = 0, suitTotalScore = 0;

                    // split name and cards
                    string[] splitNameAndCards = line.ToString().Split(':');

                    // split hand of cards into singles
                    string[] splitHandIntoSingles = splitNameAndCards[1].Split(',').ToArray();

                    // iterate through array of singles to calculate summation of each face and suit value
                    foreach (var cardInHand in splitHandIntoSingles)
                    {
                        if (cardInHand.Length >= 2)
                        {
                            // accomodate for 2 digit face value cards e.g 10
                            if (cardInHand.Length == 3)
                            {
                                faceTotalScore += GetValueOfFace(cardInHand.Substring(0, 2));
                            }
                            else
                            {
                                faceTotalScore += GetValueOfFace(cardInHand.First().ToString());
                                suitTotalScore += GetValueOfSuit(cardInHand.Last().ToString());
                            }
                        }
                    }

                    // append to List<T>
                    lstPlayerHand.Add(new PlayerHandModel
                    {
                        Name = splitNameAndCards[0],
                        FaceScore = faceTotalScore,
                        SuitScore = suitTotalScore
                    });
                }
            }

            // return the suit value of a single card
            public int GetValueOfFace(string card)
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

            // return the suit value of a single card
            public int GetValueOfSuit(string card)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    if (card.Equals(suit.ToString()))
                    {
                        return (int)(object)suit;
                    }
                }
                return 0;
            }

            public void FindWinner()
            {
                int highest = 0;

                // sort list in desc to achieve highest scores first
                List<PlayerHandModel> lstSortByFaceValDesc = lstPlayerHand.OrderByDescending(x => x.FaceScore).ToList();

                // iterate through list and find highest score/s
                foreach (var high in lstSortByFaceValDesc)
                {
                    if (high.FaceScore >= highest)
                    {
                        highest = high.FaceScore;
                        high.IsHighest = true;
                    }
                }

                // query the list to find player/s with the highest score
                List<PlayerHandModel> lstWinnersOnly = lstPlayerHand.Where(x => x.IsHighest.Equals(true)).ToList();

                if (lstWinnersOnly.Count == 1)
                {
                    // winner 
                    WriteToFile(lstWinnersOnly[0].Name + ":" + lstWinnersOnly[0].FaceScore.ToString());
                }
                // event of a tie in highest score 
                else if (lstWinnersOnly.Count > 1)
                {
                    highest = 0;

                    // sort the list desc and initialise IsHighest to false
                    lstWinnersOnly.OrderByDescending(x => x.SuitScore).ToList().ForEach(x => x.IsHighest = false);

                    // find highest suit value score
                    foreach (var high in lstWinnersOnly)
                    {
                        // determine the highest suit score
                        if (high.SuitScore >= highest)
                        {
                            highest = high.SuitScore;
                            high.IsHighest = true;
                        }
                    }
                    // winner/s will have IsHighest = true
                    var lstTiedWinners = lstWinnersOnly.Where(x => x.IsHighest == true).ToList();

                    if (lstTiedWinners.Count == 1)
                    {
                        // write winner to output file
                        WriteToFile(lstTiedWinners[0].Name + ":" + lstTiedWinners[0].FaceScore.ToString());
                    }
                    else if (lstTiedWinners.Count > 1)
                    {
                        // two or more names formatted
                        string names = string.Join(",", lstTiedWinners.Select(x => x.Name));

                        // write winner/s to file
                        WriteToFile(names + ":" + lstTiedWinners[0].SuitScore.ToString());
                    }
                }
            }
            #endregion
        }

        #region Model
        public class PlayerHandModel
        {
            public string? Name { get; set; } = string.Empty;
            public int FaceScore { get; set; } = 0;
            public int SuitScore { get; set; } = 0;
            public bool IsHighest { get; set; } = false;
        }
        #endregion
    }
