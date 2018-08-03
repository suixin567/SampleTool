using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using LitJson;

    class PlayerPrefsIntModel
    {
        public string intKey;
        public int intValue;
    }

    class PlayerPrefsStringModel
    {
        public string stringKey;
        public string stringValue;
    }

   public class PlayerPrefs
    {
        public static string PlayerPrefsIntPath = Environment.CurrentDirectory + @"\PlayerPrefsInt.json";
        public static string PlayerPrefsStringPath = Environment.CurrentDirectory + @"\PlayerPrefsString.json";

        public static List<string> playerPrefsIntList = new List<string>();
        public static List<string> playerPrefsStringList = new List<string>();

        static bool isInited = false;
        public static void Init()
        {
        if (isInited == false)
        {
            isInited = true;
        }
        else {
            return;
        }
       

         //   Debug.Print(PlayerPrefsIntPath);
        if (!File.Exists(PlayerPrefsIntPath))
            {      
                FileStream f = File.Create(PlayerPrefsIntPath);//创建该文件
                f.Close();
            }
         //   Debug.Print(PlayerPrefsStringPath);
            if (!File.Exists(PlayerPrefsStringPath))
            {
                FileStream f = File.Create(PlayerPrefsStringPath);//创建该文件
                f.Close();
            }


            FileStream _file = new FileStream(PlayerPrefsIntPath, FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader sr = new StreamReader(_file, System.Text.Encoding.GetEncoding("utf-8")))//先读到文件
            {
                string content = sr.ReadToEnd().ToString(); //    Debug.Print("读到的全文"+ content);
                if (content != "")//文件不为空检查更新
                {
                    string[] datas = content.Split('\r');
                    for (int i = 0; i < datas.Length - 1; i++)//每一条数据
                    {
                        if (isRowEmpty(datas[i]) == false)
                        {
                            playerPrefsIntList.Add(datas[i]);
                        }
                    }

                }
            }
            //foreach (var item in playerPrefsList)
            //{
            //    Debug.Print("全部PlayerPrefs数据：" + item);
            //}
            FileStream _file2 = new FileStream(PlayerPrefsStringPath, FileMode.OpenOrCreate, FileAccess.Read);
            using (StreamReader sr = new StreamReader(_file2, System.Text.Encoding.GetEncoding("utf-8")))//先读到文件
            {
                string content = sr.ReadToEnd().ToString(); //    Debug.Print("读到的全文"+ content);
                if (content != "")//文件不为空检查更新
                {
                    string[] datas = content.Split('\r');
                    for (int i = 0; i < datas.Length - 1; i++)//每一条数据
                    {
                        if (isRowEmpty(datas[i]) == false)
                        {
                            playerPrefsStringList.Add(datas[i]);
                        }
                    }

                }
            }
        }


        public static void SetInt(string intKey, int intValue)
        {
            if (intKey == string.Empty)
            {
                throw new Exception("intKey不能为空");
            }         
            bool isAllreadyExist = false;
            for (int i = 0; i < playerPrefsIntList.Count; i++)
            {
          //      Debug.Print("每一条PlayerPrefs数据：" + playerPrefsList[i]);
                PlayerPrefsIntModel model = new PlayerPrefsIntModel();
            try
            {
                model = Coding<PlayerPrefsIntModel>.decode(playerPrefsIntList[i]);
            }
            catch (Exception e)
            {
                Debug.Print("SetInt解析失败"+ e.ToString());
                return;               
            }
               
                if (model.intKey == intKey)//找到了数据
                {
                    isAllreadyExist = true;
                    PlayerPrefsIntModel intModel = new PlayerPrefsIntModel();
                    intModel.intKey = intKey;
                    intModel.intValue = intValue;
                    string updateItem = Coding<PlayerPrefsIntModel>.encode(intModel);
                    playerPrefsIntList[i] = updateItem;
                    break;
                }
            }
            if (isAllreadyExist == false) {
                PlayerPrefsIntModel intModel = new PlayerPrefsIntModel();
                intModel.intKey = intKey;
                intModel.intValue = intValue;
                string updateItem = Coding<PlayerPrefsIntModel>.encode(intModel);
                playerPrefsIntList.Add(updateItem);
            }
            //写入新文件
            using (StreamWriter file = new System.IO.StreamWriter(PlayerPrefsIntPath, false))
            {
                string newJsonFile = "";
                foreach (var item in playerPrefsIntList)
                {
                    newJsonFile += item + "\r";
                }
                file.WriteLine(newJsonFile);// 直接追加文件末尾，换行 
            }
        }

        public static int GetInt(string intKey)
        {
            if (intKey == string.Empty)
            {
                throw new Exception("intKey不能为空");
            }
            foreach (var item in playerPrefsIntList)
            {
                PlayerPrefsIntModel model = new PlayerPrefsIntModel();
            try
            {
                model = Coding<PlayerPrefsIntModel>.decode(item);
            }
            catch (Exception err)
            {
                Debug.Print("GetInt解析失败" + err.ToString());
                return 0;
            }
              
                if (model.intKey == intKey)//找到了数据
                {
                    return model.intValue;
                }
            }
            return 0;
        }
















        public static void SetString(string stringKey, string stringValue)
        {
            if (stringKey == string.Empty)
            {
                throw new Exception("stringKey不能为空");
            }
            bool isAllreadyExist = false;
            for (int i = 0; i < playerPrefsStringList.Count; i++)
            {
                //      Debug.Print("每一条PlayerPrefs数据：" + playerPrefsList[i]);
                PlayerPrefsStringModel model = new PlayerPrefsStringModel();
                try
                {
                    model = Coding<PlayerPrefsStringModel>.decode(playerPrefsStringList[i]);
                }
                catch (Exception err)
                {
                    Debug.Print("SetString解析失败" + err.ToString());
                return;
                }
               
                if (model.stringKey == stringKey)//找到了数据
                {
                    isAllreadyExist = true;
                    PlayerPrefsStringModel stringModel = new PlayerPrefsStringModel();
                    stringModel.stringKey = stringKey;
                    stringModel.stringValue = stringValue;
                    string updateItem = Coding<PlayerPrefsStringModel>.encode(stringModel);
                    playerPrefsStringList[i] = updateItem;
                    break;
                }
            }
            if (isAllreadyExist == false)
            {
                PlayerPrefsStringModel stringModel = new PlayerPrefsStringModel();
                stringModel.stringKey = stringKey;
                stringModel.stringValue = stringValue;
                string updateItem = Coding<PlayerPrefsStringModel>.encode(stringModel);
                playerPrefsStringList.Add(updateItem);
            }
            //写入新文件
            using (StreamWriter file = new System.IO.StreamWriter(PlayerPrefsStringPath, false))
            {
                string newJsonFile = "";
                foreach (var item in playerPrefsStringList)
                {
                    newJsonFile += item + "\r";
                }
                file.WriteLine(newJsonFile);// 直接追加文件末尾，换行 
            }
        }

        public static string GetString(string stringKey)
        {
            if (stringKey == string.Empty)
            {
                throw new Exception("intKey不能为空");
            }
            foreach (var item in playerPrefsStringList)
            {
                PlayerPrefsStringModel model = new PlayerPrefsStringModel();
            try
            {
                model = Coding<PlayerPrefsStringModel>.decode(item);
            }
            catch (Exception err)
            {
                Debug.Print("GetString 解析失败" + err.ToString());
                return "";
            }
              
                if (model.stringKey == stringKey)//找到了数据
                {
                    return model.stringValue;
                }
            }
            return "";
        }



        static bool isRowEmpty(string value)
        {
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
            return false;
        }
    }

