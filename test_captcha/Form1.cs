using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace test_captcha
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string txt;
        private void button1_Click(object sender, EventArgs e)
        {
            lib_captcha.CaptchaGenerator generator = new lib_captcha.CaptchaGenerator();

            pictureBox1.Image = generator.GenerateCaptchaImage(out txt);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txt == textBox1.Text.ToUpper())
            {
                MessageBox.Show("Captcha đúng");
            }
            else
            {
                MessageBox.Show("Captcha sai");
            }
        }
    }
}
