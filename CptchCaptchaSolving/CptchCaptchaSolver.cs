using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace CptchCaptchaSolving
{
    public class CptchCaptchaSolver : VkNet.Utils.AntiCaptcha.ICaptchaSolver
    {

        //Ключ нужно заменить на свой со страницы https://cptch.net/profile
        private const String CPTCH_API_KEY = "0ba6b3c0c7e23ba848eebfbfe94d6afcb803184be54062da2bc3b9423e0a5ad2";

        private const String CPTCH_UPLOAD_URL = "https://cptch.net/in.php";
        private const String CPTCH_RESULT_URL = "https://cptch.net/res.php";

        public CptchCaptchaSolver() { }

        public string Solve(string url)
        {
            Console.WriteLine("Решаем капчу: " + url);
            //Скачиваем файл капчи из Вконтакте
            byte[] capcha = DownloadCapchaFromVk(url);
            if (capcha != null)
            {
                //Загружаем файл на cptch.net
                string uploadResponse = UploadCapchaToCptch(capcha);
                //Получаем из ответа id капчи
                string capchaId = ParseUploadResponse(uploadResponse);
                if (capchaId != null)
                {
                    Console.WriteLine("Id капчи: " + capchaId);
                    //Ждем несколько секунд
                    Thread.Sleep(1000);
                    //Делаем запрос на получение ответа до тех пор пока ответ не будет получен
                    string solution = null;
                    do
                    {
                        string solutionResponse = GetCapchaSolution(getCapchaRequestUri(capchaId));
                        solution = ParseSolutionResponse(solutionResponse);
                    } while (solution == null);

                    Console.WriteLine("Капча разгадана: " + solution);
                    return solution;
                }
            }
            else
            {
                Console.WriteLine("Не удалось скачать капчу с Вконтакте");
            }

            return null;
        }

        private string getCapchaRequestUri(string capchaId)
        {
            return CPTCH_RESULT_URL + "?" + "key=" + CPTCH_API_KEY + "&action=get" + "&id=" + capchaId;
        }

        private byte[] DownloadCapchaFromVk(string captchaUrl)
        {
            using (WebClient client = new WebClient())
            using (Stream s = client.OpenRead(captchaUrl))
            {
                return client.DownloadData(captchaUrl);
            }
        }

        private string UploadCapchaToCptch(byte[] capcha)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                MultipartFormDataContent form = new MultipartFormDataContent();

                form.Add(new StringContent(CPTCH_API_KEY), "key");
                form.Add(new StringContent("post"), "method");
                form.Add(new ByteArrayContent(capcha, 0, capcha.Length), "file", "capcha");
                var response = httpClient.PostAsync(CPTCH_UPLOAD_URL, form).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;
                    return responseContent.ReadAsStringAsync().Result;
                }
                else
                {
                    return null;
                }
            }
        }

        private string ParseUploadResponse(string uploadResponse)
        {
            if (uploadResponse.Contains("ERROR"))
            {
                Console.WriteLine("Возникла ошибка при загрузке капчи");
                return null;
            }
            else if(uploadResponse.Contains("OK")) 
            {
                return uploadResponse.Split('|')[1];
            }
            return null;
        }

        public static String GetCapchaSolution(string siteUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(siteUrl);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private string ParseSolutionResponse(string response)
        {
            if (response.Equals("ERROR"))
            {
                Console.WriteLine("Error when get captcha result: " + response);
                return null;
            }
            else if(response.Equals("CAPCHA_NOT_READY"))
            {
                Console.WriteLine("Not ready now");
                Thread.Sleep(1000);
                return null;
            } else if (response.Contains("OK"))
            {
                return response.Split('|')[1];
            }
            return null;
        }

        public void CaptchaIsFalse()
        {
            Console.WriteLine("Capcha is false");
        }
    }
}
