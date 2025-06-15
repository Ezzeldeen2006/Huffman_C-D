using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CND
{
    public partial class Buffer : Form
    {
        public Buffer()
        {
            InitializeComponent();
            textBox1.Text = NewBufferSize.ToString();
        }
        public int NewBufferSize { get; private set; } = 1024;


        public bool IsOkClicked { get; private set; } = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int parsedBufferSize))
            {
                if (parsedBufferSize > 0)
                {
                    NewBufferSize = parsedBufferSize;
                    IsOkClicked = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter a positive number for the buffer size.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number for the buffer size.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IsOkClicked = false; 
            this.Close();       
        }
    }
}
