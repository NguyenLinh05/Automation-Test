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
    public class Order_choose_voucher// Chon voucher
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
            // 1. Chờ danh sách địa chỉ được cập nhật sau khi thêm mới
            wait.Until(d => d.FindElements(By.XPath("//nz-radio-group//div[contains(@class, 'w-full flex flex-col ng-star-inserted')]")).Count > 0);

            // 2. Lấy danh sách tất cả các địa chỉ trong `<nz-radio-group>`
            IList<IWebElement> addressDivs = driver.FindElements(By.XPath("//nz-radio-group//div[contains(@class, 'w-full flex flex-col ng-star-inserted')]"));

            if (addressDivs.Count > 0)
            {
                // 3. Chọn địa chỉ cuối cùng
                IWebElement lastAddressDiv = addressDivs.Last();
                IWebElement addressTextElement = lastAddressDiv.FindElement(By.XPath(".//span[@class='ng-star-inserted']"));

                // 5. Lấy nội dung text
                string addressText = addressTextElement.Text;

                // 4. Tìm radio button trong địa chỉ cuối cùng
                try
                {
                    IWebElement radioButton = lastAddressDiv.FindElement(By.XPath(".//span[@class='ant-radio']"));

                    // 5. Cuộn đến radio button để đảm bảo có thể click
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", radioButton);
                    Thread.Sleep(500); // Chờ một chút để cuộn hoàn tất

                    // 6. Click chọn địa chỉ
                    radioButton.Click();
                    Console.WriteLine($"Đã chọn địa chỉ cuối cùng có tên là: {addressText}");
                    Thread.Sleep(3000);
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Không tìm thấy radio button trong địa chỉ cuối.");
                }
            }
            else
            {
                Console.WriteLine("Không có địa chỉ nào để chọn.");
            }

            // Chọn phương thức vận chuyển và thanh toán
            IList<IWebElement> radioButtons = wait.Until(d => d.FindElements(By.XPath("//label[contains(@class, 'ant-radio-wrapper')]")));
            if (radioButtons.Count > 1)
            {
                IWebElement secondRadio = radioButtons[1];
                if (!secondRadio.GetAttribute("class").Contains("ant-radio-wrapper-checked"))
                {
                    secondRadio.Click();
                    Console.WriteLine("Chọn phương thức vận chuyển hoặc thanh toán");
                }
            }
            else
            {
                if (radioButtons.Count == 1)
                {
                    Console.WriteLine("Chỉ có một phương thức, hệ thống tự động chọn mặc định.");
                }
                else
                {
                    Console.WriteLine("Không tìm thấy phương thức vận chuyển hoặc thanh toán nào!");
                }
            }

            // Chon voucher 
            IWebElement voucher = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(text(), 'Chọn hoặc nhập mã')]")));
            Thread.Sleep(3000);
            voucher.Click();

            IWebElement priceElement = wait.Until(d => d.FindElement(By.XPath("//span[contains(@class, 'text-danger')]")));
            string priceText = priceElement.Text.Replace(".", "").Replace("đ", "").Trim();

            // Chuyển giá trị sang số nguyên
            int orderValue = int.Parse(priceText);
            Console.WriteLine($"Giá trị đơn hàng: {orderValue}đ");

            // Gọi class để chọn voucher tốt nhất
            Select_voucher.Select(driver, orderValue);

            //// Click nút "Đặt hàng"
            //IWebElement orderButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Đặt hàng')]]")));
            //orderButton.Click();
            //Console.WriteLine("Đặt hàng thành công!");
        }

        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(2000);
            driver.Quit();
        }
    }
}
