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
        private ChromeDriverService _chromeService;
        private string _regionName;
        private string _queryText;

        public ChromeController()
        {
            _chromeOptions = new ChromeOptions();
            _chromeOptions.AddArgument("headless");
            _chromeOptions.AddArgument("start-maximized");

            _chromeService = ChromeDriverService.CreateDefaultService();
            _chromeService.HideCommandPromptWindow = true;
        }

        public void SetRegionName(string name) //incorrect but dont know how to realise this properly
        {
            _regionName = name;
        }
        public void SetQueryText(string text)
        {
            _queryText = text;
        }

        public void StartParsing()
        {
            _chromeDriver = new ChromeDriver(_chromeService, _chromeOptions);
            _chromeDriver.Navigate().GoToUrl("https://www.avito.ru/" + _regionName);
            EnterSearchQuery();
            Thread.Sleep(2000);
            GetLinksList();
        }

        private void EnterSearchQuery()
        {
            try
            {
                IWebElement searchBox = _chromeDriver.FindElement(By.CssSelector(".input-layout-input-layout-_HVr_"));
                searchBox.Click();
                searchBox.SendKeys(_queryText + Keys.Enter);
            }
            catch (NoSuchElementException)
            {
                return;
            }
        }

        private void GetLinksList()
        {
            var ads = _chromeDriver.FindElements(By.CssSelector(".iva-item-titleStep-_CxvN a"));
            List<string> links = new List<string>();

            foreach (var element in ads)
            {
                links.Add(element.GetAttribute("href"));
            }

            InfoSerializer.WriteList(links);
        }
    }
}
