# 介绍

测试使用 [MailKit](https://github.com/jstedfast/MailKit) 发送邮件到 QQ 邮箱，并附带了一个包含中文字符的附件，在 QQ 邮箱中附件名称显示乱码的问题。

目前已解决乱码问题，使用 `MimeMessage.CreateFromMailMessage` 生成的 `MimeMessage` 发送后会乱码，自己构建该对象则不会乱码，可能是我传递的参数缺失或错误，但因时间问题，不再详细查看源码和本代码的区别。
