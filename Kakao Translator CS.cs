using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

// Token: 0x02000036 RID: 54
public static class KAKAO
{
    // Token: 0x06000168 RID: 360 RVA: 0x000122B4 File Offset: 0x000104B4
    public static void Load()
    {
        KAKAO.SOURCE = DEFINE.GetIniValue("KAKAO", "SOURCE", "TranslationTool.ini", "auto");
        KAKAO.TARGET = DEFINE.GetIniValue("KAKAO", "TARGET", "TranslationTool.ini", "kr");
        KAKAO._SourceList.Add("auto", "언어감지");
        KAKAO._SourceList.Add("en", "영어");
        KAKAO._SourceList.Add("jp", "일본어");
        KAKAO._SourceList.Add("cn", "중국어");
        KAKAO._SourceList.Add("nl", "네덜란드어");
        KAKAO._SourceList.Add("de", "독일어");
        KAKAO._SourceList.Add("ru", "러시아어");
        KAKAO._SourceList.Add("ms", "말레이시아어");
        KAKAO._SourceList.Add("bn", "벵골어");
        KAKAO._SourceList.Add("vi", "베트남어");
        KAKAO._SourceList.Add("es", "스페인어");
        KAKAO._SourceList.Add("ar", "아랍어");
        KAKAO._SourceList.Add("it", "이탈리아어");
        KAKAO._SourceList.Add("id", "인도네시아어");
        KAKAO._SourceList.Add("th", "태국어");
        KAKAO._SourceList.Add("tr", "터키어");
        KAKAO._SourceList.Add("pt", "포르투갈어");
        KAKAO._SourceList.Add("fr", "프랑스어");
        KAKAO._SourceList.Add("hi", "힌디어");
    }

    // Token: 0x06000169 RID: 361 RVA: 0x00002988 File Offset: 0x00000B88
    public static void Save()
    {
        DEFINE.SetIniValue("KAKAO", "SOURCE", KAKAO.SOURCE, "TranslationTool.ini");
        DEFINE.SetIniValue("KAKAO", "TARGET", KAKAO.TARGET, "TranslationTool.ini");
    }

    // Token: 0x0600016A RID: 362 RVA: 0x0001247C File Offset: 0x0001067C
    public static string Translation(string _Text)
    {
        string result;
        try
        {
            string s = string.Format("queryLanguage={1}&resultLanguage={2}&q={0}", _Text, KAKAO.SOURCE, KAKAO.TARGET);
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://translate.kakao.com/translator/translate.json");
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            httpWebRequest.Referer = "https://translate.kakao.com/";
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            httpWebRequest.ContentLength = (long)bytes.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            streamReader.Close();
            httpWebResponse.Close();
            JObject jobject = JObject.Parse(json);
            List<string> list = new List<string>();
            for (int i = 0; i < Enumerable.Count<JToken>(jobject["result"]["output"]); i++)
            {
                string text = "";
                for (int j = 0; j < Enumerable.Count<JToken>(jobject["result"]["output"][i]); j++)
                {
                    text += jobject["result"]["output"][i][j].ToString();
                }
                list.Add(text);
            }
            result = string.Join("\n", list);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            result = "";
        }
        return result;
    }

    // Token: 0x040001D0 RID: 464
    public static Dictionary<string, string> _SourceList = new Dictionary<string, string>();

    // Token: 0x040001D1 RID: 465
    public static string SOURCE = "auto";

    // Token: 0x040001D2 RID: 466
    public static string TARGET = "kr";
}
