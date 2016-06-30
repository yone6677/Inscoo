USE [Inscoo]
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'首页', 0, N'homeController', N'index', N'Home/Index', 0, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:46:18.403' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'保险酷', 0, null, NULL, N'/', 0, 1, NULL, 'fa fa-users fa-lg', 1, N'', CAST(N'2016-06-22 19:46:56.393' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'推荐产品', 1, N'insuranceController', N'index', N'Insurance/Index', 2, 1, NULL, 'fa fa-shopping-bag fa-lg', 1, N'', CAST(N'2016-06-22 19:47:25.927' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'自选产品', 1, N'insuranceController', N'customizeproduct', N'Insurance/CustomizeProduct', 2, 1, NULL, NULL, 2, N'', CAST(N'2016-06-22 19:47:51.497' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'推荐产品列表', 2, N'insuranceController', N'mixproduct', N'Insurance/MixProduct', 3, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:48:32.510' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'产品列表', 2, N'insuranceController', N'productlist', N'Insurance/ProductList', 4, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:49:22.047' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'购买区域', 2, N'insuranceController', N'cart', N'Insurance/Cart', 4, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:49:51.700' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'产品价格', 2, N'insuranceController', N'getproductprice', N'Insurance/GetProductPrice', 4, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:50:08.843' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'健康酷', 0, NULL, NULL, N'/', 0, 1, NULL, 'fa fa-medkit fa-lg', 2, N'', CAST(N'2016-06-22 19:50:39.533' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'理赔中心', 0, NULL, NULL, N'/', 0, 1, NULL, 'fa fa-rmb fa-lg', 3, N'', CAST(N'2016-06-22 19:50:50.217' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'产品管理', 0, NULL, NULL, N'/', 0, 0, NULL, 'fa fa-shopping-basket fa-lg', 4, N'', CAST(N'2016-06-22 19:51:01.613' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单管理', 0, NULL, NULL, N'/', 0, 1, NULL, 'fa fa-shopping-basket fa-lg', 5, N'', CAST(N'2016-06-22 19:51:30.087' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'账户管理', 0, N'userController', N'index', N'user/index', 0, 1, NULL, 'fa fa-user fa-lg', 7, N'', CAST(N'2016-06-22 19:52:02.697' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'企业信息', 0, 'company', 'listindex', N'company/listindex', 0, 1, NULL, 'fa fa-info-circle fa-lg', 8, N'', CAST(N'2016-06-22 19:52:14.180' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'财务管理', 0, NULL, NULL, N'/', 0, 0, NULL, 'fa fa-line-chart fa-lg', 9, N'', CAST(N'2016-06-22 19:52:23.157' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'设置', 0, NULL, NULL, N'/', 0, 1, NULL, 'fa fa-cogs fa-lg', 10, N'', CAST(N'2016-06-22 19:52:35.103' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'退出', 0, N'accountController', N'signout', N'account/signout', 0, 1, NULL, 'fa fa-power-off fa-lg', 11, N'', CAST(N'2016-06-22 19:52:44.950' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'角色管理', 1, N'roleController', N'index', N'role/index', 16, 1, NULL, NULL, 1, N'', CAST(N'2016-06-22 19:53:02.230' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'系统功能', 1, N'navController', N'index', N'Nav/index', 16, 1, NULL, NULL, 2, N'', CAST(N'2016-06-22 19:53:22.127' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'权限管理', 1, N'permissionController', N'ManagerSearch', N'permission/ManagerSearch', 16, 1, NULL, NULL, 3, N'', CAST(N'2016-06-22 19:53:47.413' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'通用属性', 1, N'genericattributeController', N'index', N'GenericAttribute/index', 16, 1, NULL, NULL, 4, N'', CAST(N'2016-06-22 19:54:07.850' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'列表', 2, N'roleController', N'list', N'role/list', 18, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:54:50.630' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'详细', 2, N'roleController', N'details', N'role/Details', 18, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:55:01.527' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'新增', 2, N'roleController', N'create', N'role/Create', 18, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:55:10.743' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'修改', 2, N'roleController', N'edit', N'role/Edit', 18, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:55:20.677' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'删除', 2, N'roleController', N'delete', N'role/Delete', 18, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:55:36.663' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'列表', 2, N'navController', N'list', N'nav/list', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:55:56.887' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'新增', 2, N'navController', N'create', N'nav/Create', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:56:06.303' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'详细', 2, N'navController', N'details', N'nav/Details', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:56:14.343' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'修改', 2, N'navController', N'edit', N'nav/Edit', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:56:27.120' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'删除', 2, N'navController', N'delete', N'nav/Delete', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:56:35.550' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'删除确认', 2, N'navController', N'deleteconfirmed', N'nav/DeleteConfirmed', 19, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:56:52.070' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'列表', 2, N'permissionController', N'list', N'permission/list', 20, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:57:15.320' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'保存', 2, N'permissionController', N'save', N'permission/save', 20, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:57:24.207' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'列表', 2, N'genericattributeController', N'list', N'GenericAttribute/list', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:57:41.647' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'新增', 2, N'genericattributeController', N'create', N'GenericAttribute/Create', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:57:49.553' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'修改', 2, N'genericattributeController', N'edit', N'GenericAttribute/Edit', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:57:58.967' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'删除', 2, N'genericattributeController', N'delete', N'GenericAttribute/Delete', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:58:07.507' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'详细', 2, N'genericattribute', N'details', N'GenericAttribute/Details', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:58:15.917' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'删除确认', 2, N'genericattributeController', N'deleteconfirmed', N'GenericAttribute/DeleteConfirmed', 21, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 19:58:29.117' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'导航栏', 1, N'homeController', N'menu', N'home/Menu', 1, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 20:00:44.020' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'头像', 1, N'homeController', N'portrait', N'home/Portrait', 1, 0, NULL, NULL, 0, N'', CAST(N'2016-06-22 20:01:04.133' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'方案确认', 1, N'orderController', N'buy', N'order/Buy', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-22 20:08:13.177' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'列表', 1, N'userController', N'list', N'user/list', 13, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-22 20:31:13.240' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'新增', 1, N'userController', N'create', N'user/Create', 13, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-22 20:31:44.007' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'分配角色', 1, N'userController', N'forrole', N'user/ForRole', 13, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-22 20:32:45.180' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'确认订单', 1, N'orderController', N'confirm', N'order/confirm', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-22 23:14:35.447' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'信息录入', 1, N'orderController', N'entryinfo', N'order/EntryInfo', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-23 15:38:08.263' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'上传员工资料', 1, N'orderController', N'uploademp', N'order/UploadEmp', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-23 15:38:23.467' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'下载模板', 1, N'orderController', N'downloadempinfo', N'order/DownloadEmpInfo', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-23 15:39:24.237' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'第三步上传文件', 1, N'orderController', N'uploadfile', N'order/UploadFile', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-23 17:26:37.133' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'上传营业执照(加盖公章)', 2, N'orderController', N'uploadbusinesslicensepdf', N'order/uploadbusinesslicensepdf', 51, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-24 21:42:38.817' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'上传人员信息(加盖公章)', 2, N'orderController', N'uploadempinfopdf', N'order/UploadEmpInfoPdf', 51, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-24 21:43:48.260' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'上传投保单信息(加盖公章)', 2, N'orderController', N'uploadpolicypdfseal', N'order/UploadPolicyPdfSeal', 51, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-24 21:44:11.633' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'第四步确认付款', 1, N'orderController', N'confirmpayment', N'order/ConfirmPayment', 12, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-25 15:17:24.297' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单列表', 1, N'orderController', N'index', N'order/index', 12, 1, NULL, NULL, 0, N'Admin', CAST(N'2016-06-25 23:36:07.060' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单详细', 2, N'orderController', N'details', N'order/details', 56, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-27 20:41:37.743' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单详细-人员列表', 2, N'orderController', N'detailsemp', N'order/DetailsEmp', 56, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-27 22:14:12.940' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单审核', 2, N'order', N'auditorder', N'order/AuditOrder', 56, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-27 22:14:12.940' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'未付订单', 1, N'order', N'ordertopay', N'order/ordertopay', 12, 1, NULL, NULL, 1, N'Admin', CAST(N'2016-06-29 19:26:00.800' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单列表', 2, N'ordercontroller', N'ordertopaylist', N'orderController/OrderToPayList', 60, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-29 19:26:56.367' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'已付订单', 1, N'order', N'completedorder', N'order/CompletedOrder', 12, 1, NULL, NULL, 2, N'Admin', CAST(N'2016-06-29 19:33:38.787' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'订单列表', 2, N'order', N'completedorderlist', N'order/CompletedOrderList', 62, 0, NULL, NULL, 0, N'Admin', CAST(N'2016-06-29 19:35:16.067' AS DateTime), 0)
GO
INSERT [dbo].[Navigation] ([name], [level], [controller], [action], [url], [pId], [isShow], [memo], [htmlAtt], [sequence], [Author], [CreateTime], [IsDeleted]) VALUES (N'修改密码', 2, N'UserController', N'ChangePassword', N'User/ChangePassword', 16, 1, NULL, NULL, 0, N'Admin', CAST(N'2016-06-29 19:35:16.067' AS DateTime), 0)
GO