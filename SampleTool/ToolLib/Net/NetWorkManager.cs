using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ToolLib;
using System.Drawing;

public class NetWorkManager
    {
        //单例
        private static NetWorkManager instance;
        public static NetWorkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetWorkManager();
                }
                return instance;
            }
        }
        public static int maxReceiveSize = 4000;
        private Socket socket;
        private string url = ConstDefine.SocketUrl;
        private int port = 10102;
        private byte[] dataCarriage = new byte[maxReceiveSize];//数据车厢
        private byte[] dataTrack = new byte[maxReceiveSize];//数据铁轨
        private int dataTrackIndex = 0;//铁轨当前长度
        private List<SocketModel> messages = new List<SocketModel>();
      //  static Queue<SocketModel> reSendQueue = new Queue<SocketModel>();//需要重新发送的消息列队
        public static int NET_STATE = 0;    //网络状态
        private bool isConnected;
        public bool IsConnected {//网络是否连接
            get {
                return isConnected;            
            }
            set {
                isConnected = value;
                if (isConnected==true) {//联网
                    if (onLineEvent!=null) {
                        onLineEvent();
                    }
                } else {//断网
                    if (offLineEvent != null)
                    {
                        offLineEvent();
                    }
                }
            }
        }      
        Thread heartPackage;

        public delegate void OnLineEvent();
        public OnLineEvent onLineEvent;

        public delegate void OffLineEvent();
        public OffLineEvent offLineEvent;


        

        public void Start()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(url, port);
                //异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
                socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, null);//最多只能接受maxReceiveSize大小数据
                IsConnected = true;
                //心跳
                heartPackage = new Thread(new ThreadStart(sendHeartPackage));
            heartPackage.Start();
            Debug.Print("连接服务器成功..."+ url+" "+port);
            }
            catch (Exception e)
            {
                Debug.Print("连接服务器失败:"+ url+ "  "+ port + e.Message);
            MessageBox.Show("连接服务器失败，请重试","叮叮鸟提示：");
            Environment.Exit(0);
            }
        }


    //停止网络
    public void Stop() {
        socket.Close();       
        heartPackage.Abort();
    }


    //sock异步线程读取完毕后调用此方法 形参ar会在异步线程结束后被自动传递过来
    private void ReceiveCallBack(IAsyncResult ar)
    {
        // Debug.Print("收到消息啦啦啦");
        int carriageLen = 0;//单个车厢的长度
        try
        {
            //单个车厢长度
            carriageLen = socket.EndReceive(ar);
            if (carriageLen >= maxReceiveSize)
            {
                Debug.Print("收到超长数据！！！");
                MessageBox.Show("收到超长数据！！！" + carriageLen);
                IsConnected = false;
                socket.Close();
                return;
            }
            //Debug.Log("车厢长度:" + carriageLen);
            //把车厢摆放到铁轨上
            Buffer.BlockCopy(dataCarriage, 0, dataTrack, dataTrackIndex, carriageLen);
            dataTrackIndex += carriageLen;
            getTrain();
        }
        catch (SocketException)
        {
            IsConnected = false;
            Debug.Print("服务器关闭了，我会知道");
            socket.Close();
            return;
        }
        //获得服务器消息后 再次继续监听
        if (IsConnected)
        {
            try
            {
                socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, dataCarriage);
            }
            catch (Exception err)
            {
                //    MessageBox.Show("继续监听出错：" + err.ToString());
                Debug.Print("继续监听出错：" + err.ToString());
            }
        }
    }

    //取出一个火车
    void getTrain()
    {
        if (dataTrack.Length < 4)
        {
            return;
        }
        //获取火车头所显示的长度，对列车长度进行比较，确定是否已经是一个完整的列车
        MemoryStream ms = new MemoryStream(dataTrack, 0, 4);
        ByteArray arr = new ByteArray(ms);
        int headLen = arr.ReadInt();
        //   Debug.Print("车头长度:" + headLen);
        //   Debug.Print("铁轨标识长度:" + dataTrackIndex);
        if (headLen == 0)
        {
            Debug.Print("被服务器踢下线，因为长时间没有握手。。。");
            IsConnected = false;
            return;
        }
        //如果已经至少存在一个列车，取出列车，进行逻辑处理
        if (headLen <= dataTrackIndex)
        {
            //Debug.Print("已经足够一个列车");
            //打印铁轨
            //string tempa = "";
            //for (int i = 0; i < 200; i++)
            //{
            //    tempa += dataTrack[i];
            //}
            //Debug.Print("整个铁轨数据：" + tempa);
            byte[] dataTrain = new byte[headLen - 4];
            Buffer.BlockCopy(dataTrack, 4, dataTrain, 0, headLen - 4);//从轨道中获得一个完整列车数据（不包含车头）
                                                                      //重置轨道
            if (dataTrackIndex == headLen)
            {//正好是一条列车
                dataTrackIndex = 0;
                //Debug.Log("正好是一个列车，所以清空");
            }
            else
            {//列车后面还连着后面的列车
                byte[] temp = new byte[dataTrackIndex - headLen];
                Buffer.BlockCopy(dataTrack, headLen, temp, 0, dataTrackIndex - headLen);//把剩余铁轨保存在临时变量中。
                Buffer.BlockCopy(temp, 0, dataTrack, 0, dataTrackIndex - headLen);
                dataTrackIndex = dataTrackIndex - headLen;
                //Debug.Print("递归数据");
                //Debug.LogError("数据长度："+ dataTrackIndex);
                //Debug.LogError("包头长度："+ headLen);
                getTrain();//递归调用
            }
            try
            {
                SocketModel model = Codec.Decode(dataTrain);
                if (model != null)
                {
                    messages.Add(model);
                }
                else
                {
                    Debug.Print("收到一个空的数据包！！！！！！！！！！！！！！！！！！！！！！");
                }
            }
            catch (Exception decodeerr)
            {
                Debug.Print("decodeerr!!!" + decodeerr.ToString());
            }         
            //   OnMessage(model);
        }
        else
        {
            //   Debug.Log("列车还不完全");
        }
    }

      



        public void sendMessage(int type, int area, int command, string message)
        {
       // Debug.Print("发送消息啊");
        if (IsConnected == false)
            {
                //reSendQueue.Enqueue(sm);//加入重新发送列表
                return;
            }
            SocketModel sm = new SocketModel(type, area, command, message);
            byte[] removeZero = Codec.Encode(sm);
        
            try
            {
                if (removeZero != null)
                {
               // Debug.Print("发送一条消息啊啊啊啊啊啊啊");
                foreach (var item in removeZero)
                {
                 //   Debug.Print(item.ToString());
                }
                    socket.Send(removeZero);
                }
            }
            catch
            {
                IsConnected = false;
                //reSendQueue.Enqueue(sm);//加入重新发送列表
            }
        }


        /// <summary>
        /// 重新发送上次失败的消息
        /// </summary>
        //public void reSend()
        //{
        //    //    StartCoroutine(reSendLogic());
        //}

        //IEnumerator reSendLogic() {
        //    while (true) {
        //        yield return new WaitForSeconds(Time.deltaTime);
        //        if (reSendQueue.Count > 0)
        //        {
        //            SocketModel sm = reSendQueue.Dequeue();                
        //            byte[] removeZero = Codec.Encode(sm);
        //            try
        //            {
        //                if (removeZero != null)
        //                {
        //                    socket.Send(removeZero);
        //                }
        //            }
        //             catch
        //             {
        //                    Debug.Print("消息重新发送失败...尝试再次重连中...");
        //                    reSendQueue.Enqueue(sm);//加入重新发送列表
        //                //    StartCoroutine(reConnectLogic());
        //              }
        //        }
        //        else
        //        {//没有需要重新发送的消息后就跳出循环
        //            break;
        //        }
        //    }
        //}
        void sendHeartPackage()
        {
            while (true)
            {
                if (IsConnected)
                {
            //    Debug.Print("每秒发送心跳包");
                sendMessage(Protocol.HEART_PACKAGE_CREQ, 0, 0, "");                   
                }
                else
                {//断线后重新连接                 
                    reConnect();
                }
                Thread.Sleep(1000);
            }
        }




        void reConnect()
        {
        Debug.Print("与服务器断开连接，尝试重连中...");
        //Log.FormLog.Instance.flyTipSafePost("与服务器断开连接，尝试重连中...");
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(url, port);
            //异步进行socket的读取,读取完毕之后进行回调                                   ，最后一个参数用于给回调函数传递子线程参数，这里不需要
            socket.BeginReceive(dataCarriage, 0, maxReceiveSize, SocketFlags.None, ReceiveCallBack, null);//最多只能接受maxReceiveSize大小数据
            IsConnected = true;
            Debug.Print("重连成功...");
            reLogin();//重新登陆
        }
        catch (Exception e)
        {
            Debug.Print("重连失败:" + e.Message);
        }
        }

    /// <summary>
    /// 重新登陆
    /// </summary>
    void reLogin()
    {
        //已经登陆需要重新登陆
        Debug.Print("发送重新登陆请求...");
        LoginDTO dto = new LoginDTO();
        dto.email = PlayerPrefs.GetString("account");// GameInfo.ACC_ID;
        dto.passWord = PlayerPrefs.GetString("passWord");// GameInfo.ACC_PSD;
        dto.uniq = MachineCode.GetMachineCodeString(); 
        string message = Coding<LoginDTO>.encode(dto);
        // Debug.Print(message);
        sendMessage(Protocol.LOGIN, 0, LoginProtocol.RELOGIN_CREQ, message);
    }

    public List<SocketModel> getList()
    {
        if (messages.Count > 0)
        {
            List<SocketModel> tempList = new List<SocketModel>(messages.ToArray());
            messages.Clear();
            return tempList;
        }
        else
        {
            return null;
        }
    }

            //消息 处理
        //public void OnMessage(SocketModel model)
        //{
        //    switch (model.Type)
        //    {
        //        case Protocol.LOGIN:                 
        //        //    Manager.Instance.formLogin.OnMessage(model);
        //     //       Login.LoginMgr.Instance.formLogin.OnMessage(model);
        //            break;
        //        case Protocol.MESSAGE://消息相关
        //      //      Manager.Instance.msgMgr.onMessage(model);
        //            break;              
        //        default:
        //            Debug.Print("网络协议类型错误：" + model.Type, 5);
        //            break;
        //    }
        //}
    }