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
            driver = new ChromeDriver(EnvConfig.ChromeDriverPath);
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
            Thread.Sleep(1000);

            // Kiểm tra xem có nút "Thêm địa chỉ nhận hàng" hoặc "Thiết lập" không
            //bool hasAddAddressButton = driver.FindElements(By.XPath("//div[span[contains(text(), 'Thêm địa chỉ nhận hàng')]]")).Count > 0;
            //bool hasSetupButton = driver.FindElements(By.XPath("//p[contains(@class, 'text-[13px] text-primary cursor-pointer') and contains(text(), 'Thiết lập')]")).Count > 0;

            //if (hasAddAddressButton)
            //{
            //    Console.WriteLine("Không có địa chỉ nhận hàng, bấm vào 'Thêm địa chỉ nhận hàng'");
            //    wait.Until(d => d.FindElement(By.XPath("//div[span[contains(text(), 'Thêm địa chỉ nhận hàng')]]"))).Click();
            //}
            //else if (hasSetupButton)
            //{
            //    Console.WriteLine("Có địa chỉ nhận hàng, bấm vào 'Thiết lập'");
            //    wait.Until(d => d.FindElement(By.XPath("//p[contains(@class, 'text-[13px] text-primary cursor-pointer') and contains(text(), 'Thiết lập')]"))).Click();
            //}

            //// Nếu bấm vào "Thêm địa chỉ nhận hàng" hoặc "Thiết lập" thì tiếp tục tạo địa chỉ mới
            //if (hasAddAddressButton || hasSetupButto
            //n)
            //{
            //    Console.WriteLine("Chuyển đến trang Chọn địa chỉ nhận hàng");
            //    wait.Until(ExpectedConditions.UrlContains("/card-order/addressList"));

            //    IWebElement addNewAddressButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(), 'Thêm địa chỉ nhận hàng')]")));
            //    addNewAddressButton.Click();
            //    Console.WriteLine("Click vào 'Thêm địa chỉ nhận hàng' thành công!");

            //    Random_address.Select(driver);//Them moi dia chi chon random
            //}

            // Chọn địa chỉ nhận hàng
            //Choose_address_end choose = new Choose_address_end();
            //choose.AddressEnd(driver);

            //Choose_address choose_add = new Choose_address();
            //choose_add.RandomAdd(driver);


            // Click nút "Đặt hàng"
            IWebElement orderButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[span[contains(text(), 'Đặt hàng')]]")));
            orderButton.Click();
            Console.WriteLine("Đặt hàng thành công!");
            Thread.Sleep(2000);

            //Quay lại trang chủ
            IWebElement logo = driver.FindElement(By.XPath("//div[contains(@class, 'cursor-pointer')]/img[contains(@class, 'h-6') and contains(@class, 'pl-6') and contains(@class, 'scale-[160%]')]"));
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
            Thread.Sleep(1000);
        }

        [TearDown]
        public void CloseTest()
        {
            Thread.Sleep(2000);
            driver.Quit();
        }
    }
}
