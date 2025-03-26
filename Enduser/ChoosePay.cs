using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enduser
{
    public class ChoosePay
    {
         public void Choose_Pay(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            // Lấy tất cả radio button của phương thức vận chuyển
            IList<IWebElement> shippingOptions = wait.Until(d => d.FindElements(By.XPath("//div[@nz-radio]")));

            if (shippingOptions.Count > 0)
            {
                // Random chỉ số từ 0 đến số lượng radio button - 1
                Random random = new Random();
                int randomIndex = random.Next(shippingOptions.Count);

                IWebElement selectedOption = shippingOptions[randomIndex];

                // Lấy tên phương thức vận chuyển (span đầu tiên bên trong)
                IWebElement shippingName = selectedOption.FindElement(By.XPath(".//span/span[@class='block !w-full']"));
                string name = shippingName.Text;

                // Kiểm tra nếu radio đã được chọn thì bỏ qua việc click
                if (!selectedOption.GetAttribute("class").Contains("ant-radio-wrapper-checked"))
                {
                    selectedOption.Click();
                    Console.WriteLine($"Đã chọn phương thức vận chuyển: {name}");
                }
                else
                {
                    Console.WriteLine($"Phương thức '{name}' đã được chọn. Tiếp tục chương trình.");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy phương thức vận chuyển nào!");
            }

        }
    }
}
