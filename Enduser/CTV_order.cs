using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Threading;

namespace Enduser
{
    public class CTV_order
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
            
            //Goi chon random san pham
            Random_product random_Product = new Random_product();
            random_Product.RandomProduct(driver);

            //8. Click tiếp tục để chuyển sang trang yêu cầu đặt hàng
            IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Tiếp tục')]]")));
            continueButton.Click();
            Console.WriteLine("Click nút 'Tiếp tục' thành công!");

            // Chờ trang "Yêu cầu đặt hàng" tải hoàn tất
            wait.Until(ExpectedConditions.UrlContains("/card-order/checkout"));
            Console.WriteLine("Chuyển đến trang yêu cầu đặt hàng thành công!");

            //Chọn đặt cho khách 
            IWebElement customerRadio = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@nz-radio and @nzvalue='CUSTOMER']")));
            customerRadio.Click();
            Console.WriteLine("Chọn đặt cho khách!");
            Thread.Sleep(1000);

            //Thêm địa chỉ 
            IWebElement addButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[span[contains(text(), 'Thêm thông tin khách hàng')]]")));
            addButton.Click();
            Thread.Sleep(500);
            IWebElement addAddress = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[span[contains(text(), 'Thêm thông tin khách hàng')]]")));
            addAddress.Click();
            Thread.Sleep(500);

            //Thêm mới địa chỉ 
            Dictionary<string, string> customerTypes = new Dictionary<string, string>
            {
                { "Cá nhân", "//nz-option-item[contains(@title, 'Cá nhân')]" },
                { "Doanh nghiệp", "//nz-option-item[contains(@title, 'Doanh nghiệp')]" }
            };

            // 2. Random chọn loại khách hàng
            Random random = new Random();
            List<string> keys = new List<string>(customerTypes.Keys);
            string selectedType = keys[random.Next(keys.Count)];
            string xpath = customerTypes[selectedType];

            // 3. Click vào dropdown để mở danh sách
            IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//nz-select[@formcontrolname='customerType']")));
            dropdown.Click();
            Thread.Sleep(500); // Đợi dropdown mở ra

            // 4. Click vào lựa chọn ngẫu nhiên
            IWebElement option = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
            option.Click();

            Console.WriteLine($"Đã chọn loại khách hàng: {selectedType}");

            if (selectedType == "Cá nhân")
            {
                Random_CN randomA = new Random_CN();
                randomA.randomIndividual(driver);
            }
            else
            {
                Random_DN randomB = new Random_DN();
                randomB.randomBusiness(driver);
            }
            Thread.Sleep(2000);

            Choose_address_first choose = new Choose_address_first();
            choose.AddressEnd(driver);
            Thread.Sleep(2000);

            IWebElement orderButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Đặt hàng')]]")));
            orderButton.Click();
            Console.WriteLine("Đặt hàng thành công!");
            Thread.Sleep(2000);

            //Quay lại trang chủ
            IWebElement logo = driver.FindElement(By.XPath("//div[@class='cursor-pointer ng-tns-c184-1 ng-star-inserted']/img"));
            logo.Click();
            Console.WriteLine("Chuyển về trang chủ thành công");
            Thread.Sleep(1000);

            //Xem lại đơn hàng
            IWebElement accountTab = driver.FindElement(By.CssSelector("a[href='/user/config'] span.tab-account"));
            accountTab.Click();
            Console.WriteLine("Chuyển đến trang thông tin tk thành công");
            Thread.Sleep(1000);

            //Click Xem đơn hàng
            IWebElement prodDetail = driver.FindElement(By.CssSelector("a[href='/user/order']"));
            prodDetail.Click();
            Thread.Sleep(1000);

            //Xem chi tiết đơn hàng
            IWebElement orderItem = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class, 'py-2')]/div[contains(@class, 'cursor-pointer')]")));
            IWebElement orderCodeElement = orderItem.FindElement(By.XPath(".//div[contains(text(), 'Mã đơn hàng:')]"));
            string orderCode = orderCodeElement.Text.Replace("Mã đơn hàng:", "").Trim();
            orderItem.Click();
            Console.WriteLine($"Chọn đơn hàng có mã: {orderCode}");

            // Kiểm tra lại mã đơn ở trang chi tiết
            IWebElement orderCodeE = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//span[contains(text(), 'TCSDH')]") // Chọn span chứa mã đơn hàng
            ));

            // 3. Lấy nội dung mã đơn hàng
            string orderC = orderCodeE.Text.Trim();

            // 4. In mã đơn hàng ra Console
            Console.WriteLine("---Thông tin đơn hàng---");
            Console.WriteLine($"Mã đơn hàng: {orderC}");
            Thread.Sleep(2000);

        }

        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(2000);
            driver.Quit();
        }
    }
}
