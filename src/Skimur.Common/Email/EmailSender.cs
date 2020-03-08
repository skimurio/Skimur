using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Skimur.Common.Email
{
    public class EmailSender : IEmailSender
    {

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAccount">Email account to use</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="fromAddress">From address</param>
        /// <param name="fromName">From display name</param>
        /// <param name="toAddress">To address</param>
        /// <param name="toName">To display name</param>
        /// <param name="replyTo">ReplyTo address</param>
        /// <param name="replyToName">ReplyTo display name</param>
        /// <param name="bcc">BCC addresses list</param>
        /// <param name="cc">CC addresses list</param>
        /// <param name="attachmentFilePath">Attachment file path</param>
        /// <param name="attachmentFileName">Attachment file name. If specified, then this file name will be
        /// sent to a recepient. Otherwise, "AttachmentFilePath" name will be used.</param>
        public void SendEmail(EmailServerSettings emailAccount,
            string subject,
            string body,
            string fromAddress,
            string fromName,
            string toAddress,
            string toName,
            string replyTo = null,
            string replyToName = null,
            IEnumerable<string> bcc = null,
            IEnumerable<string> cc = null,
            string attachmentFilePath = null,
            string attachmentFileName = null)
        {
            fromAddress = fromAddress ?? emailAccount.FromEmail;
            fromName = fromName ?? emailAccount.FromName;

            var message = new MailMessage { From = new MailAddress(fromAddress, fromName) };
            message.To.Add(new MailAddress(toAddress, toName));

            // if we have a reply to address, add it to the message
            if (!string.IsNullOrEmpty(replyTo))
            {
                message.ReplyToList.Add(new MailAddress(replyTo, replyToName));
            }

            // if we have bcc addresses, add them to the message
            if (bcc != null)
            {
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                {
                    message.Bcc.Add(address.Trim());
                }
            }

            // if we have cc addreses, add them to the message
            if (cc != null)
            {
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                {
                    message.CC.Add(address.Trim());
                }
            }

            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            // if we have an attachment, add it to the message
            if (string.IsNullOrEmpty(attachmentFilePath) &&
                File.Exists(attachmentFilePath))
            {
                var attachment = new Attachment(attachmentFilePath);
                attachment.ContentDisposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                attachment.ContentDisposition.ModificationDate = File.GetLastWriteTime(attachmentFilePath);
                attachment.ContentDisposition.ReadDate = File.GetLastAccessTime(attachmentFilePath);

                if (!string.IsNullOrEmpty(attachmentFileName))
                {
                    attachment.Name = attachmentFileName;
                }

                message.Attachments.Add(attachment);
            }

            // create a new smtp client for sending the message
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSSL;
                smtpClient.Credentials = new NetworkCredential(emailAccount.UserName, emailAccount.Password);
                smtpClient.Send(message);
            }
        }
    }
}
