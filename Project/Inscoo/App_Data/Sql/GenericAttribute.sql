USE [Inscoo]
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'AgeRange', N'40岁及以下', N'1', N'test', N'', CAST(N'2016-06-17 16:42:58.683' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'AgeRange', N'41-50岁', N'2', NULL, N'', CAST(N'2016-06-17 16:45:53.193' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'AgeRange', N'51-60岁', N'3', NULL, N'', CAST(N'2016-06-17 16:46:22.687' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'AgeRange', N'61-65岁', N'4', NULL, N'', CAST(N'2016-06-17 16:46:35.877' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'3-4人', N'1', NULL, N'', CAST(N'2016-06-17 16:47:18.380' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'5-10人', N'2', NULL, N'', CAST(N'2016-06-17 16:47:36.403' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'11-30人', N'3', NULL, N'', CAST(N'2016-06-17 16:47:56.670' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'31-50人', N'4', NULL, N'', CAST(N'2016-06-17 16:48:11.183' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'51-99人', N'5', NULL, N'', CAST(N'2016-06-17 16:48:25.887' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'StaffRange', N'100人及以上', N'6', NULL, N'', CAST(N'2016-06-17 16:48:44.477' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'InsuranceCompany', N'中国人民保险', N'PICC', NULL, N'', CAST(N'2016-06-17 16:50:04.733' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'InsuranceCompany', N'中国平安', N'PINGAN', NULL, N'', CAST(N'2016-06-17 16:50:21.807' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'InsuranceCompany', N'中国人寿', N'YAIC', NULL, N'', CAST(N'2016-06-17 16:50:35.110' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'方案确认', N'1', NULL, N'Admin', CAST(N'2016-06-26 00:06:43.800' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'信息填写', N'2', NULL, N'Admin', CAST(N'2016-06-26 00:06:54.797' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'上传文件', N'3', NULL, N'Admin', CAST(N'2016-06-26 00:07:05.230' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'付款通知', N'4', NULL, N'Admin', CAST(N'2016-06-26 00:07:23.623' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'待支付', N'5', NULL, N'Admin', CAST(N'2016-06-28 22:44:52.630' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'已完成', N'6', NULL, N'Admin', CAST(N'2016-06-28 22:45:02.970' AS DateTime), 0)
GO
INSERT [dbo].[GenericAttribute] ([KeyGroup], [Key], [Value], [Description], [Author], [CreateTime], [IsDeleted]) VALUES (N'orderState', N'审核未通过', N'7', NULL, N'Admin', CAST(N'2016-06-28 22:45:19.953' AS DateTime), 0)
GO