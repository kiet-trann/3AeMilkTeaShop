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
    [Description] NVARCHAR(100),
    [CategoryID] INT NOT NULL,
    [Price_S] DECIMAL(10, 2) NOT NULL,
    [Price_M] DECIMAL(10, 2) NOT NULL,
    [Price_L] DECIMAL(10, 2) NOT NULL,
    [IsAvailable_S] BIT NOT NULL DEFAULT (1),
    [IsAvailable_M] BIT NOT NULL DEFAULT (1),
    [IsAvailable_L] BIT NOT NULL DEFAULT (1),
    [ImageUrl] NVARCHAR(MAX),
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

SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [CategoryID], [Price_S], [Price_M], [Price_L], [IsAvailable_S], [IsAvailable_M], [IsAvailable_L], [ImageUrl]) VALUES 
(1, N'Trà Sữa Trân Châu Đường Đen', N'Hương vị ngọt ngào từ đường đen với trân châu dai ngon', 2, 25000, 30000, 35000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737356814_ts-oolong-nuong-suong-sao_38937aa46d4f4ccb9525df18c752ef0c.png'),
(2, N'Trà Sữa Matcha', N'Hòa quyện vị trà xanh Matcha Nhật Bản và sữa béo', 2, 27000, 32000, 37000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737358282_tra-sua-oolong-tu-quy-bo_e1de7569aad44e9883d38fd31a4e9a7e_large.png'),
(3, N'Trà Sữa Thái Xanh', N'Hương vị trà Thái xanh tươi mát', 2, 25000, 30000, 35000, 1, 1, 0, N'https://product.hstatic.net/1000075078/product/1737356772_tra-sua-oolong-blao_0790aac40efb45afaef2b1c6abbb1da3_large.png'),
(4, N'Trà Ô Long Kem Macchiato', N'Vị trà Ô Long thanh mát kết hợp với lớp kem béo', 4, 30000, 35000, 40000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737358261_hong-tra-sua-tran-chau_df79631d280046c7a6460f87f4a27e43_large.png'),
(5, N'Trà Đào Cam Sả', N'Giải nhiệt với hương vị trà đào kết hợp cam sả', 6, 28000, 33000, 38000, 1, 0, 1, N'https://product.hstatic.net/1000075078/product/1737355604_tx-latte-nong_111bf6feaf044991bd17012b13f0ecc3_large.png'),
(6, N'Trà Sữa Khoai Môn', N'Vị khoai môn béo ngậy hòa quyện với trà sữa', 2, 27000, 32000, 37000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737355662_tx-latte-dam-vi_c74112b7a7a74c56a45f4851598389b0_large.png'),
(7, N'Trà Latte', N'Sự kết hợp hoàn hảo giữa trà và sữa', 3, 29000, 34000, 39000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737355641_tra-xanh-latte-sua-yen-mach-nong_296fe546228a480cb382a891ed4fd3d0_large.png'),
(8, N'Trà Sữa Pudding', N'Trà sữa thơm béo với pudding mềm mịn', 2, 30000, 35000, 40000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737355560_chocolate-da_56317be1073646d880612ae27912f343.png'),
(9, N'Trà Sữa Hồng Trà', N'Vị trà đậm đà và béo ngậy', 2, 25000, 30000, 35000, 1, 0, 1, N'https://product.hstatic.net/1000075078/product/1737356372_oolong-tu-quy-dau_14feaccad86e4f22ad0147dea899adae_large.png'),
(10, N'Trà Sữa Trân Châu Hoàng Kim', N'Hương vị thơm ngon từ trân châu hoàng kim', 2, 27000, 32000, 37000, 1, 1, 1, N'https://product.hstatic.net/1000075078/product/1737356364_oolong-kim-quat-tran-chau_b12f1fb7ed2c45f4991332bfb50541a4_large.png');
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
