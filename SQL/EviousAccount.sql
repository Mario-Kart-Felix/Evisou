USE [master]
GO
/****** Object:  Database [EviousAccount]    Script Date: 2014/10/19 7:53:05 ******/
CREATE DATABASE [EviousAccount]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GMSAccount', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GMSAccount.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GMSAccount_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GMSAccount_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EviousAccount] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EviousAccount].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EviousAccount] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EviousAccount] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EviousAccount] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EviousAccount] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EviousAccount] SET ARITHABORT OFF 
GO
ALTER DATABASE [EviousAccount] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EviousAccount] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [EviousAccount] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EviousAccount] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EviousAccount] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EviousAccount] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EviousAccount] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EviousAccount] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EviousAccount] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EviousAccount] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EviousAccount] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EviousAccount] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EviousAccount] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EviousAccount] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EviousAccount] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EviousAccount] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EviousAccount] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EviousAccount] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EviousAccount] SET RECOVERY FULL 
GO
ALTER DATABASE [EviousAccount] SET  MULTI_USER 
GO
ALTER DATABASE [EviousAccount] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EviousAccount] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EviousAccount] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EviousAccount] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'EviousAccount', N'ON'
GO
USE [EviousAccount]
GO
/****** Object:  Table [dbo].[LoginInfo]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginInfo](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LoginToken] [uniqueidentifier] NOT NULL,
	[LastAccessTime] [datetime] NOT NULL,
	[UserID] [int] NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[BusinessPermissionString] [nvarchar](4000) NULL,
	[ClientIP] [nvarchar](90) NULL,
	[EnumLoginAccountType] [int] NOT NULL,
 CONSTRAINT [PK_LoginInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Info] [nvarchar](300) NULL,
	[BusinessPermissionString] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[LoginName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSessionData]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSessionData](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[LoginToken] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](500) NOT NULL,
	[Value] [image] NOT NULL,
 CONSTRAINT [PK_UserSessionData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VerifyCode]    Script Date: 2014/10/19 7:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VerifyCode](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VerifyText] [nvarchar](50) NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [VerifyCode_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[LoginInfo] ON 

INSERT [dbo].[LoginInfo] ([ID], [CreateTime], [LoginToken], [LastAccessTime], [UserID], [LoginName], [BusinessPermissionString], [ClientIP], [EnumLoginAccountType]) VALUES (2066, CAST(0x0000A3C601019042 AS DateTime), N'a4328ec9-fc04-4554-aee2-afdb2a318fd9', CAST(0x0000A3C6011154EB AS DateTime), 7, N'admin', N'101,102,201,202,301,302,303,304,401,402,403,501,502,503,504,505,506,507', N'Unknown', 0)
SET IDENTITY_INSERT [dbo].[LoginInfo] OFF
SET IDENTITY_INSERT [dbo].[Role] ON 

INSERT [dbo].[Role] ([ID], [Name], [CreateTime], [Info], [BusinessPermissionString]) VALUES (1, N'系统管理员', CAST(0x0000A27900EFEC38 AS DateTime), N'暂时无', N'101,102,201,202,301,302,303,304,401,402,403,501,502,503,504,505,506,507')
INSERT [dbo].[Role] ([ID], [Name], [CreateTime], [Info], [BusinessPermissionString]) VALUES (2, N'高级管理员', CAST(0x0000A27900F2C9BC AS DateTime), N'暂时无', N'101')
INSERT [dbo].[Role] ([ID], [Name], [CreateTime], [Info], [BusinessPermissionString]) VALUES (3, N'测试工程师', CAST(0x0000A27F01261DAA AS DateTime), N'测试项目的人员', NULL)
INSERT [dbo].[Role] ([ID], [Name], [CreateTime], [Info], [BusinessPermissionString]) VALUES (4, N'编辑', CAST(0x0000A30B0020EA29 AS DateTime), N'网站编辑', N'201,202')
SET IDENTITY_INSERT [dbo].[Role] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (1, N'guozili', N'C9-55-45-E4-DE-40-C0-BE-F5-3D-BF-AF-69-FB-19-DE', CAST(0x0000A27900EF6E1A AS DateTime), 1, N'guozili@163.com', N'13911153443')
INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (2, N'guozili2', N'96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12', CAST(0x0000A27F00EC1F62 AS DateTime), 0, N'guozili2@gmail.com', N'13856567825')
INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (4, N'guozili3', N'96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12', CAST(0x0000A27F01143C57 AS DateTime), 1, N'guozili3@163.com', N'13911854551')
INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (6, N'dakongyi', N'96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12', CAST(0x0000A28A01692CA5 AS DateTime), 1, NULL, NULL)
INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (7, N'admin', N'21-23-2F-29-7A-57-A5-A7-43-89-4A-0E-4A-80-1F-C3', CAST(0x0000A2A300FA4987 AS DateTime), 1, N'admin@admin.com', NULL)
INSERT [dbo].[User] ([ID], [LoginName], [Password], [CreateTime], [IsActive], [Email], [Mobile]) VALUES (8, N'test', N'96-E7-92-18-96-5E-B7-2C-92-A5-49-DD-5A-33-01-12', CAST(0x0000A30B0021ACD2 AS DateTime), 1, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (1, 1, 1, CAST(0x0000A27900EFFB08 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (2, 1, 2, CAST(0x0000A27900F320D7 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (7, 2, 1, CAST(0x0000A27F011BEE91 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (8, 1, 3, CAST(0x0000A27F0138BA90 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (9, 5, 3, CAST(0x0000A28000BB9D0F AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (10, 5, 2, CAST(0x0000A28000BBA58B AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (12, 4, 3, CAST(0x0000A280010C3CD3 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (13, 6, 3, CAST(0x0000A28A01692CD9 AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (14, 7, 1, CAST(0x0000A2A300FA49BA AS DateTime))
INSERT [dbo].[UserRole] ([ID], [UserID], [RoleID], [CreateTime]) VALUES (15, 8, 4, CAST(0x0000A30B0021AD03 AS DateTime))
SET IDENTITY_INSERT [dbo].[UserRole] OFF
ALTER TABLE [dbo].[LoginInfo] ADD  CONSTRAINT [DF_LoginInfo_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[LoginInfo] ADD  DEFAULT ((0)) FOR [EnumLoginAccountType]
GO
ALTER TABLE [dbo].[Role] ADD  CONSTRAINT [DF_Role_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRole] ADD  CONSTRAINT [DF_UserRole_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[UserSessionData] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[VerifyCode] ADD  CONSTRAINT [DF_VerifyCode_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
USE [master]
GO
ALTER DATABASE [EviousAccount] SET  READ_WRITE 
GO
