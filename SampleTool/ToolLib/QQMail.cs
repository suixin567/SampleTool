using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ToolLib
{
    public class QQMail
    {

        public static bool SentMailHXD(string to, string body, string title, string path, string Fname)
        {
            // Debug.Print("配置是" + ConfigurationManager.AppSettings["FromEAcount"]);
            Debug.Print("发送邮件 -> "+ to);
            bool retrunBool = false;
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = true;
            var toEmailAcount = Fname;// ConfigurationManager.AppSettings["FromEAcount"];
            string strFromEmail = toEmailAcount;
            string strEmailPassword = "naqqulcxgefzdeba";
            try
            {
                mail.From = new MailAddress("" + Fname + "<" + strFromEmail + ">");
                mail.To.Add(new MailAddress(to));
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Priority = MailPriority.Normal;
                mail.Body = body;
                mail.Subject = title;
                smtp.Host = "smtp.qq.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(strFromEmail, strEmailPassword);
                smtp.Send(mail);   //同步发送  
                retrunBool = true;
            }
            catch
            {
                //   Debug.Print(err.ToString());
                retrunBool = false;
            }
            // smtp.SendAsync(mail, mail.To); //异步发送 （异步发送时页面上要加上Async="true" ）  
            return retrunBool;
        }

    }
}
