using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Enduser
{
    /// <summary>
    /// Đăng ký cộng tác viên
    /// </summary>
    public class Register_Contributors 
    {
        IWebDriver driver;
        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver("E:\\chromedriver-win64");
            driver.Manage().Window.Maximize();
        }

        [Test]

        public void RunOrder()
        {
            //Goi login
            Login login = new Login();
            login.RunTest(driver);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            Thread.Sleep(1000);

            //Chọn tiện ích
            IWebElement prodDetail = driver.FindElement(By.CssSelector("a[href='/user/convenience']"));
            prodDetail.Click();
            Thread.Sleep(1000);

            try
            {
                // 1. Tìm button "Đăng ký ngay"
                IWebElement registerButton = driver.FindElement(By.XPath("//button[span[contains(text(), 'Đăng ký ngay')]]"));
                registerButton.Click();
                // 2. Nếu tìm thấy button thì tiếp tục chạy chương trình
                Console.WriteLine("Button 'Đăng ký ngay' tồn tại. ");
                Thread.Sleep(1000);
                driver.FindElement(By.XPath("//input[@formcontrolname='referralCode']")).SendKeys("testhh01");
                Console.WriteLine("Nhập mã giới thiệu: testhh01");
                Thread.Sleep(1000);
                IWebElement saveButton = driver.FindElement(By.XPath("//button[span[contains(text(), 'Lưu thay đổi')]]"));
                saveButton.Click();
                Console.WriteLine("Đăng ký thành công");
                Thread.Sleep(1000);
                IWebElement orderButton = driver.FindElement(By.XPath("//div[span[contains(text(), 'Xem lại thông tin đã đăng ký')]]"));
                orderButton.Click();
                Thread.Sleep(1000);

            }
            catch (NoSuchElementException)
            {
                // 3. Nếu không tìm thấy button, in ra console và kết thúc chương trình
                Console.WriteLine("Không tìm thấy button 'Đăng ký ngay'");
                IWebElement orderButton = driver.FindElement(By.XPath("//div[span[contains(text(), 'Xem lại thông tin đã đăng ký')]]"));
                orderButton.Click();
                Thread.Sleep(1000);
                driver.Quit(); // Đóng trình duyệt
            }

        }
        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(2000);
            driver.Quit();
        }
    }
}
