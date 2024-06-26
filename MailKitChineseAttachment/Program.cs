using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using System.Net.Mail;
using Volo.Abp.Emailing;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

var defaultSenderEmail = "x@x.com";
var defaultSenderEmailPwd = "xx";
var isBodyHtml = true;

var message = new MimeMessage();
message.From.Add(new MailboxAddress("System", defaultSenderEmail));
message.To.Add(new MailboxAddress("myesn", "myesn@foxmail.com"));
message.Subject = $"attachment test #{DateTime.UtcNow.Microsecond}";
var body = new TextPart(isBodyHtml ? "html" : "plain")
{
    Text = @"<strong style=""color:red"">hi~</strong>"
};

var filePath = @"C:\\1_All_2024-04-01_1_XXX你好世界（测试）.jpg";
// create an image attachment for the file located at path
var attachment = new MimePart("image", "jpg") 
{
    Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
    ContentTransferEncoding = ContentEncoding.Base64,
    FileName = Path.GetFileName(filePath)
};
// now create the multipart/mixed container to hold the message text and the
// image attachment
var multipart = new Multipart("mixed")
{
    body,
    attachment
};

// now set the multipart/mixed as the message body
message.Body = multipart;

using (var client = new SmtpClient())
{
    await client.ConnectAsync("smtp.qiye.aliyun.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);

    // Note: only needed if the SMTP server requires authentication
    await client.AuthenticateAsync(defaultSenderEmail, defaultSenderEmailPwd);

    var mailMessage = BuildMailMessage(
        from: defaultSenderEmail, to: "myesn@foxmail.com", subject: message.Subject,
        body: body.Text, isBodyHtml: isBodyHtml, additionalEmailSendingArgs: new AdditionalEmailSendingArgs
        {
            Attachments = new List<EmailAttachment>
            { new EmailAttachment { File = File.ReadAllBytes(filePath), Name = Path.GetFileName(filePath) } }
        });
    var message2 = MimeMessage.CreateFromMailMessage(mailMessage);
    message2.MessageId = MimeUtils.GenerateMessageId();

    //await client.SendAsync(message2);

    //await client.SendAsync(message);

    await client.DisconnectAsync(true);
}

MailMessage BuildMailMessage(string? from, string to, string? subject, string? body, bool isBodyHtml = true, AdditionalEmailSendingArgs? additionalEmailSendingArgs = null)
{
    //return base.BuildMailMessage(from, to, subject, body, isBodyHtml, additionalEmailSendingArgs);
    var message = from == null
        ? new MailMessage { To = { to }, Subject = subject, Body = body, IsBodyHtml = isBodyHtml }
        : new MailMessage(from, to, subject, body) { IsBodyHtml = isBodyHtml };

    if (additionalEmailSendingArgs != null)
    {
        if (additionalEmailSendingArgs.Attachments != null)
        {
            foreach (var attachment in additionalEmailSendingArgs.Attachments.Where(x => x.File != null))
            {
                var fileStream = new MemoryStream(attachment.File!);
                fileStream.Seek(0, SeekOrigin.Begin);
                message.Attachments.Add(new Attachment(fileStream, attachment.Name)
                {
                    ContentType = new System.Net.Mime.ContentType("image/jpg"),
                    TransferEncoding =  System.Net.Mime.TransferEncoding.Base64,
                });
            }
        }

        if (additionalEmailSendingArgs.CC != null)
        {
            foreach (var cc in additionalEmailSendingArgs.CC)
            {
                message.CC.Add(cc);
            }
        }
    }

    return message;
}
