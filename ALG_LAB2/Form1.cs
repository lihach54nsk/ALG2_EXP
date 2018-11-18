using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ALG_LAB2
{
    public partial class Form1 : Form
    {
        WorkFiles Work;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // richTextBox100.Text = textBox1.Text;
           Work = new WorkFiles(textBox1.Text);
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           richTextBox100.Text+= Work.AddCity(textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox100.Text += Work.DeleteCity(Convert.ToInt32(textBox3.Text));
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //richTextBox100.Text += Work.DeleteCity(textBox3.Text);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox100.Text += Work.AddRoad(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox6.Text));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox100.Text += Work.DeleteRoad(Convert.ToInt32(textBox8.Text), Convert.ToInt32(textBox9.Text));
        }

       
    }
}
