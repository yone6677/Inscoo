USE [Inscoo]
GO
INSERT [dbo].[ProductMix] ([Name], [Code], [Description], [Price], [Address], [StaffRange], [AgeRange], [Author], [CreateTime], [IsDeleted]) VALUES (N'保酷一号', N'OR00000667', N'保酷一号', CAST(80.00 AS Decimal(18, 2)), N'无', N'100人以上', N'16-60周岁，平均年龄40岁以内', N'admin', CAST(N'2016-06-18 11:52:04.747' AS DateTime), 0)
GO
INSERT [dbo].[ProductMix] ([Name], [Code], [Description], [Price], [Address], [StaffRange], [AgeRange], [Author], [CreateTime], [IsDeleted]) VALUES (N'保酷二号', N'OR00000668', N'保酷二号', CAST(100.00 AS Decimal(18, 2)), N'无', N'51-100', N'16-60周岁，平均年龄40岁以内', N'admin', CAST(N'2016-06-18 11:53:32.750' AS DateTime), 0)
GO
INSERT [dbo].[ProductMix] ([Name], [Code], [Description], [Price], [Address], [StaffRange], [AgeRange], [Author], [CreateTime], [IsDeleted]) VALUES (N'保酷三号', N'OR00000669', N'保酷三号', CAST(200.00 AS Decimal(18, 2)), N'无', N'31-50', N'16-60周岁，平均年龄40岁以内', N'admin', CAST(N'2016-06-18 11:53:32.753' AS DateTime), 0)
GO
INSERT [dbo].[ProductMix] ([Name], [Code], [Description], [Price], [Address], [StaffRange], [AgeRange], [Author], [CreateTime], [IsDeleted]) VALUES (N'保酷四号', N'OR00000670', N'保酷四号', CAST(400.00 AS Decimal(18, 2)), N'无', N'5-10', N'16-60周岁，平均年龄40岁以内', N'admin', CAST(N'2016-06-18 11:53:32.753' AS DateTime), 0)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (1, N'死亡伤残责任', CAST(100.00 AS Decimal(18, 2)), N'5万', N'无', N'admin', CAST(N'2016-06-18 12:30:26.150' AS DateTime), 0, 1)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (1, N'医疗责任', CAST(100.00 AS Decimal(18, 2)), N'2万', N'无', N'admin', CAST(N'2016-06-18 12:30:26.150' AS DateTime), 0, 24)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (1, N'重大疾病保险', CAST(35.00 AS Decimal(18, 2)), N'5万', N'无', N'admin', CAST(N'2016-06-18 12:30:26.150' AS DateTime), 0, 41)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (2, N'误工费', CAST(2.50 AS Decimal(18, 2)), N'50元/天', N'无', N'admin', CAST(N'2016-06-18 12:31:00.773' AS DateTime), 0, 25)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (2, N'医疗责任', CAST(100.00 AS Decimal(18, 2)), N'2万', N'无', N'admin', CAST(N'2016-06-18 12:31:00.773' AS DateTime), 0, 24)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (2, N'重大疾病保险', CAST(40.00 AS Decimal(18, 2)), N'5万', N'无', N'admin', CAST(N'2016-06-18 12:31:00.773' AS DateTime), 0, 41)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (3, N'误工费', CAST(2.50 AS Decimal(18, 2)), N'50元/天', N'无', N'admin', CAST(N'2016-06-18 12:31:12.217' AS DateTime), 0, 25)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (3, N'重大疾病保险', CAST(45.00 AS Decimal(18, 2)), N'5万', N'无', N'admin', CAST(N'2016-06-18 12:31:12.217' AS DateTime), 0, 41)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (4, N'医疗责任', CAST(100.00 AS Decimal(18, 2)), N'2万', N'无', N'admin', CAST(N'2016-06-18 12:31:22.107' AS DateTime), 0, 24)
GO
INSERT [dbo].[ProductMixItem] ([mid], [SafefuardName], [OriginalPrice], [CoverageSum], [PayoutRatio], [Author], [CreateTime], [IsDeleted], [product_Id]) VALUES (4, N'重大疾病保险', CAST(45.00 AS Decimal(18, 2)), N'5万', N'无', N'admin', CAST(N'2016-06-18 12:31:22.107' AS DateTime), 0, 41)
GO