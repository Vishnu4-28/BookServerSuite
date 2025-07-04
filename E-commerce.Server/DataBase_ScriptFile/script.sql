USE [master]
GO
/****** Object:  Database [BookInventoryApplication]    Script Date: 30-05-2025 11:45:48 AM ******/
CREATE DATABASE [BookInventoryApplication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BookInventoryApplication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BookInventoryApplication.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BookInventoryApplication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BookInventoryApplication_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BookInventoryApplication] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BookInventoryApplication].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BookInventoryApplication] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET ARITHABORT OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BookInventoryApplication] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BookInventoryApplication] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BookInventoryApplication] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BookInventoryApplication] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BookInventoryApplication] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BookInventoryApplication] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BookInventoryApplication] SET  MULTI_USER 
GO
ALTER DATABASE [BookInventoryApplication] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BookInventoryApplication] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BookInventoryApplication] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BookInventoryApplication] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BookInventoryApplication] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BookInventoryApplication] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BookInventoryApplication] SET QUERY_STORE = ON
GO
ALTER DATABASE [BookInventoryApplication] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BookInventoryApplication]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 30-05-2025 11:45:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 30-05-2025 11:45:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[Book_Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Author] [nvarchar](max) NOT NULL,
	[ISBN] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[Book_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 30-05-2025 11:45:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [nvarchar](max) NULL,
	[Last_Name] [nvarchar](max) NULL,
	[Date_Of_Birth] [datetime2](7) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Phone_Number] [nvarchar](max) NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250526074259_initial', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250526074433_BooksEntitie', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250528085135_updatedBoookEntity', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250529101455_userTable', N'8.0.0')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250529115824_updateUser', N'8.0.0')
GO
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (3, N'end with us ', N'ANA hong', 256655455, 1, 0)
INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (4, N'iceBreaker', N'hannah grace', 254648751, 2, 0)
INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (5, N'twisted Games', N'Ana Huang', 1234569, 22, 0)
INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (1011, N'ghsfdgh', N'fghfgh', 646464644, 11, 0)
INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (1012, N'hghf', N'fghfgh', 196898955, 2, 1)
INSERT [dbo].[Books] ([Book_Id], [Title], [Author], [ISBN], [Quantity], [IsDeleted]) VALUES (1013, N' dfghdgh', N' dghdfghdfgh', 1, 1, 0)
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([User_Id], [First_Name], [Last_Name], [Date_Of_Birth], [Email], [Password], [Phone_Number], [Role]) VALUES (1, N'vishnu', N'Yadav', CAST(N'2003-04-28T11:28:30.6190000' AS DateTime2), N'vishnuyadav@gmail.com', N'Vishnu@28', NULL, 0)
INSERT [dbo].[Users] ([User_Id], [First_Name], [Last_Name], [Date_Of_Birth], [Email], [Password], [Phone_Number], [Role]) VALUES (2, N'book', N'admin', CAST(N'2003-04-28T11:28:30.6190000' AS DateTime2), N'Admin@gmail.com', N'Admin@123', N'8013042517', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Books] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Role]
GO
USE [master]
GO
ALTER DATABASE [BookInventoryApplication] SET  READ_WRITE 
GO
