CREATE DATABASE [ThreeBrothersMilkTeaShop]
GO

USE [ThreeBrothersMilkTeaShop]
GO

-- ==========================================
-- 1. Tạo bảng Categories
-- ==========================================
CREATE TABLE [dbo].[Categories](
    [CategoryID] INT IDENTITY(1,1) NOT NULL,
    [CategoryName] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    [IsActive] BIT NOT NULL DEFAULT (1),
    PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);
GO

-- ==========================================
-- 2. Tạo bảng Products
-- ==========================================
CREATE TABLE [dbo].[Products](
    [ProductID] INT IDENTITY(1,1) NOT NULL,
    [ProductName] NVARCHAR(100) NOT NULL,
    [CategoryID] INT NOT NULL,
    [Price_S] DECIMAL(10, 2) NOT NULL,
    [Price_M] DECIMAL(10, 2) NOT NULL,
    [Price_L] DECIMAL(10, 2) NOT NULL,
    [IsAvailable_S] BIT NOT NULL DEFAULT (1),
    [IsAvailable_M] BIT NOT NULL DEFAULT (1),
    [IsAvailable_L] BIT NOT NULL DEFAULT (1),
    PRIMARY KEY CLUSTERED ([ProductID] ASC),
    FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Categories]([CategoryID])
);
GO

-- ==========================================
-- 3. Tạo bảng Toppings
-- ==========================================
CREATE TABLE [dbo].[Toppings](
    [ToppingID] INT IDENTITY(1,1) NOT NULL,
    [ToppingName] NVARCHAR(100) NOT NULL,
    [Price] DECIMAL(10, 2) NOT NULL,
    [IsAvailable] BIT NOT NULL DEFAULT (1),
    PRIMARY KEY CLUSTERED ([ToppingID] ASC)
);
GO

-- ==========================================
-- 4. Tạo bảng Users (cập nhật thêm Email và ShippingAddress)
-- ==========================================
CREATE TABLE [dbo].[Users](
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(255) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [PhoneNumber] NVARCHAR(15) NULL,
    [Role] NVARCHAR(20) NOT NULL, -- CHECK ([Role] IN ('Customer', 'Staff', 'Admin')),
    [ShippingAddress] NVARCHAR(255) NULL, -- Thêm trường ShippingAddress
    [IsActive] BIT NOT NULL DEFAULT (1),
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);
GO

-- ==========================================
-- 5. Tạo bảng Orders
-- ==========================================
CREATE TABLE [dbo].[Orders](
    [OrderID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [OrderDate] DATETIME NOT NULL DEFAULT (GETDATE()),
    [TotalAmount] DECIMAL(10, 2) NOT NULL,
    [FinalAmount] DECIMAL(10, 2) NOT NULL,
    [PaymentMethod] NVARCHAR(20) NOT NULL CHECK ([PaymentMethod] IN ('ZaloPay', 'Momo', 'CreditCard', 'Cash')),
    [Status] NVARCHAR(20) NOT NULL CHECK ([Status] IN ('Cancelled', 'Completed', 'Processing', 'Pending')) DEFAULT ('Pending'),
    [Note] NVARCHAR(255) NULL, -- Added Note field here
    PRIMARY KEY CLUSTERED ([OrderID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users]([UserID])
);
GO

-- ==========================================
-- 6. Tạo bảng OrderDetails
-- ==========================================
CREATE TABLE [dbo].[OrderDetails](
    [OrderDetailID] INT IDENTITY(1,1) NOT NULL,
    [OrderID] INT NOT NULL,
    [ProductID] INT NOT NULL,
    [Quantity] INT NOT NULL CHECK ([Quantity] > 0),
    [UnitPrice] DECIMAL(10, 2) NOT NULL,
    [Size] NVARCHAR(1) NOT NULL CHECK ([Size] IN ('L', 'M', 'S')),
    [SugarLevel] NVARCHAR(10) NOT NULL CHECK ([SugarLevel] IN ('100%', '70%', '50%', '30%', '0%')),
    [IceLevel] NVARCHAR(10) NOT NULL CHECK ([IceLevel] IN ('100%', '70%', '50%', '30%', '0%')),
    [SubTotal] DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderDetailID] ASC),
    FOREIGN KEY ([OrderID]) REFERENCES [dbo].[Orders]([OrderID]),
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Products]([ProductID])
);
GO

-- ==========================================
-- 7. Tạo bảng OrderDetailToppings
-- ==========================================
CREATE TABLE [dbo].[OrderDetailToppings](
    [ID] INT IDENTITY(1,1) NOT NULL,
    [OrderDetailID] INT NOT NULL,
    [ToppingID] INT NOT NULL,
    [Quantity] INT NOT NULL DEFAULT (1) CHECK ([Quantity] > 0),
    [Price] DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([OrderDetailID]) REFERENCES [dbo].[OrderDetails]([OrderDetailID]),
    FOREIGN KEY ([ToppingID]) REFERENCES [dbo].[Toppings]([ToppingID])
);
GO

-- ==========================================
-- 8. Insert dữ liệu mẫu
-- ==========================================
SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT [dbo].[Categories] ([CategoryID], [CategoryName], [Description]) VALUES 
(1, N'Flavored Tea', N'Các loại trà có mùi thơm'),
(2, N'Milk Tea', N'Các loại trà sữa'),
(3, N'Tea Latte', N'Các loại trà Latte'),
(4, N'Macchiato', N'Các loại macchiato ngon'),
(5, N'Ice Cream', N'Béo vị kem, đậm vị trà'),
(6, N'Fruit and Juice Tea', N'Giải nhiệt cùng các loại nước uống từ trái cây');
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO

SET IDENTITY_INSERT [dbo].[Toppings] ON
INSERT [dbo].[Toppings] ([ToppingID], [ToppingName], [Price]) VALUES 
(1, N'Trân Châu Hoàng Kim', 7000),
(2, N'Sương Sáo', 5000),
(3, N'Thạch QQ', 7000),
(4, N'Thạch dừa', 5000),
(5, N'Pudding', 8000),
(6, N'Bánh Flan', 12000),
(7, N'Khoai môn viên', 10000),
(8, N'Trân Châu Đen', 5000),
(9, N'Trân Châu Trắng', 5000);
SET IDENTITY_INSERT [dbo].[Toppings] OFF
GO
