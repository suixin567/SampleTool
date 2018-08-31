using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using com.montnets.mwgate.common;
using com.montnets.mwgate.isms;
using com.montnets.mwgate.sms;

namespace ToolLib
{
    public class SendPhoneCode
    {

        public static int sendVerifyPhoneMsg(string phoneNum, string content)
        {
            if (phoneNum.Length!=11)
            {
                return -1;
            }
            //设置全局参数：路径和是否需要日志
            GlobalParams gp = new GlobalParams();
            gp.setRequestPath("/sms/v2/std/");
            gp.setNeedlog(1);
            ConfigManager.setGlobalParams(gp);
            //设置用户账号信息
            int result = setAccountInfo();
            if (result != 0)
            {
                return result;
            }
            // 是否保持长连接 false:否;true:是
            bool isKeepAlive = false;
            // 实例化短信处理对象
            ISMS sms = new SmsSendConn(isKeepAlive);
            result = singleSend(sms, "E104Y4", phoneNum, content);
            return result;
        }

        /**
	 * @description 设置用户账号信息
	 */
        public static int setAccountInfo()
        {
            // 设置用户账号信息
            // 用户账号
            String userid = "E104Y4";
            // 密码
            String password = "34qL38";
            // 发送优先级
            int priority = 1;
            // 主IP信息
            String masterIpAddress = "api01.monyun.cn:7901";
            // 备用IP1信息
            String ipAddress1 = "192.169.1.189:8086";
            // 备用IP2信息
            String ipAddress2 = null;
            // 备用IP3信息
            String ipAddress3 = null;
            // 返回值
            int result = -310007;
            try
            {
                // 设置用户账号信息
                result = ConfigManager.setAccountInfo(userid, password, priority,
                        masterIpAddress, ipAddress1, ipAddress2, ipAddress3);
                // 判断返回结果，0设置成功，否则失败
                if (result == 0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                    //MessageBox.Show("短信验证消息错误！设置用户账号信息失败，错误码：" + result);
                }
            }
            catch
            {
                // 异常处理
                return -2;
                // MessageBox.Show(e.Message);
            }
        }


        /**
	 * 
	 * @description 单条发送
	 * @param ISMS
	 *            短信处理对象,在这个方法中调用发送短信功能
	 * @param userid
	 *            用户账号
	 */
        static int singleSend(ISMS sms, string userid, string phoneNum, string content)
        {
            try
            {
                // 参数类
                com.montnets.mwgate.common.Message message = new com.montnets.mwgate.common.Message();
                // 设置用户账号 指定用户账号发送，需要填写用户账号，不指定用户账号发送，无需填写用户账号
                message.UserId = userid;
                // 设置手机号码 此处只能设置一个手机号码
                message.Mobile = phoneNum;
                // 设置内容
                message.Content = content;
                // 设置扩展号
                message.ExNo = "11";
                // 用户自定义流水编号
                message.CustId = "20160929194950100001";
                // 自定义扩展数据
                message.ExData = "abcdef";
                // 业务类型
                message.SvrType = "SMS001";
                // 返回的流水号
                string returnValue = string.Empty;
                // 返回值
                int result = -310099;
                // 发送短信
                result = sms.singleSend(message, out returnValue);
                // result为0:成功
                if (result == 0)
                {
                    // MessageBox.Show("单条发送提交成功！");
                    //  MessageBox.Show(returnValue);
                    return 0;
                }
                // result为非0：失败
                else
                {
                    return -3;
                    // MessageBox.Show("短信验证消息错误！单条发送提交失败,错误码：" + result);
                }
            }
            catch
            {
                // 异常处理
                // MessageBox.Show(e.Message);
                return -4;
            }
        }
    }
}
