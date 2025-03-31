USE [3AnhEmMilkTeaShop]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[Points] [int] NULL,
	[UserID] [int] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [decimal](10, 2) NOT NULL,
	[Size] [nvarchar](10) NULL,
	[SugarLevel] [nvarchar](20) NULL,
	[IceLevel] [nvarchar](20) NULL,
	[SpecialRequest] [nvarchar](255) NULL,
	[SubTotal] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetailToppings]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetailToppings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDetailID] [int] NULL,
	[ToppingID] [int] NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NULL,
	[UserID] [int] NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[Discount] [decimal](10, 2) NULL,
	[FinalAmount] [decimal](10, 2) NOT NULL,
	[PaymentMethod] [nvarchar](50) NULL,
	[Status] [nvarchar](20) NULL,
	[Notes] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[CategoryID] [int] NULL,
	[Description] [nvarchar](255) NULL,
	[Price_S] [decimal](10, 2) NOT NULL,
	[Price_M] [decimal](10, 2) NOT NULL,
	[Price_L] [decimal](10, 2) NOT NULL,
	[IsAvailable_S] [bit] NULL,
	[IsAvailable_M] [bit] NULL,
	[IsAvailable_L] [bit] NULL,
	[ImageURL] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Toppings]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Toppings](
	[ToppingID] [int] IDENTITY(1,1) NOT NULL,
	[ToppingName] [nvarchar](100) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[IsAvailable] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ToppingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 31/03/2025 10:53:41 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Email] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (1, N'Flavored Tea', N'Các loại trà có mùi thơm ', 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (2, N'Milk  Tea', N'Các loại trà sữa ', 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (3, N'Tea Latte', N'Các loại trà Latte', 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (4, N'Macchiato', N'Các loại macchiato ngon', 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (5, N'Ice Cream', N'Béo vị kem, đậm vị trà', 1)
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description], [IsActive]) VALUES (6, N'Fruit and Juice Tea', N'Giải nhiệt cùng các loại nước uống từ trái cây', 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Toppings] ON 

INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (1, N'Trân Châu Hoàng Kim', CAST(7000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (2, N'Sương Sáo', CAST(5000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (3, N'Thạch QQ', CAST(7000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (4, N'Thạch dừa', CAST(5000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (5, N'Pudding', CAST(8000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (6, N'Bánh Flan', CAST(12000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (7, N'Khoai môn viên', CAST(10000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (8, N'Trân Châu Đen', CAST(5000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price], [IsAvailable]) VALUES (9, N'Trân Châu Trắng', CAST(5000.00 AS Decimal(10, 2)), 1)
SET IDENTITY_INSERT [dbo].[Toppings] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4D248E399]    Script Date: 31/03/2025 10:53:41 CH ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT ((0)) FOR [Points]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OrderDetailToppings] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0)) FOR [Discount]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ('Pending') FOR [Status]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [IsAvailable_S]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [IsAvailable_M]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [IsAvailable_L]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Toppings] ADD  DEFAULT ((1)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[OrderDetailToppings]  WITH CHECK ADD FOREIGN KEY([OrderDetailID])
REFERENCES [dbo].[OrderDetails] ([OrderDetailID])
GO
ALTER TABLE [dbo].[OrderDetailToppings]  WITH CHECK ADD FOREIGN KEY([ToppingID])
REFERENCES [dbo].[Toppings] ([ToppingID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([IceLevel]='100%' OR [IceLevel]='70%' OR [IceLevel]='50%' OR [IceLevel]='30%' OR [IceLevel]='0%'))
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([SugarLevel]='100%' OR [SugarLevel]='70%' OR [SugarLevel]='50%' OR [SugarLevel]='30%' OR [SugarLevel]='0%'))
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([Size]='L' OR [Size]='M' OR [Size]='S'))
GO
ALTER TABLE [dbo].[OrderDetailToppings]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD CHECK  (([PaymentMethod]='ZaloPay' OR [PaymentMethod]='Momo' OR [PaymentMethod]='CreditCard' OR [PaymentMethod]='Cash'))
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD CHECK  (([Status]='Cancelled' OR [Status]='Completed' OR [Status]='Processing' OR [Status]='Pending'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]='Customer' OR [Role]='Staff' OR [Role]='Admin'))
GO
