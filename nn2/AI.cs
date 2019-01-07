using nn2.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

                List<String> data = db.getDataForMeaning(i);
                if(data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        String[] lines = item.Split(Environment.NewLine.ToCharArray());
                        for (int y = 0; y < 6; y++)
                        {
                            String[] items = lines[y].Split(',');
                            for (int x = 0; x < 6; x++)
                            {
                                localMatrix[x, y] += Int32.Parse(items[x]);

                                if (max == 0 || max < localMatrix[x, y])
                                    max = localMatrix[x, y];
                            }
                        }
                    }
                }

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

        public static void trainForNumber(int number)
        {
            
        }

        public static void recognizeForPattern()
        {

        }
    }
}
