using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LBKJClient.utils
{
    class HttpClient
    {
        private static HttpWebResponse response;
        private static StreamReader streamReader;
        public static String getDeviceData(String deviceids,String ipport)
        {
            String responseContent="";
            //System.GC.Collect();
            //ServicePointManager.DefaultConnectionLimit = 200;
            String url = "http://"+ipport;
            byte[] byteArray = Encoding.UTF8.GetBytes(deviceids);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

            webRequest.Proxy = null;
            webRequest.Method = "post";
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = byteArray.Length;
            webRequest.KeepAlive = false;
            webRequest.Timeout = 5000;
            //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据
            webRequest.ServicePoint.Expect100Continue = false;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            Thread.Sleep(300);
            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                responseContent = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return responseContent = "";

            }
            finally
            {
                //streamReader.Close();
                //newStream.Close();
                if (response!=null) {
                    response.Close();
                    webRequest.Abort();
                }
            }
            
            return responseContent;
        }
    }
    

}
