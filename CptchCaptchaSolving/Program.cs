using System;
using System.Text;

namespace CptchCaptchaSolving
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            CptchCaptchaSolver solver = new CptchCaptchaSolver();

            string testCaptchaUrl = "https://vk.com/captcha.php?sid=948716433991&s=1";
            solver.Solve(testCaptchaUrl);
        }
    }
}
