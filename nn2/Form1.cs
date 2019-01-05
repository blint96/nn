using nn2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nn2
{
    public partial class Form1 : Form
    {
        // privates
        private List<PictureBox> inputs = new List<PictureBox>();
        private SQLiteConnect sql = new SQLiteConnect();

        // table only for displaying current matrix
        private int[,] input = new int[6, 6];

        // table for trained data
        private int[,] trainData = new int[10];

        public Form1()
        {
            InitializeComponent();
        }

        private void startMatrix()
        {
            if(inputs.Count > 0)
            {
                foreach(PictureBox item in inputs) {
                    this.Controls.Remove(item);
                }
            }

            Array.Clear(input, 0, input.Length);
            inputs.Clear();

            int top = 42;
            int left = 10;
            for (int i = 0; i < 36; i++)
            {
                PictureBox tmp = new PictureBox();
                tmp.Height = 32;
                tmp.Width = 32;
                tmp.Left = left;
                tmp.Top = top;
                tmp.BackColor = Color.White;
                tmp.Name = "ipt_" + i;
                this.Controls.Add(tmp);
                inputs.Add(tmp);

                // "click" element
                tmp.Click += (s, err) => {
                    Int32 fieldId = Int32.Parse(tmp.Name.Split('_')[1]);
                    Boolean clicked = tmp.BackColor == Color.White ? true : false;
                    tmp.BackColor = clicked ? Color.Black : Color.White;

                    // operations
                    
                    Int32 x = fieldId % 6;
                    Int32 y = fieldId / 6;
                    //MessageBox.Show("x: " + x + ", y:" + y);
                    input[x, y] = 1;

                };

                left += 38;
                if (((i + 1) % 6) == 0)
                {
                    top = top + 38;
                    left = 10;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            startMatrix();

            // get the training data
            // sql.selectQuery..
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            startMatrix();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String patternName = txtName.Text;
            if (patternName.Length == 0)
            {
                MessageBox.Show("Musisz wprowadzić symbol jakiemu odpowiada wzór, który narysowałeś/aś.");
                return;
            }

            String serialize = "";
            for (int i = 0; i < 36; i++)
            {
                Int32 x = i % 6;
                Int32 y = i / 6;

                serialize += input[x, y].ToString();

                if (x == 5) serialize += "\n";
                else serialize += ",";
            }

            MessageBox.Show(serialize);
            sql.saveItem(patternName, serialize);
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            sql.getItemTest();
        }
    }
}
