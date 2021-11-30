using System;
using System.Threading.Tasks;
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

            _chromeController.CloseDriver();
        }

        private void siticoneCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            ParsingMode.OnlyOnePage = siticoneCheckBox2.Checked;
        }

        private void siticoneButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                siticoneButton1.Text = "Введите данные";
            }
            else
            {
                _chromeController.SetRegionName(textBox1.Text);
                _chromeController.SetQueryText(textBox2.Text);
                StartParsing();
            }
        }

        private async void StartParsing()
        {
            DisableUI();
            await Task.Run(() => _chromeController.Parse());
            EnableUI();
        }

        private void DisableUI()
        {
            ChangeTextBoxesState(false);
            siticoneButton1.Enabled = false;
            siticoneButton1.Text = "Идёт обработка...";
            siticoneCheckBox2.Enabled = false;

        }
        private void EnableUI()
        {
            ChangeTextBoxesState(true);
            siticoneButton1.Text = "Начать обработку";
            siticoneButton1.Enabled = true;
            siticoneCheckBox2.Enabled = false;
        }

        private void ChangeTextBoxesState(bool state)
        {
            TextBox[] textBoxes = new TextBox[] { textBox1, textBox2 };

            foreach (TextBox textBox in textBoxes)
            {
                textBox.Enabled = state;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            siticoneButton1.Text = "Начать обработку";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            siticoneButton1.Text = "Начать обработку";
        }
    }
}
