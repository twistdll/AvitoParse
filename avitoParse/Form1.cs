using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace avitoParse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            siticoneShadowForm1.SetShadowForm(this);
            ChromeController driver = new ChromeController();
            driver.StartParsing();
        }
    }

    struct SearchInfo
    {
        private string _regionName;
        private string _queryText;

        public SearchInfo(string regionName, string queryText)
        {
            _regionName = regionName;
            _queryText = queryText;
        }
    }
}
