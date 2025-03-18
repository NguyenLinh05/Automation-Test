using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SeleniumExtras.WaitHelpers;

namespace Enduser
{
    public class Add_product_cart
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

            //Chọn sản phẩm 
            Random_product random_Product = new Random_product();
            random_Product.RandomProduct(driver);

            //Thêm vào giỏ
            wait.Until(d => d.FindElement(By.XPath("//button[span[contains(text(), 'Thêm vào giỏ hàng')]]"))).Click();
            Console.WriteLine("Thêm sp vào giỏ thành công");

            //Chuyển vào giỏ hàng
            wait.Until(m=>m.FindElement(By.XPath("//span[@nztype='ng-zorro:cartIcon']"))).Click();
            Console.WriteLine("Chuyển đến trang giỏ hàng");

            //Chuyển đến yêu cầu đặt hàng
            wait.Until(m => m.FindElement(By.XPath("//button[contains(@class, 'ant-btn-primary') and span[contains(text(), 'Mua hàng')]]"))).Click();
            Console.WriteLine("Chuyển đến trang y/cầu đặt hàng");
            Thread.Sleep(2000);

            // Kiểm tra xem có nút "Thêm địa chỉ nhận hàng" hoặc "Thiết lập" không
            bool hasAddAddressButton = driver.FindElements(By.XPath("//div[span[contains(text(), 'Thêm địa chỉ nhận hàng')]]")).Count > 0;
            bool hasSetupButton = driver.FindElements(By.XPath("//p[contains(@class, 'text-[13px] text-primary cursor-pointer') and contains(text(), 'Thiết lập')]")).Count > 0;

            if (hasAddAddressButton)
            {
                Console.WriteLine("Không có địa chỉ nhận hàng, bấm vào 'Thêm địa chỉ nhận hàng'");
                wait.Until(d => d.FindElement(By.XPath("//div[span[contains(text(), 'Thêm địa chỉ nhận hàng')]]"))).Click();
            }
            else if (hasSetupButton)
            {
                Console.WriteLine("Có địa chỉ nhận hàng, bấm vào 'Thiết lập'");
                wait.Until(d => d.FindElement(By.XPath("//p[contains(@class, 'text-[13px] text-primary cursor-pointer') and contains(text(), 'Thiết lập')]"))).Click();
            }

            // Nếu bấm vào "Thêm địa chỉ nhận hàng" hoặc "Thiết lập" thì tiếp tục tạo địa chỉ mới
            if (hasAddAddressButton || hasSetupButton)
            {
                Console.WriteLine("Chuyển đến trang Chọn địa chỉ nhận hàng");
                wait.Until(ExpectedConditions.UrlContains("/card-order/addressList"));

                IWebElement addNewAddressButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(), 'Thêm địa chỉ nhận hàng')]")));
                addNewAddressButton.Click();
                Console.WriteLine("Click vào 'Thêm địa chỉ nhận hàng' thành công!");

                Random_address.Select(driver);//Them moi dia chi chon random
            }

            // Chọn địa chỉ nhận hàng
            Choose_address_end choose = new Choose_address_end();
            choose.AddressEnd(driver);

            // Click nút "Đặt hàng"
            IWebElement orderButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Đặt hàng')]]")));
            orderButton.Click();
            Console.WriteLine("Đặt hàng thành công!");


        }
        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(20000);
            driver.Quit();
        }
    }
}
