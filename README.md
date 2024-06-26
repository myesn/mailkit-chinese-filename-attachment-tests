# 介绍

测试使用 [MailKit](https://github.com/jstedfast/MailKit) 发送邮件到 QQ 邮箱，并附带了一个包含中文字符的附件，在 QQ 邮箱中附件名称显示乱码的问题。

目前已解决乱码问题，使用 `MimeMessage.CreateFromMailMessage` 生成的 `MimeMessage` 发送后会乱码，自己构建该对象则不会乱码，可能是我传递的参数缺失或错误，但因时间问题，不再详细查看源码和本代码的区别。

示例中 `Smtp` 配置的 `Host`、`Port` 是阿里云企业邮箱的，并启用了 `SSL`，如果程序在阿里云服务器上运行，必须要启用 `SSL`，因为 `25` 端口因政策原因被官方封禁，无法连接。
