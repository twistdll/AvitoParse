using System;
using System.Windows.Forms;

namespace avitoParse
{
    public partial class Form1 : Form
    {
        private ChromeController _chromeController;

        public Form1()
        {
            InitializeComponent();
            _chromeController = new ChromeController();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            siticoneShadowForm1.SetShadowForm(this);
            textBox1.Text = InfoSerializer.ReadName();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (siticoneCheckBox1.Checked)
                InfoSerializer.WriteName(textBox1.Text);
            else
                InfoSerializer.DeleteName();
        }

        private void siticoneButton1_Click(object sender, EventArgs e) 
        {
            _chromeController.SetRegionName(textBox1.Text);
            _chromeController.SetQueryText(textBox2.Text);
            ChangeTextBoxesState(false);
            siticoneButton1.Enabled = false;
            siticoneButton1.Text = "Идёт обработка...";
            _chromeController.StartParsing();
            ChangeTextBoxesState(true);
            siticoneButton1.Text = "Начать обработку";
            siticoneButton1.Enabled = true;
        }

        private void ChangeTextBoxesState(bool state)
        {
            TextBox[] textBoxes = new TextBox[] { textBox1, textBox2 };

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Enabled = state;
            }
        }
    }
}
