using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;

namespace IC.CrossCutting
{
    public static class EmailService
    {
        public static bool SendEmail(string adress, string subject, string code, string body, Bitmap qrCode)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.To.Add(adress);

                        var stream = new MemoryStream();
                        qrCode.Save(stream, ImageFormat.Jpeg);
                        stream.Position = 0;

                        message.Attachments.Add(new Attachment(stream, $"{code}.jpg"));

                        message.Subject = subject;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        message.Body = body;

                        client.Send(message);
                    }
                }
                return true;
            }
            catch
            {
                // Log
                return false;
            }
        }
    }
}