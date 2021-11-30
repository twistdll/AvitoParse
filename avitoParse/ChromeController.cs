using System.Collections.Generic;
using System.Linq;
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
        private uint _pagesCount;

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

            SetPagesCount();
            InfoSerializer.CreateFile();

            for (uint i = 1; i <= _pagesCount; i++)
            {
                InfoSerializer.WritePage(GetAdsOnPage(), i);
                GoToNextPage(i);
            }
        }

        public void CloseDriver()
        {
            try { _chromeDriver.Close(); }
            catch { return; }
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
        private void SetPagesCount()
        {
            try
            {
                _pagesCount = ParsingMode.OnlyOnePage == false ? 
                    uint.Parse(_chromeDriver
                    .FindElement(By.CssSelector(".pagination-root-Ntd_O :nth-last-child(-n+2)")).Text) : 1;
            }
            catch (NoSuchElementException)
            {
                _pagesCount = 1;
            }
        }

        private void GoToNextPage(uint pageNumber)
        {
            string currentURL = _chromeDriver.Url;
            string newURL = pageNumber == 1 ? currentURL.Replace("q=", $"p={pageNumber + 1}&q=") : currentURL.Replace($"p={pageNumber}&q=", $"p={pageNumber + 1}&q=");
            _chromeDriver.Navigate().GoToUrl(newURL);

            Thread.Sleep(500);
        }

        private List<string> GetAdsOnPage()
        {
            List<string> links = _chromeDriver
                .FindElements(By.CssSelector(".iva-item-titleStep-_CxvN a"))
                .Select(e => e.GetAttribute("href"))
                .ToList();

            List<string> names = _chromeDriver
                .FindElements(By.CssSelector(".iva-item-titleStep-_CxvN a > h3"))
                .Select(e => e.Text)
                .ToList();
            
            List<string> prices = _chromeDriver
                .FindElements(By.CssSelector(".price-price-BQkOZ :nth-child(2)"))
                .Select(e => e.GetAttribute("content"))
                .Select(e => e == "..." ? e = "не указана" : e)
                .ToList();

            return AdsListConstructer.Construct(links,names,prices);
        }
    }
}
