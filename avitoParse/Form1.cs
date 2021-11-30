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

        private void siticoneButton1_Click(object sender, EventArgs e)
        {
            _chromeController.SetRegionName(textBox1.Text);
            _chromeController.SetQueryText(textBox2.Text);
            StartParsing();
        }

        private async void StartParsing()
        {
            DisableUI();
            await Task.Run(() => _chromeController.Parse());
            EnableUI();
        }

        private void refreshButton_Click_1(object sender, EventArgs e)
        {
            totalPagesCount.Text = _chromeController.PagesCount.ToString();
            currentPageCount.Text = _chromeController.CurrentPageNumber.ToString();
        }

        private void DisableUI()
        {
            ChangeTextBoxesState(false);
            ChangePageTrackersState(true);
            siticoneButton1.Enabled = false;
            siticoneButton1.Text = "Идёт обработка...";
        }
        private void EnableUI()
        {
            ChangeTextBoxesState(true);
            ChangePageTrackersState(false);
            siticoneButton1.Text = "Начать обработку";
            siticoneButton1.Enabled = true;
        }

        private void ChangePageTrackersState(bool state)
        {
            totalPagesCount.Visible = state;
            currentPageCount.Visible = state;
            slashPageCount.Visible = state;
            refreshButton.Visible = state;
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
