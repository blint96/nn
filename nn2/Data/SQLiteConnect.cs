using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nn2.Data
{
    class SQLiteConnect
    {
        private SQLiteConnection sqlite;

        public SQLiteConnect()
        {
            if (File.Exists("nn.db"))
            {
                sqlite = new SQLiteConnection("Data Source=nn.db");
            } else
            {
                SQLiteConnection.CreateFile("nn.db");
                sqlite = new SQLiteConnection("Data Source=nn.db");
            }
        }

        public void getItemTest()
        {
            String query = "SELECT * FROM data WHERE id = 5";
            try
            {
                SQLiteCommand cmd;
                sqlite.Open();

                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;

                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();

                int contentIndex = reader.GetOrdinal("content");
                String result = reader.GetString(contentIndex);
                MessageBox.Show("Wynik z bazy: \n" + result);

                // rewrite from db to table
                int[,] tmp = new int[6, 6];
                String[] lines = result.Split(Environment.NewLine.ToCharArray());
                for(int y = 0; y < 6; y++)
                {
                    String[] items = lines[y].Split(',');
                    for(int x = 0; x < 6; x++)
                    {
                        tmp[x, y] = Int32.Parse(items[x]);
                    }
                }

                // display from table
                String resultTable = "";
                for (int i = 0; i < 36; i++)
                {
                    Int32 x = i % 6;
                    Int32 y = i / 6;

                    resultTable += tmp[x, y].ToString();

                    if (x == 5) resultTable += "\n";
                    else resultTable += ",";
                }
                MessageBox.Show("Wynik z tabeli: \n" + resultTable);
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }

            sqlite.Close();
        }

        public void saveItem(String meaning, String content)
        {
            String query = "INSERT INTO data VALUES(NULL, " + Int32.Parse(meaning) + ", '" + content + "')";
            try
            {
                SQLiteCommand cmd;
                sqlite.Open();
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }

            sqlite.Close();
        }

        public DataTable selectQuery(string query)
        {
            SQLiteDataAdapter ad;
            DataTable dt = new DataTable();

            try
            {
                SQLiteCommand cmd;
                sqlite.Open();  //Initiate connection to the db
                cmd = sqlite.CreateCommand();
                cmd.CommandText = query;  //set the passed query
                ad = new SQLiteDataAdapter(cmd);
                ad.Fill(dt); //fill the datasource
            }
            catch (SQLiteException ex)
            {
                //Add your exception code here.
            }

            sqlite.Close();
            return dt;
        }
    }
}
