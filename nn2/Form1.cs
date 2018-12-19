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
        private List<PictureBox> inputs = new List<PictureBox>();

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
                    if (tmp.BackColor == Color.White)
                        tmp.BackColor = Color.Black;
                    else
                        tmp.BackColor = Color.White;
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
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            startMatrix();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String patternName = txtName.Text;
            if(patternName.Length == 0)
            {
                MessageBox.Show("Musisz wprowadzić symbol jakiemu odpowiada wzór, który narysowałeś/aś.");
                return;
            }
        }
    }
}
