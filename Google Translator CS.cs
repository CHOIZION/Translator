using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

// Token: 0x02000037 RID: 55
public static class GOOGLE
{
    // Token: 0x0600016C RID: 364 RVA: 0x00012644 File Offset: 0x00010844
    public static void Load()
    {
        GOOGLE.SOURCE = DEFINE.GetIniValue("KAKAO", "SOURCE", "TranslationTool.ini", "auto");
        GOOGLE.TARGET = DEFINE.GetIniValue("KAKAO", "TARGET", "TranslationTool.ini", "kr");
        GOOGLE._SourceList.Add("auto", "언어감지");
        GOOGLE._SourceList.Add("en", "영어");
        GOOGLE._SourceList.Add("jp", "일본어");
        GOOGLE._SourceList.Add("cn", "중국어");
        GOOGLE._SourceList.Add("nl", "네덜란드어");
        GOOGLE._SourceList.Add("de", "독일어");
        GOOGLE._SourceList.Add("ru", "러시아어");
        GOOGLE._SourceList.Add("ms", "말레이시아어");
        GOOGLE._SourceList.Add("bn", "벵골어");
        GOOGLE._SourceList.Add("vi", "베트남어");
        GOOGLE._SourceList.Add("es", "스페인어");
        GOOGLE._SourceList.Add("ar", "아랍어");
        GOOGLE._SourceList.Add("it", "이탈리아어");
        GOOGLE._SourceList.Add("id", "인도네시아어");
        GOOGLE._SourceList.Add("th", "태국어");
        GOOGLE._SourceList.Add("tr", "터키어");
        GOOGLE._SourceList.Add("pt", "포르투갈어");
        GOOGLE._SourceList.Add("fr", "프랑스어");
        GOOGLE._SourceList.Add("hi", "힌디어");
    }

    // Token: 0x0600016D RID: 365 RVA: 0x000029DC File Offset: 0x00000BDC
    public static void Save()
    {
        DEFINE.SetIniValue("KAKAO", "SOURCE", GOOGLE.SOURCE, "TranslationTool.ini");
        DEFINE.SetIniValue("KAKAO", "TARGET", GOOGLE.TARGET, "TranslationTool.ini");
    }

    // Token: 0x0600016E RID: 366 RVA: 0x0001280C File Offset: 0x00010A0C
    public static string Translation(string _Text)
    {
        string result;
        try
        {
            string text = string.Format("client=gtx&sl=auto&tl=ko&dt=t&q={0}", HttpUtility.UrlEncode(_Text));
            Encoding.UTF8.GetBytes(text);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://translate.google.com/translate_a/single?" + text);
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36";
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
            string text2 = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            string[] array = text2.Split(new char[]
            {
                '\r',
                '\n'
            });
            foreach (string text3 in array)
            {
                try
                {
                    int num = text3.IndexOf("[\"");
                    int num2 = text3.IndexOf("\",\"", num);
                    int num3 = text3.IndexOf("\",null", num2);
                    if (num >= 0 && num < text3.Length)
                    {
                        if (num2 >= 0 && num2 < text3.Length)
                        {
                            if (num3 >= 0 && num3 < text3.Length)
                            {
                                string text4 = text3.Substring(num + 2, num2 - num - 2);
                                string text5 = text3.Substring(num2 + 3, num3 - num2 - 3);
                                if (text4.EndsWith("\\n"))
                                {
                                    text4 = text4.Remove(text4.Length - 2);
                                }
                                if (text5.EndsWith("\\n"))
                                {
                                    text5 = text5.Remove(text5.Length - 2);
                                }
                                _Text = _Text.Replace(text5, text4);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            result = _Text;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            result = "";
        }
        return result;
    }

    // Token: 0x0600016F RID: 367 RVA: 0x00012A14 File Offset: 0x00010C14
    public static void Parsing(string text)
    {
        string text2 = "";
        int num = text.IndexOf(string.Format(",,\"en\"", new object[0]));
        if (num == -1)
        {
            int num2 = text.IndexOf('"');
            if (num2 != -1)
            {
                int num3 = text.IndexOf('"', num2 + 1);
                if (num3 != -1)
                {
                    text2 = text.Substring(num2 + 1, num3 - num2 - 1);
                }
            }
        }
        else
        {
            text = text.Substring(0, num);
            text = text.Replace("],[", ",");
            text = text.Replace("]", string.Empty);
            text = text.Replace("[", string.Empty);
            text = text.Replace("\",\"", "\"");
        }
        string[] array = text.Split(new char[]
        {
            '"'
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < Enumerable.Count<string>(array); i += 2)
        {
            string text3 = array[i];
            if (text3.StartsWith(",,"))
            {
                i--;
            }
            else
            {
                text2 = text2 + text3 + "  ";
            }
        }
        text2 = text2.Trim();
        text2 = text2.Replace(" ?", "?");
        text2 = text2.Replace(" !", "!");
        text2 = text2.Replace(" ,", ",");
        text2 = text2.Replace(" .", ".");
        text2 = text2.Replace(" ;", ";");
        Console.WriteLine(text2);
    }

    // Token: 0x040001D3 RID: 467
    public static Dictionary<string, string> _SourceList = new Dictionary<string, string>();

    // Token: 0x040001D4 RID: 468
    public static string SOURCE = "auto";

    // Token: 0x040001D5 RID: 469
    public static string TARGET = "kr";
}
