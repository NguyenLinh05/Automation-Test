using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using NUnit.Framework;

namespace Enduser
{
    public class Login
    {
        
        [SetUp]
        public void SetupTest(IWebDriver driver)
        {
            driver = new ChromeDriver("E:\\chromedriver-win64");
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void RunTest(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://democtv.trueconnect.vn/auth/login");
            //Đăng nhập 
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//input[@formcontrolname='username']")).SendKeys("test02");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@formcontrolname='password']")).SendKeys("default_password_zalo");
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("button[nztype='primary']")).Click();
        }
    }
}
