using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShootRate.Service
{
    internal class MailService
    {
        internal async Task Send(string title, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.163.com");

                mail.From = new MailAddress("chenyujun536@163.com");
                mail.To.Add(new MailAddress("5398497@qq.com"));
                mail.To.Add(new MailAddress("f_steel@163.com"));
                mail.Subject = title;
                mail.Body = content;

                SmtpServer.Port = 25;
                SmtpServer.Host = "smtp.163.com";
                SmtpServer.EnableSsl = false;
                SmtpServer.UseDefaultCredentials = false;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("chenyujun536@163.com", "AEJBUZMTIOOCSYWC");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("chenyujun536@163.com", ".5Walnut");
                //SmtpServer.Credentials = new System.Net.NetworkCredential("chenyujun536", ".5Walnut");
                SmtpServer.Credentials = new System.Net.NetworkCredential("chenyujun536", "AEJBUZMTIOOCSYWC");

                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                App.LogService.LogError($"Send email {title} failed. Error {ex}");
            }
            
        }
    }
}
