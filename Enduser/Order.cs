using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enduser
{
    public class Order
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
            
            //Goi chon san pham dau tien
            //Choose_product choose_Product = new Choose_product();
            //choose_Product.RunProduct(driver);

            //Goi chon random san pham
            Random_product random_Product = new Random_product();
            random_Product.RandomProduct(driver);

            // Chờ trang "Yêu cầu đặt hàng" tải hoàn tất
            wait.Until(ExpectedConditions.UrlContains("/card-order/checkout"));
            Console.WriteLine("Chuyển đến trang yêu cầu đặt hàng thành công!");

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

            //// Chọn phương thức vận chuyển và thanh toán
            //ChoosePay choosePay = new ChoosePay();
            //choosePay.Choose_Pay(driver);
            //Thread.Sleep(5000);

            //// Chon voucher 
            //IWebElement voucher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(text(), 'Chọn hoặc nhập mã')]")));
            //Thread.Sleep(3000);
            //voucher.Click();

            //IWebElement priceElement = wait.Until(d => d.FindElement(By.XPath("//span[contains(@class, 'text-danger')]")));
            //string priceText = priceElement.Text.Replace(".", "").Replace("đ", "").Trim();

            //// Chuyển giá trị sang số nguyên
            //int orderValue = int.Parse(priceText);
            //Console.WriteLine($"Giá trị đơn hàng: {orderValue}đ");

            //// Gọi class để chọn voucher tốt nhất
            //Select_voucher.Select(driver, orderValue);

            // Click nút "Đặt hàng"
            IWebElement orderButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Đặt hàng')]]")));
            orderButton.Click();
            Console.WriteLine("Đặt hàng thành công!");
        }

        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(2000);
            driver.Quit();
        }
    }
}
