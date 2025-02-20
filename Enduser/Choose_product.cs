using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enduser
{
    public class Choose_product
    {
        public void RunProduct(IWebDriver driver)
        {
            //Vào trang chủ chọn sản phẩm
            //Đợi 10s để load
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.TagName("swiper-container")));

            // Chọn sản phẩm đầu tiên
            IWebElement firstProduct = driver.FindElement(By.XPath("(//swiper-slide//button)[1]"));
            firstProduct.Click();

            // Chờ tải trang chi tiết sản phẩm
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

            // Ấn tiếp tục vào trang "Yêu cầu đặt hàng"
            wait.Until(d => d.FindElement(By.XPath("//button[span[contains(text(), 'Tiếp tục')]]"))).Click();
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            Console.WriteLine("Vao y/cau thanh confg");
        }
    }
}
