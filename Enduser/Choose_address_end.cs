using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Enduser
{
    public class Choose_address_end
    {
        public void AddressEnd(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // 1. Chờ danh sách địa chỉ được cập nhật sau khi thêm mới
            wait.Until(d => d.FindElements(By.XPath("//nz-radio-group//div[contains(@class, 'w-full flex flex-col ng-star-inserted')]")).Count > 0);

            // 2. Lấy danh sách tất cả các địa chỉ trong `<nz-radio-group>`
            IList<IWebElement> addressDivs = driver.FindElements(By.XPath("//nz-radio-group//div[contains(@class, 'w-full flex flex-col ng-star-inserted')]"));

            if (addressDivs.Count > 0)
            {
                // 3. Chọn địa chỉ cuối cùng
                IWebElement lastAddressDiv = addressDivs.First(); //Last() - khi chọn địa chỉ cuối
                IWebElement addressTextElement = lastAddressDiv.FindElement(By.XPath(".//span[@class='ng-star-inserted']"));

                // 5. Lấy nội dung text
                string addressText = addressTextElement.Text;

                // 4. Tìm radio button trong địa chỉ cuối cùng
                try
                {
                    IWebElement radioButton = lastAddressDiv.FindElement(By.XPath(".//span[@class='ant-radio']"));

                    // 5. Cuộn đến radio button để đảm bảo có thể click - chỉ dùng khi chọn địa chỉ cuối
                    //((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", radioButton);
                    //Thread.Sleep(500); // Chờ một chút để cuộn hoàn tất

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
        }
    }
}
