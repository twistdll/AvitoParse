using System.Collections.Generic;
using System.Threading;
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

        public void Parse()
        {
            _chromeDriver = new ChromeDriver(_chromeService, _chromeOptions);
            _chromeDriver.Navigate().GoToUrl("https://www.avito.ru/");
            ChooseRegion();
            EnterSearchQuery();
            Thread.Sleep(2000);
            InfoSerializer.WriteList(GetAdsList());
            _chromeDriver.Close();
        }

        private void ChooseRegion()
        {
            try
            {
                IWebElement regionElement = _chromeDriver.FindElement(By.CssSelector(".main-locationWrapper-R8itV"));
                regionElement.Click();
                IWebElement regionInput = _chromeDriver.FindElement(By.CssSelector(".suggest-input-rORJM"));
                regionInput.Click();
                regionInput.SendKeys(_regionName);
                Thread.Sleep(1000);
                regionInput.SendKeys(Keys.Enter);
                regionInput = _chromeDriver.FindElement(By.CssSelector(".button-button-CmK9a.button-size-m-LzYrF.button-primary-x_x8w"));
                regionInput.Click();
                Thread.Sleep(1000);
            }
            catch (NoSuchElementException)
            {
                return;
            }
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

        private List<string> GetAdsList()
        {
            var links = _chromeDriver.FindElements(By.CssSelector(".iva-item-titleStep-_CxvN a"));
            var names = _chromeDriver.FindElements(By.CssSelector(".iva-item-titleStep-_CxvN a > h3"));
            var prices = _chromeDriver.FindElements(By.CssSelector(".price-price-BQkOZ :nth-child(2)"));

            List<string> result = new List<string>();

            for (int i = 0; i < links.Count; i++)
            {
                string price = prices[i].GetAttribute("content");

                result.Add(links[i].GetAttribute("href") + " " +
                           names[i].Text + " " + 
                           "Цена: " + (price == "..." ? "не указана" : price)
                    );;
            }

            return result;
        }
    }
}
