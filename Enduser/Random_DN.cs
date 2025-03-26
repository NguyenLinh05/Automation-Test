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
    public class Random_DN
    {
        public void randomBusiness (IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            //Random thông tin 
            Random random = new Random();
            string randomName = "Test User " + random.Next(1000, 9999);
            string randomEmail = "test" + random.Next(1000, 9999) + "@gmail.com";
            string randomCode = "01" + random.Next(10000000, 99999999);
            string randomPhone = "01" + random.Next(10000000, 99999999);

            IWebElement name = driver.FindElement(By.XPath("//input[@formcontrolname='fullname']"));
            name.SendKeys(randomName);
            Console.WriteLine($"Nhập tên khách hàng: {randomName}");

            IWebElement email = driver.FindElement(By.XPath("//input[@formcontrolname='email']"));
            email.SendKeys(randomEmail);
            Console.WriteLine($"Nhập email: {randomEmail}");

            IWebElement contactPerson = driver.FindElement(By.XPath("//input[@formcontrolname='contactPerson']"));
            contactPerson.SendKeys(randomName);
            Console.WriteLine($"Nhập người liên hệ: {randomName}");

            driver.FindElement(By.XPath("//input[@formcontrolname='position']")).SendKeys("Nhân viên");
            Console.WriteLine("Nhập chức vụ: nhân viên");

            IWebElement taxCode = driver.FindElement(By.XPath("//input[@formcontrolname='taxCode']"));
            taxCode.SendKeys(randomCode);
            Console.WriteLine($"Nhập mã số thuế: {randomCode}");

            IWebElement mobile = driver.FindElement(By.XPath("//input[@formcontrolname='mobile']"));
            mobile.SendKeys(randomPhone);
            Console.WriteLine($"Nhập SĐT: {randomPhone}");

            IWebElement provinceDropdown = driver.FindElement(By.XPath("//nz-select[@formcontrolname='province']"));
            provinceDropdown.Click();
            Thread.Sleep(1000); // Đợi menu xổ xuống

            IList<IWebElement> provinceOptions = driver.FindElements(By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//nz-option-item"));
            if (provinceOptions.Count > 0)
            {
                int randomProvince = random.Next(0, provinceOptions.Count);
                provinceOptions[randomProvince].Click();
                Console.WriteLine($"Chọn tỉnh: {provinceOptions[randomProvince].Text}");
            }
            Thread.Sleep(1000);

            //D2. Chọn quận
            IWebElement districtDropdown = driver.FindElement(By.XPath("//nz-select[@formcontrolname='district']"));
            districtDropdown.Click();
            Thread.Sleep(1000);

            IList<IWebElement> districtOptions = driver.FindElements(By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//nz-option-item"));
            if (districtOptions.Count > 0)
            {
                int randomDistrict = random.Next(0, districtOptions.Count);
                districtOptions[randomDistrict].Click();
                Console.WriteLine($"Chọn quận: {districtOptions[randomDistrict].Text}");
            }
            Thread.Sleep(1000);

            //D3. Chọn xã
            IWebElement communeDropdown = driver.FindElement(By.XPath("//nz-select[@formcontrolname='commune']"));
            communeDropdown.Click();
            Thread.Sleep(1000);

            IList<IWebElement> communeOptions = driver.FindElements(By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//nz-option-item"));
            if (communeOptions.Count > 0)
            {
                int randomCommune = random.Next(0, communeOptions.Count);
                communeOptions[randomCommune].Click();
                Console.WriteLine($"Chọn xã: {communeOptions[randomCommune].Text}");
            }
            Thread.Sleep(1000);

            //D4. Chọn đường
            IWebElement street = driver.FindElement(By.XPath("//nz-select[@formcontrolname='address']"));
            street.Click();
            Thread.Sleep(2000);

            IList<IWebElement> streetOptions = driver.FindElements(By.XPath("//div[contains(@class, 'cdk-overlay-pane')]//nz-option-item"));
            if (streetOptions.Count > 0)
            {
                int randomStress = random.Next(0, streetOptions.Count);
                streetOptions[randomStress].Click();
                Console.WriteLine($"Chọn đường: {streetOptions[randomStress].Text}");
            }
            Thread.Sleep(2000);

            wait.Until(d => d.FindElement(By.XPath("//button[contains(text(), 'Lưu')]"))).Click();
            Console.WriteLine("Lưu địa chỉ thành công");
        }
    }
}
