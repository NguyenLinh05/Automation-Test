using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;

namespace Enduser
{
    public class Random_product //Đặt hàng random sản phẩm
    {
        [Test]
        public void RandomProduct(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(ExpectedConditions.UrlContains("/home"));

            // 2. Chờ và click vào nút "Cửa hàng" 
            IWebElement storeLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//a[span[contains(text(), 'Cửa hàng')]]")));
            storeLink.Click();

            // 3. Chờ cho đến khi trang chuyển hướng thành công đến `/card-order/list?tab=shop`
            wait.Until(ExpectedConditions.UrlContains("/card-order/list?tab=shop"));

            //4. Danh sách sản phẩm 
            IWebElement productContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.flex.flex-wrap.w-full.p-1.overflow-scroll")));

            // 5. Tìm tất cả các sản phẩm trong danh sách
            IList<IWebElement> products = productContainer.FindElements(By.TagName("app-card-list-item"));

            // 6. Random 1 sản phẩm
            if (products.Count > 0)
            {
                // Random chọn một sản phẩm
                Random random = new Random();
                int randomIndex = random.Next(0, products.Count);

                IWebElement selectedProduct = products[randomIndex];

                // Lấy tên sản phẩm
                try
                {
                    IWebElement productNameElement = selectedProduct.FindElement(By.XPath(".//div[contains(@class, 'line-clamp-2')]/span"));
                    string productName = productNameElement.Text.Trim();
                    Console.WriteLine($"Chọn sản phẩm: {productName}");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Không lấy được tên sản phẩm!");
                }

                // Click vào sản phẩm được chọn ngẫu nhiên
                selectedProduct.Click();

                Console.WriteLine($"Chọn sản phẩm thứ {randomIndex + 1} thành công!");
                // 7. Chờ trang chuyển đến chi tiết sản phẩm `/card-order/details/{id}`
                wait.Until(ExpectedConditions.UrlMatches(@"\/card-order\/details\/\d+"));
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
                Console.WriteLine("Chuyển đến trang chi tiết sản phẩm thành công!");
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(50);
            }
            else
            {
                Console.WriteLine("Không tìm thấy sản phẩm nào trong danh sách.");
            }
        }
    }
}
