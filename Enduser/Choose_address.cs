using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enduser
{
    public class Choose_address
    {
        public void RandomAdd(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            IList<IWebElement> addressRadios = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(
        By.XPath("//span[contains(@class, 'ant-radio')]")));


            if (addressRadios.Count > 0)
            {
                // Random một chỉ số từ 0 đến số lượng radio button - 1
                Random random = new Random();
                int randomIndex = random.Next(addressRadios.Count);
                IWebElement selectedRadio = addressRadios[randomIndex];

                // Nếu radio chưa được chọn thì click vào
                if (!selectedRadio.Selected)
                {
                    selectedRadio.Click();
                    Console.WriteLine($"Đã chọn địa chỉ nhận hàng số {randomIndex + 1}");
                }
                else
                {
                    Console.WriteLine($"Địa chỉ nhận hàng số {randomIndex + 1} đã được chọn trước đó.");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy địa chỉ nhận hàng nào!");
            }
        }
    }
}
