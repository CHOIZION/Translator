using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

// Token: 0x0200000F RID: 15
public static class PAPAGO
{
    // Token: 0x06000097 RID: 151 RVA: 0x0000789C File Offset: 0x00005A9C
    public static void Load()
    {
        PAPAGO.SOURCE = DEFINE.GetIniValue("PAPAGO", "SOURCE", "TranslationTool.ini", "en");
        PAPAGO.TARGET = DEFINE.GetIniValue("PAPAGO", "TARGET", "TranslationTool.ini", "ko");
        PAPAGO.HONORIFIC = DEFINE.GetIniValue("PAPAGO", "HONORIFIC", "TranslationTool.ini", "false");
    }

    // Token: 0x06000098 RID: 152 RVA: 0x00007904 File Offset: 0x00005B04
    public static void Save()
    {
        DEFINE.SetIniValue("PAPAGO", "SOURCE", PAPAGO.SOURCE, "TranslationTool.ini");
        DEFINE.SetIniValue("PAPAGO", "TARGET", PAPAGO.TARGET, "TranslationTool.ini");
        DEFINE.SetIniValue("PAPAGO", "HONORIFIC", PAPAGO.HONORIFIC, "TranslationTool.ini");
    }

    // Token: 0x06000099 RID: 153 RVA: 0x0000795C File Offset: 0x00005B5C
    public static string Translation(string text)
    {
        string result;
        try
        {
            Guid guid = Guid.NewGuid();
            string uriString = "https://papago.naver.com/apis/n2mt/translate";
            string format = "honorific={0}&source={1}&target={2}&text={3}";
            string s = "v1.5.2_0d13cb6cf4";
            Uri uri = new Uri(uriString);
            string s2 = string.Format(format, new object[]
            {
                PAPAGO.HONORIFIC,
                PAPAGO.SOURCE,
                PAPAGO.TARGET,
                Uri.EscapeDataString(text)
            });
            double num = Math.Truncate(DateTime.Now.Subtract(DateTime.MinValue.AddYears(1969)).TotalMilliseconds);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] bytes2 = Encoding.UTF8.GetBytes(string.Format("{0}\n{1}\n{2}", guid, uri, num));
            string arg = Convert.ToBase64String(new HMACMD5(bytes).ComputeHash(bytes2));
            byte[] bytes3 = Encoding.UTF8.GetBytes(s2);
            int num2 = bytes3.Length;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Set("Authorization", string.Format("PPG {0}:{1}", guid, arg));
            httpWebRequest.Headers.Set("Timestamp", num.ToString());
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            httpWebRequest.ContentLength = (long)num2;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes3, 0, num2);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            result = JObject.Parse(json)["translatedText"].ToString();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result = "";
        }
        return result;
    }

    // Token: 0x04000078 RID: 120
    public static Dictionary<string, string> _SourceList = new Dictionary<string, string>
    {
        {
            "auto",
            "언어감지"
        },
        {
            "en",
            "영어"
        },
        {
            "ja",
            "일본어"
        },
        {
            "zh-CN",
            "중국어(간체)"
        },
        {
            "zh-TW",
            "중국어(번체)"
        },
        {
            "es",
            "스페인어"
        },
        {
            "fr",
            "프랑스어"
        },
        {
            "de",
            "독일어"
        },
        {
            "ru",
            "러시아어"
        },
        {
            "tk",
            "이탈리아어"
        },
        {
            "vi",
            "베트남어"
        },
        {
            "th",
            "태국어"
        },
        {
            "id",
            "인도네이사어"
        }
    };

    // Token: 0x04000079 RID: 121
    public static string SOURCE = "auto";

    // Token: 0x0400007A RID: 122
    public static string TARGET = "ko";

    // Token: 0x0400007B RID: 123
    public static string HONORIFIC = "false";
}
