using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace ToolLib
{
    public class HttpReqHelper
    {

        public static string request(string url)
        {
            string responseString = "";
            try
            {
                //  Debug.Print("发出是：" + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "textml;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //   Debug.Print("收到的是" + responseString);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
            }
            return responseString;
        }



        public delegate void RequestInfoEvent(string callback);

        public static void requestSync(string url, RequestInfoEvent callBack)
        {
            string responseString = "";
            Func<string, string> reqInfo = requestThread;
            IAsyncResult iar = reqInfo.BeginInvoke(url, ar =>
            {
                responseString = reqInfo.EndInvoke(ar);
                if (callBack != null)
                {
                    callBack(responseString);
                }
            }, reqInfo);
        }

        static string requestThread(string url)
        {
            string responseString = "";
            try
            {
                //     Debug.Print("发出是：" + url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "textml;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //   Debug.Print("收到的是" + responseString);
            }
            catch (Exception e)
            {
                Debug.Print(e.ToString());
                return e.ToString();
            }
            //   Debug.Print("线程收到的数据" + responseString);
            return responseString;
        }







        ///// <summary>
        ///// 头像下载
        ///// </summary>
        ///// <param name="img"></param>
        //public delegate void RequestPicEvent(Image img);
        //public static void loadPicSync(string face, RequestPicEvent callBack)
        //{
        //    bool isFaceExit = false;
        //    foreach (var item in texturesList)
        //    {
        //        if (item.EndsWith(face))
        //        {
        //            isFaceExit = true;
        //            break;
        //        }
        //    }
        //    if (isFaceExit)
        //    {
        //        //   Debug.Print("图片存在，无需下载" + face);
        //        loadFaceFromLocal(face, callBack);
        //    }
        //    else
        //    {
        //        //  Debug.Print("图片不存在，网络下载"+ face);
        //        loadFaceFromNet(face, callBack);
        //    }
        //}


        //static void loadFaceFromLocal(string face, RequestPicEvent callBack)
        //{
        //    string url = ConstDefine.WinPicPath + face;
        //    Image tempImage;
        //    Func<string, RequestPicEvent, Image> reqPic = loadFaceFromLocalThread;
        //    IAsyncResult iar = reqPic.BeginInvoke(url, callBack, ar =>
        //     {
        //         tempImage = reqPic.EndInvoke(ar);
        //         if (callBack != null)
        //         {
        //             callBack(tempImage);
        //         }
        //     }, reqPic);
        //}

        //static Image loadFaceFromLocalThread(string url, RequestPicEvent callBack)
        //{
        //    FileStream fs = null;
        //    try
        //    {
        //        fs = File.OpenRead(url);
        //        int filelength = 0;
        //        filelength = (int)fs.Length; //获得文件长度 
        //        Byte[] image = new Byte[filelength]; //建立一个字节数组 
        //        fs.Read(image, 0, filelength); //按字节流读取 
        //        Image result = Image.FromStream(fs);
        //        fs.Close();
        //        return result;
        //    }
        //    catch (Exception err)
        //    {
        //        Debug.Print("loadFaceFromLocalThread下载图片失败,转为网络下载。" + url + err.ToString());
        //        int count = url.LastIndexOf("/");
        //        string face = url.Substring(count);
        //        loadFaceFromNet(face, callBack);
        //        return null;
        //    }
        //    finally
        //    {
        //        if (fs != null)
        //        {
        //            fs.Close();
        //        }
        //    }
        //}

        //static void loadFaceFromNet(string face, RequestPicEvent callBack)
        //{

        //    Image tempImage;
        //    Func<string, Image> reqPic = loadFaceFromNetThread;
        //    IAsyncResult iar = reqPic.BeginInvoke(face, ar =>
        //    {
        //        tempImage = reqPic.EndInvoke(ar);
        //        if (callBack != null)
        //        {
        //            callBack(tempImage);
        //        }
        //    }, reqPic);
        //}

        //static Image loadFaceFromNetThread(string face)
        //{
        //    string url = ConstDefine.WebUrl + "res/yzm/" + face;
        //    Debug.Print("下载验证码的地址是"+ url);
        //    Image image = null;
        //    Stream resStream = null;
        //    try
        //    {
        //        //    Debug.Print("发出请求图片是：" + url);
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "GET";
        //        request.ContentType = "application/octet-stream";
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        resStream = response.GetResponseStream();
        //        image = Image.FromStream(resStream);

        //        //本地缓存
        //        //image.Save(ConstDefine.WinPicPath + face);
        //        //if (texturesList.Contains(ConstDefine.WinPicPath + face) == false)
        //        //{
        //        //    texturesList.Add(ConstDefine.WinPicPath + face);
        //        //}
        //        return image;
        //    }
        //    catch (Exception err)
        //    {
        //        Debug.Print("requestPicThread下载图片失败" + err.ToString());
        //        return null;
        //    }
        //    finally
        //    {
        //        if (resStream != null)
        //        {
        //            resStream.Close();
        //        }
        //    }
        //}


        //文件下载
        public delegate void AssetLoadEvent(string err);
        public delegate void AssetLoadProgress(float progress);
        public static void downloadFile(string url, string path, AssetLoadEvent callback, AssetLoadProgress progress = null)
        {
            Debug.Print("下载Unity的请求" + url);
            int a = path.LastIndexOf('\\');
            string directory = path.Substring(0, a);
            if (File.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                response = request.GetResponse() as HttpWebResponse;
            }
            catch (Exception err)
            {
                Debug.Print("下载Unity文件失败" + err.ToString());
                if (callback != null) callback(err.ToString());
                return;
            }

            long totalBytes = response.ContentLength;
            Stream responseStream = response.GetResponseStream();
            Stream stream = new FileStream(path, FileMode.Create);
            long totalDownloadedByte = 0;
            try
            {
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    totalDownloadedByte = size + totalDownloadedByte;
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    if (progress != null)
                    {
                        progress((float)totalDownloadedByte / (float)totalBytes * 100);
                    }
                    //System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                    //  Thread.Sleep(10);
                }
                stream.Close();
                responseStream.Close();
                //下载成功
                if (callback != null) callback(null);
            }
            catch (Exception e)
            {
                Debug.Print("ToolLib下载文件出错:" + e);
                if (callback != null) callback(e.ToString());
            }
            finally
            {
                stream.Close();
                responseStream.Close();
            }

        }


        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            }
        }



        /// <summary>
        /// 图片缓存
        /// </summary>
        public static List<string> texturesList = new List<string>();//获取到的路径
                                                                     // public delegate void TextureLoadEvent(string err, Texture2D tex);
        //public static void Init()
        //{
        //    if (Directory.Exists(ConstDefine.WinPicPath) == false)//如果不存在就创建file文件夹
        //    {
        //        Directory.CreateDirectory(ConstDefine.WinPicPath);
        //    }
        //    //获取所有本地图片
        //    getAllTextures(ConstDefine.WinPicPath);
        //    //foreach (var item in texturesList)
        //    //{
        //    //    Debug.Print("已经有的图" + item);
        //    //}
        //}
        //获取所有本地图
        public static void getAllTextures(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            //   DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                texturesList.Add(f.FullName.Replace('\\', '/'));//添加文件的路径到列表
            }
        }

    }
}