using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;
using System.Text;
using System.Threading;

namespace Enduser
{
    public class Signup
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver(); // Không cần chỉ định đường dẫn nếu đã đặt ChromeDriver vào PATH
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        [Test]
        public void RunOrder()
        {
            driver.Navigate().GoToUrl("https://democtv.trueconnect.vn/auth/login");

            // Click "Đăng ký ngay"
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(), 'Đăng ký ngay')]"))).Click();

            // Random dữ liệu đăng ký
            Random random = new Random();
            string randomUsername = "Test" + random.Next(100, 999);
            string randomHoten = "Nguyễn " + random.Next(10, 99);
            string randomEmail = "mai" + random.Next(100, 999) + "@gmail.com";
            string randomSDT = "03" + random.Next(10000000, 99999999);
            string randomPassword = GenerateRandomPassword(12);

            // Nhập thông tin
            EnterText("//input[@placeholder='Tên đăng nhập']", randomUsername);
            EnterText("//input[@placeholder='Họ tên']", randomHoten);
            EnterText("//input[@placeholder='Email']", randomEmail);
            EnterText("//input[@placeholder='Số điện thoại']", randomSDT);
            EnterText("//input[@placeholder='Mật khẩu']", randomPassword);
            EnterText("//input[@placeholder='Nhập lại mật khẩu']", randomPassword);

            // Xử lý mã giới thiệu
            IWebElement referralCodeField = driver.FindElement(By.XPath("//input[@placeholder='Mã giới thiệu']"));
            if (string.IsNullOrEmpty(referralCodeField.GetAttribute("value")))
            {
                referralCodeField.Clear();
                referralCodeField.SendKeys("TrueRetail");
                Console.WriteLine("Nhập mã giới thiệu: TrueRetail");
            }

            // Click Đăng ký
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[nztype='primary']"))).Click();
            Console.WriteLine("✅ Click Đăng ký thành công");
            Thread.Sleep(10000);

            // Chờ popup OTP hiển thị
            WaitForOTPPopup(driver);
            Thread.Sleep(10000);

        }

        [TearDown]
        public void CloseTest()
        {
            //Thread.Sleep(5000);
            driver.Quit();
        }

        // Hàm nhập dữ liệu vào input field
        private void EnterText(string xpath, string value)
        {
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            element.Clear();
            element.SendKeys(value);
            Console.WriteLine($"Nhập: {value}");
        }

        // Hàm tạo mật khẩu ngẫu nhiên
        private string GenerateRandomPassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Hàm chờ popup OTP hiển thị
        private void WaitForOTPPopup(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Kiểm tra popup có tồn tại không
            var otpPopups = driver.FindElements(By.XPath("//div[@role='document']"));
            if (otpPopups.Count > 0)
            {
                Console.WriteLine("✅ Popup xác thực email đã hiển thị!");

                // Tìm nút đóng trong popup
                var closeButtons = driver.FindElements(By.XPath("//div[@role='document']//button[@aria-label='Close']"));
                if (closeButtons.Count > 0)
                {
                    IWebElement closeButton = closeButtons[0];

                    // Cuộn đến nút nếu cần
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", closeButton);
                    Thread.Sleep(500); // Đợi một chút để đảm bảo có thể click

                    // Click bằng JavaScript nếu click thông thường không hoạt động
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", closeButton);
                    Console.WriteLine("✅ Đã đóng popup xác thực email!");
                }
                else
                {
                    Console.WriteLine("❌ Không tìm thấy nút đóng popup!");
                }
            }
            else
            {
                Console.WriteLine("❌ Không tìm thấy popup xác thực email!");
            }
        }



    }
}
