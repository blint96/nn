using nn2.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nn2
{
    class AI
    {
        // trained data array
        public static List<Tuple<int, double[,]>> trainedData = new List<Tuple<int, double[,]>>();

        // all patterns 
        public static List<Tuple<int, List<int[,]>>> patterns = new List<Tuple<int, List<int[,]>>>();

        /**
         * Train script
         * for all numbers
         **/
        public static void trainAll()
        {
            SQLiteConnect db = new SQLiteConnect();
            SQLiteConnection sqlite = db.getConnection();

            for(int i = 0; i <= 9; i++)
            {
                // local table
                int[,] localMatrix = new int[6, 6];
                int max = 0;

                List<int[,]> _patterns = new List<int[,]>();
                List<String> data = db.getDataForMeaning(i);

                if(data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        // lokalna tablica do zachowania wzoru
                        int[,] _tmpPatternsMatrix = new int[6, 6];

                        String[] lines = item.Split(Environment.NewLine.ToCharArray());
                        for (int y = 0; y < 6; y++)
                        {
                            String[] items = lines[y].Split(',');
                            for (int x = 0; x < 6; x++)
                            {
                                localMatrix[x, y] += Int32.Parse(items[x]);
                                _tmpPatternsMatrix[x, y] = Int32.Parse(items[x]);

                                if (max == 0 || max < localMatrix[x, y])
                                    max = localMatrix[x, y];
                            }
                        }

                        _patterns.Add(_tmpPatternsMatrix);
                    }
                }

                // fill patterns
                patterns.Add(new Tuple<int, List<int[,]>>(i, _patterns));

                // max for recalc
                double[,] localMatrixComputed = new double[6, 6];
                for(int n = 0; n < 36; n++)
                {
                    Int32 x = n % 6;
                    Int32 y = n / 6;

                    if (max != 0)
                    {
                        localMatrixComputed[x, y] = Math.Round((Convert.ToDouble(localMatrix[x, y]) / max), 3);
                    }
                }

                // add computed data with value 
                // to AI array
                trainedData.Add(new Tuple<int, double[,]>(i, localMatrixComputed));

                // TODO: test, tylko wyświetla
                // jakie wartości utworzyło
                String resultTable = "";
                for (int n = 0; n < 36; n++)
                {
                    Int32 x = n % 6;
                    Int32 y = n / 6;

                    resultTable += localMatrixComputed[x, y].ToString();

                    if (x == 5) resultTable += "\n";
                    else resultTable += ",";
                }
                MessageBox.Show("Wynik z tabeli dla cyfry " + i + " : \n" + resultTable);
            }
        }

        public static Tuple<int, double, int, double> recognizeForPattern(int[,] input)
        {
            int firstGuess = -1;
            double firstGuessChance = 0.0;

            int secondGuess = -1;
            double secondGuessChance = 0.0;

            // check in computed patterns
            foreach (Tuple<int, double[,]> item in trainedData)
            {
                // Sum all fields for this pattern
                double whole = 0.0;
                double fitValue = 0.0;
                for (int n = 0; n < 36; n++)
                {
                    Int32 x = n % 6;
                    Int32 y = n / 6;

                    whole += item.Item2[x, y];
                    if(input[x, y] > 0)
                    {
                        fitValue += item.Item2[x, y] * input[x, y];
                    }
                }

                double chance = (100 * fitValue) / whole;
                if(chance > firstGuessChance)
                {
                    firstGuess = item.Item1;
                    firstGuessChance = chance;
                }
            }

            // check every single one pattern
            foreach(Tuple<int, List<int[,]>> item in patterns)
            {
                foreach(int[,] n in item.Item2)
                {
                    int overall = 0;
                    int fit = 0;

                    for(int m = 0; m < 36; m++)
                    {
                        Int32 x = m % 6;
                        Int32 y = m / 6;

                        if (n[x, y] > 0)
                        {
                            overall += 1;
                        }

                        if(n[x, y] > 0 && input[x, y] > 0)
                        {
                            fit += 1;
                        }

                        if(input[x, y] > 0 && n[x, y] == 0)
                        {
                            fit -= 1;
                        }
                    }

                    double chance = (100 * fit) / overall;
                    if (chance > secondGuessChance)
                    {
                        secondGuess = item.Item1;
                        secondGuessChance = chance;
                    }
                }
            }

            return new Tuple<int, double, int, double>(firstGuess, firstGuessChance, secondGuess, secondGuessChance);
        }
    }
}
