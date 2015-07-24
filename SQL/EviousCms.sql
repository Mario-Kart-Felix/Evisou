USE [master]
GO
/****** Object:  Database [EviousCms]    Script Date: 2014/10/19 7:55:10 ******/
CREATE DATABASE [EviousCms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GMSCms', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GMSCms.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GMSCms_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\GMSCms_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [EviousCms] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EviousCms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EviousCms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EviousCms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EviousCms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EviousCms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EviousCms] SET ARITHABORT OFF 
GO
ALTER DATABASE [EviousCms] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [EviousCms] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [EviousCms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EviousCms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EviousCms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EviousCms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EviousCms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EviousCms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EviousCms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EviousCms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EviousCms] SET  DISABLE_BROKER 
GO
ALTER DATABASE [EviousCms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EviousCms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EviousCms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EviousCms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EviousCms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EviousCms] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EviousCms] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EviousCms] SET RECOVERY FULL 
GO
ALTER DATABASE [EviousCms] SET  MULTI_USER 
GO
ALTER DATABASE [EviousCms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EviousCms] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EviousCms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EviousCms] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'EviousCms', N'ON'
GO
USE [EviousCms]
GO
/****** Object:  Table [dbo].[Article]    Script Date: 2014/10/19 7:55:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Article](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[ChannelId] [int] NOT NULL,
	[CoverPicture] [nvarchar](300) NULL,
	[Hits] [int] NOT NULL,
	[Diggs] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArticleTag]    Script Date: 2014/10/19 7:55:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArticleTag](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ArticleId] [int] NOT NULL,
	[TagId] [int] NOT NULL,
 CONSTRAINT [PK_ArticleTag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Channel]    Script Date: 2014/10/19 7:55:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Channel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Desc] [nvarchar](300) NULL,
	[CoverPicture] [nvarchar](300) NULL,
	[Hits] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Channel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tag]    Script Date: 2014/10/19 7:55:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Hits] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Article] ON 

INSERT [dbo].[Article] ([ID], [Title], [Content], [ChannelId], [CoverPicture], [Hits], [Diggs], [IsActive], [UserId], [UserName], [CreateTime]) VALUES (16, N'吉林"最狠拆迁女市长"被双开 曾称有"尚方宝剑"', N'<p class="f_center"><img src="http://img6.cache.netease.com/cnews/2013/12/29/201312290432079cb70.jpg" alt="韩迎新资料图" /><br />韩迎新资料图</p><p>晨报讯 吉林省舒兰市原市委常委、副市长韩迎新昨天被证实开除党籍、公职，其涉嫌犯罪问题已移送司法机关依法处理。2011年，韩迎新曾因辖区内拆迁户到中央上访而受到舆论关注。</p><p>经查，韩迎新利用职务便利和职权影响，为他人牟取利益，收受他人贿赂数额较大。其行为已构成严重违纪。公开资料显示，韩迎新任副市长期间曾分管市监察局、市财政局、市审计局。并协助市长分管市政府办公室、市住房和城乡建设局、市城市管理局、市棚户区改造管理办公室、市政府采购中心等重要部门。</p><p>公开报道称，韩迎新曾说过“我不懂拆迁法，不按拆迁法办”、“我有尚方宝剑！你们随便告，我不怕”等言论，而被称为“史上最美最狠拆迁女市长”。2011年，韩迎新更是成为焦点人物。那一年，因门面房被无理强拆，舒兰市市民许桂芹到中央上访，并得到了时任总理温家宝的亲自接待。</p><!--EndFragment-->', 3, N'/Upload/cms/day_140413/20140413104855103.jpg', 0, 0, 1, 1, N'guozili', CAST(0x0000A2A301264AB1 AS DateTime))
INSERT [dbo].[Article] ([ID], [Title], [Content], [ChannelId], [CoverPicture], [Hits], [Diggs], [IsActive], [UserId], [UserName], [CreateTime]) VALUES (17, N'李克强在海南买特产所用10元纸币被便利店保存', N'<p>本报讯11日上午， 在考察了位于坡博路的海口市工商局后，中共中央政治局常委、国务院总理李克强随机到工商局对面的一家便利店走访，了解本地产品种类、销售等情况。看到货架上不少都是海南特产，总理赞许产品包装不错。临走前总理对女店员说，&ldquo;耽误你做生意了，来了就消费一下。&rdquo;总理说着便掏钱买了一盒椰子脆片和一盒椰奶酥卷，共计19元。</p>

<p>记者在回访中了解到，李克强总理花19元钱购买的两盒海南特产分别是春光牌&ldquo;原味椰子脆片&rdquo;和&ldquo;椰奶酥卷&rdquo;。这两盒海南特产被网友亲切地称为&ldquo;总理套餐&rdquo;。</p>

<p>据了解，便利店的&ldquo;总理套餐&rdquo;当天就被抢购一空，包括淘宝网在内的<a href="http://travel.ifeng.com/theme/shopping/list_0/0.shtml" target="_blank">购物</a>网站随后纷纷推出&ldquo;总理套餐&rdquo;。</p>

<p><strong>&ldquo;总理问我店里的生意好不好&rdquo;</strong></p>

<p>第一天到海口&ldquo;宜之佳&rdquo;便利店第五分店上班的昌江县石碌镇姑娘韩玉琼，做梦也没有想到11日会迎来一位特殊的顾客。他，就是李克强总理。</p>

<p>&ldquo;这两天我都有点懵，很多同学、朋友都打电话过来问我是不是见到了总理？&rdquo;25岁的韩玉琼告诉记者，&ldquo;当时我一个人在店里，然后看到走进来很多人，心里正纳闷呢。突然有人告诉我是李总理来了，我的心跳一下子就加快了。&rdquo;韩玉琼告诉记者，11日上午9时51分，李克强总理走进了便利店，直接走到了收银台前，主动和她握手，然后站在收银台前和她进行了一番简短的交谈。</p>

<p>&ldquo;总理一进门就和我握手，微笑着问我&lsquo;店里生意好不好&rsquo;。&rdquo;韩玉琼接受记者采访时笑着说自己&ldquo;反应迟钝&rdquo;，总理和她握手后，这位年轻的海南姑娘愣了几秒才反应过来：&ldquo;哦，原来是总理。&rdquo;</p>

<p>&ldquo;我当时觉得有些不可思议，心里激动得七上八下的。&rdquo;韩玉琼告诉记者，总理又问她&ldquo;这里是不是绝不销售伪劣产品？&rdquo;因为他们的店门口上方，贴着&ldquo;本店郑重承诺：绝不销售假冒伪劣商品&rdquo;的横幅。她很肯定地回答总理&ldquo;是&rdquo;。之后，总理又问她：&ldquo;你一个月的工资有多少？&rdquo;&ldquo;1800到2000元左右。&rdquo;韩玉琼回答说。</p>

<p>李克强总理的关心，让韩玉琼很是感动，&ldquo;你想想，总理日理万机，还关心着我们普通老百姓的生活情况，让我心里暖暖的。&rdquo;</p>

<p><strong>&ldquo;总理让我介绍一下海南特产&rdquo;</strong></p>

<p>&ldquo;随后，总理就直接走到了便利店内的海南特产区域，问我海南特产有哪些，让我介绍一下。当时总理说货架上摆放的特产包装都很精美，他喜欢海南的椰子产品。然后，总理就先后挑选了海南本地生产的一份椰子脆片和椰奶酥卷。&rdquo;</p>

<p>韩玉琼告诉记者，总理在便利店内的特产区域大概停留了3分钟左右，然后回到收银台前，让她结账。李克强总理拿了这两盒特产来到收银台前，韩玉琼特意看了看收银机上显示的时间：9时56分。&ldquo;总理先后递过来两张10元的纸币，两份海南特产的售价一共是19元，我找了总理1元钱。&rdquo;韩玉琼告诉记者，总理付完账后，又伸出手跟她握了手，整个过程中，总理一直面带微笑，真是太和蔼、太亲切了。</p>
', 2, N'/Upload/cms/day_140414/201404140634216673.jpg', 0, 0, 1, 7, N'admin', CAST(0x0000A30D01323C9A AS DateTime))
INSERT [dbo].[Article] ([ID], [Title], [Content], [ChannelId], [CoverPicture], [Hits], [Diggs], [IsActive], [UserId], [UserName], [CreateTime]) VALUES (18, N'李克强', N'<p>hfggh gf</p>
', 3, NULL, 0, 0, 0, 7, N'admin', CAST(0x0000A30F00F15867 AS DateTime))
SET IDENTITY_INSERT [dbo].[Article] OFF
SET IDENTITY_INSERT [dbo].[ArticleTag] ON 

INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (26, 16, 18)
INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (27, 16, 19)
INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (1028, 16, 15)
INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (1029, 16, 16)
INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (1030, 16, 21)
INSERT [dbo].[ArticleTag] ([ID], [ArticleId], [TagId]) VALUES (1031, 16, 22)
SET IDENTITY_INSERT [dbo].[ArticleTag] OFF
SET IDENTITY_INSERT [dbo].[Channel] ON 

INSERT [dbo].[Channel] ([ID], [Name], [Desc], [CoverPicture], [Hits], [IsActive], [CreateTime]) VALUES (1, N'公司动态', N'暂时无', NULL, 0, 1, CAST(0x0000A28700E84B46 AS DateTime))
INSERT [dbo].[Channel] ([ID], [Name], [Desc], [CoverPicture], [Hits], [IsActive], [CreateTime]) VALUES (2, N'最新通知', N'暂时无', NULL, 0, 1, CAST(0x0000A28700E86377 AS DateTime))
INSERT [dbo].[Channel] ([ID], [Name], [Desc], [CoverPicture], [Hits], [IsActive], [CreateTime]) VALUES (3, N'业界新闻', N'暂时没有', NULL, 0, 1, CAST(0x0000A2870144506F AS DateTime))
SET IDENTITY_INSERT [dbo].[Channel] OFF
SET IDENTITY_INSERT [dbo].[Tag] ON 

INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (15, N'最新', 7, CAST(0x0000A2880185565E AS DateTime))
INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (16, N'最热', 2, CAST(0x0000A2880185565E AS DateTime))
INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (18, N'反腐', 4, CAST(0x0000A2A301264AB5 AS DateTime))
INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (19, N'温家宝', 4, CAST(0x0000A2A301264AB5 AS DateTime))
INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (21, N'名人', 3, CAST(0x0000A30D015B34EA AS DateTime))
INSERT [dbo].[Tag] ([ID], [Name], [Hits], [CreateTime]) VALUES (22, N'领导人', 1, CAST(0x0000A30D015B784E AS DateTime))
SET IDENTITY_INSERT [dbo].[Tag] OFF
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_Hits]  DEFAULT ((0)) FOR [Hits]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_Diggs]  DEFAULT ((0)) FOR [Diggs]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_ExternalId]  DEFAULT ((0)) FOR [UserName]
GO
ALTER TABLE [dbo].[Article] ADD  CONSTRAINT [DF_Article_CreateDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ArticleTag] ADD  CONSTRAINT [DF_ArticleTag_ArticleId]  DEFAULT ((0)) FOR [ArticleId]
GO
ALTER TABLE [dbo].[ArticleTag] ADD  CONSTRAINT [DF_ArticleTag_TagId]  DEFAULT ((0)) FOR [TagId]
GO
ALTER TABLE [dbo].[Channel] ADD  CONSTRAINT [DF_Channel_Hits]  DEFAULT ((0)) FOR [Hits]
GO
ALTER TABLE [dbo].[Channel] ADD  CONSTRAINT [DF_Channel_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Channel] ADD  CONSTRAINT [DF_Channel_CreateDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Tag] ADD  CONSTRAINT [DF_Tag_Hits]  DEFAULT ((0)) FOR [Hits]
GO
ALTER TABLE [dbo].[Tag] ADD  CONSTRAINT [DF_Tag_CreateDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
USE [master]
GO
ALTER DATABASE [EviousCms] SET  READ_WRITE 
GO
