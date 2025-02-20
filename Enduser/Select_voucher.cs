using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Enduser
{
    public class Select_voucher
    {
        public static void Select(IWebDriver driver, int orderValue)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Chờ danh sách voucher hiển thị
            wait.Until(d => d.FindElements(By.XPath("//div[contains(@class, 'w-full border-solid border-[1px] rounded-lg flex mt-2')]")).Count > 0);

            IList<IWebElement> vouchers = driver.FindElements(By.XPath("//div[contains(@class, 'w-full border-solid border-[1px] rounded-lg flex mt-2')]"));

            IWebElement bestVoucher = null;
            int maxDiscount = 0;

            foreach (var voucher in vouchers)
            {
                try
                {
                    IWebElement voucherTextElement = voucher.FindElement(By.XPath(".//p[contains(@class, 'mb-0')]"));
                    string voucherText = voucherTextElement.Text;

                    int discountValue = ExtractDiscountValue(voucherText);
                    int minOrderValue = ExtractMinOrderValue(voucherText);

                    if (orderValue >= minOrderValue && discountValue > maxDiscount)
                    {
                        maxDiscount = discountValue;
                        bestVoucher = voucher;
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Không tìm thấy nội dung voucher.");
                }
            }

            if (bestVoucher != null)
            {
                IWebElement radioButton = bestVoucher.FindElement(By.XPath(".//input[@type='radio']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", radioButton);
                Thread.Sleep(500);
                radioButton.Click();
                Console.WriteLine($"Chọn voucher giảm {maxDiscount}đ");
            }
            else
            {
                Console.WriteLine("Không có voucher phù hợp.");
            }
        }

        private static int ExtractDiscountValue(string text)
        {
            Match percentMatch = Regex.Match(text, @"(\d+)%");
            Match amountMatch = Regex.Match(text, @"(\d+)[ ]?[đd]");

            if (percentMatch.Success)
                return int.Parse(percentMatch.Groups[1].Value) * 1000;
            else if (amountMatch.Success)
                return int.Parse(amountMatch.Groups[1].Value.Replace(".", ""));

            return 0;
        }

        private static int ExtractMinOrderValue(string text)
        {
            Match minOrderMatch = Regex.Match(text, @"tối thiểu[ ]?(\d+)[ ]?[đd]");
            if (minOrderMatch.Success)
                return int.Parse(minOrderMatch.Groups[1].Value.Replace(".", ""));
            return 0;
        }
    }
}
