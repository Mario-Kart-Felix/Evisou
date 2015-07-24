USE [EviousOA]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 2015/7/24 22:04:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Desc] [nvarchar](300) NULL,
	[ParentId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 2015/7/24 22:05:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CoverPicture] [nvarchar](300) NULL,
	[Gender] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[BirthDate] [datetime] NULL,
	[Tel] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](200) NULL,
	[BranchId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Branch] ON 

INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (1, N'总经办', NULL, 0, CAST(0x0000A29A017DB622 AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (2, N'测试部', NULL, 6, CAST(0x0000A29A017E06C6 AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (3, N'研发部', NULL, 6, CAST(0x0000A29B017CF018 AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (4, N'前端组', NULL, 3, CAST(0x0000A29B017D22FE AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (5, N'后端组', NULL, 3, CAST(0x0000A29B017DD85E AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (6, N'开发中心', NULL, 1, CAST(0x0000A29B017DFAA9 AS DateTime))
INSERT [dbo].[Branch] ([ID], [Name], [Desc], [ParentId], [CreateTime]) VALUES (7, N'人力资源部', NULL, 1, CAST(0x0000A29B017E143F AS DateTime))
SET IDENTITY_INSERT [dbo].[Branch] OFF
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (1, N'程一冰', NULL, 2, 0, CAST(0x000074E000000000 AS DateTime), N'13876540999', NULL, NULL, 5, CAST(0x0000A29C00DA08B8 AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (3, N'欧景致', NULL, 1, 0, CAST(0x0000709800000000 AS DateTime), N'13876544531', NULL, NULL, 4, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (4, N'李敏', NULL, 1, 0, CAST(0x000077BA00000000 AS DateTime), N'13811329822', NULL, NULL, 0, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (5, N'宋慈', NULL, 1, 0, CAST(0x000074E000000000 AS DateTime), N'17627138777', NULL, NULL, 0, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (6, N'郑然', NULL, 2, 0, CAST(0x000074E000000000 AS DateTime), N'18978634571', NULL, NULL, 7, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (7, N'张京微', NULL, 1, 0, CAST(0x00007A9500000000 AS DateTime), N'13456788765', NULL, NULL, 0, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (8, N'黎西', NULL, 2, 0, CAST(0x0000792800000000 AS DateTime), N'13487673000', NULL, NULL, 0, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (9, N'李进', NULL, 1, 0, CAST(0x0000737300000000 AS DateTime), N'14534567890', NULL, NULL, 1, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (10, N'张一易', NULL, 1, 2, CAST(0x000077BA00000000 AS DateTime), N'13987654356', NULL, NULL, 3, CAST(0x0000A29C00DB1D7B AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (11, N'名人', NULL, 1, 1, CAST(0x0000A31600000000 AS DateTime), N'18927705761', N'happymama0813@gmail.com', N'NANHAI', 0, CAST(0x0000A3160037A606 AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (12, N'Test User', NULL, 1, 0, CAST(0x0000A31100000000 AS DateTime), N'02122102106', N'vson.h@gmail.com', N'New York', 0, CAST(0x0000A3160037B765 AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (13, N'Anya Lee', NULL, 1, 0, CAST(0x0000A30F00000000 AS DateTime), N'05786227560', N'anya.lee1215@hotmail.com', N'Room 602， 20BLG， Guihua Garden, GuiC', 0, CAST(0x0000A3160037D4AD AS DateTime))
INSERT [dbo].[Staff] ([ID], [Name], [CoverPicture], [Gender], [Position], [BirthDate], [Tel], [Email], [Address], [BranchId], [CreateTime]) VALUES (14, N'dsada', NULL, 1, 1, NULL, N'18927705761', N'happymama0813@gmail.com', N'NANHAI', 0, CAST(0x0000A3160037EA11 AS DateTime))
SET IDENTITY_INSERT [dbo].[Staff] OFF
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF_Branch_ParentId]  DEFAULT ((0)) FOR [ParentId]
GO
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF_Branch_CreateDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_Hits]  DEFAULT ((0)) FOR [Gender]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_IsActive]  DEFAULT ((0)) FOR [Position]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_Diggs]  DEFAULT ((0)) FOR [Tel]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_CreateDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
