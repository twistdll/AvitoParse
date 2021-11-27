using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace avitoParse
{
    class ChromeController
    {
        private ChromeDriver _chromeDriver;
        private ChromeOptions _chromeOptions;

        public ChromeController()
        { 
            _chromeOptions = new ChromeOptions();
            _chromeOptions.AddArgument("headless");
            _chromeOptions.AddArgument("start-maximized");
        }

        public void StartParsing()
        {
            _chromeDriver = new ChromeDriver(_chromeOptions);
            _chromeDriver.Navigate().GoToUrl("https://www.avito.ru/" + "smolensk");
            EnterSearchQuery("мопс");
            Thread.Sleep(2000);
            GetAdsList();
        }

        private void EnterSearchQuery(string queryText)
        {
            IWebElement searchBox = _chromeDriver.FindElement(By.CssSelector(".input-layout-input-layout-_HVr_"));
            searchBox.Click();
            searchBox.SendKeys(queryText + Keys.Enter);        
        }

        private void GetAdsList()
        {
            var ads = _chromeDriver.FindElements(By.CssSelector(".iva-item-root-Nj_hb"));
        }
    }
}
