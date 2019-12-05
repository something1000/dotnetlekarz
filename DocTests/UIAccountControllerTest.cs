using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DocTests
{
    public class UIAccountControllerTest : IDisposable
    {
        private readonly IWebDriver _driver;
        public UIAccountControllerTest()
        {
            _driver = new ChromeDriver();
        }

        [Fact]
        public void GoToLoginPageAndTryToLoginWithDefaultAccountThenLogout()
        {
            _driver.Navigate()
                .GoToUrl("https://localhost:5001/Account/Login");

            var loginElem = _driver.FindElement(By.Id("login"));
            var passwordElem = _driver.FindElement(By.Id("password"));
            var submitButton = _driver.FindElement(By.XPath("/html/body/div/main/div/div/div/form/div[3]/button"));

            Assert.NotNull(loginElem);
            Assert.NotNull(passwordElem);
            Assert.NotNull(submitButton);


            loginElem.SendKeys("jkowalski");
            passwordElem.SendKeys("123");
            submitButton.Click();
            
            Assert.Contains("Doktor: jkowalski", _driver.PageSource);


            var logoutLink = _driver.FindElement(By.LinkText("Wyloguj"));
            Assert.NotNull(logoutLink);

            logoutLink.Click();


        }

        public void Dispose()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }
}
