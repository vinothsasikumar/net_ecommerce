-- Schema: basic eCommerce relational model for MSSQL
-- Includes: Users, UserProfiles, Addresses, Products, Categories, ProductCategories,
-- Orders, OrderItems, Payments, PaymentMethods, OrderStatuses, PaymentStatuses

-- Lookup: Order statuses
CREATE TABLE dbo.OrderStatuses (
    OrderStatusId TINYINT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200) NULL
);
INSERT INTO dbo.OrderStatuses (OrderStatusId, Name) VALUES
(1, 'Pending'), (2, 'Processing'), (3, 'Shipped'), (4, 'Completed'), (5, 'Cancelled'), (6, 'Refunded');

-- Lookup: Payment statuses
CREATE TABLE dbo.PaymentStatuses (
    PaymentStatusId TINYINT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);
INSERT INTO dbo.PaymentStatuses (PaymentStatusId, Name) VALUES
(1, 'Pending'), (2, 'Authorized'), (3, 'Captured'), (4, 'Failed'), (5, 'Refunded');

-- Payment methods
CREATE TABLE dbo.PaymentMethods (
    PaymentMethodId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Provider NVARCHAR(100) NULL,
    Details NVARCHAR(MAX) NULL -- JSON or provider config
);

-- Users
CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(512) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    LastLoginAt DATETIME2 NULL,
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT UQ_Users_Username UNIQUE (Username)
);
CREATE INDEX IX_Users_IsDeleted ON dbo.Users(IsDeleted);

-- User profiles (1:1)
CREATE TABLE dbo.UserProfiles (
    UserId INT PRIMARY KEY,
    FirstName NVARCHAR(100) NULL,
    LastName NVARCHAR(100) NULL,
    Phone NVARCHAR(50) NULL,
    BirthDate DATE NULL,
    Gender NVARCHAR(20) NULL,
    Bio NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_UserProfiles_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId) ON DELETE CASCADE
);

-- Addresses (users may have multiple addresses)
CREATE TABLE dbo.Addresses (
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    AddressType NVARCHAR(50) NOT NULL, -- e.g. 'billing', 'shipping'
    Line1 NVARCHAR(250) NOT NULL,
    Line2 NVARCHAR(250) NULL,
    City NVARCHAR(100) NOT NULL,
    Region NVARCHAR(100) NULL,
    PostalCode NVARCHAR(30) NULL,
    Country NVARCHAR(100) NOT NULL,
    IsDefault BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Addresses_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId) ON DELETE CASCADE
);
CREATE INDEX IX_Addresses_UserId ON dbo.Addresses(UserId);

-- Products
CREATE TABLE dbo.Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    SKU NVARCHAR(50) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    ShortDescription NVARCHAR(1000) NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(18,2) NOT NULL CONSTRAINT CK_Products_Price_NonNeg CHECK (Price >= 0),
    Cost DECIMAL(18,2) NULL CONSTRAINT CK_Products_Cost_NonNeg CHECK (Cost >= 0),
    Currency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    Stock INT NOT NULL DEFAULT 0,
    IsPublished BIT NOT NULL DEFAULT 0,
    Weight DECIMAL(10,3) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT UQ_Products_SKU UNIQUE (SKU)
);
CREATE INDEX IX_Products_IsPublished ON dbo.Products(IsPublished);
CREATE INDEX IX_Products_IsDeleted ON dbo.Products(IsDeleted);

-- Categories and product-category many-to-many
CREATE TABLE dbo.Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Slug NVARCHAR(150) NOT NULL,
    ParentCategoryId INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT UQ_Categories_Slug UNIQUE (Slug),
    CONSTRAINT FK_Categories_Parent FOREIGN KEY (ParentCategoryId) REFERENCES dbo.Categories(CategoryId) ON DELETE NO ACTION
);

CREATE TABLE dbo.ProductCategories (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (ProductId, CategoryId),
    CONSTRAINT FK_ProductCategories_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId) ON DELETE CASCADE,
    CONSTRAINT FK_ProductCategories_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(CategoryId) ON DELETE CASCADE
);

-- Orders
CREATE TABLE dbo.Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE, -- e.g. generated external order id
    UserId INT NULL, -- allow guest orders (NULL) or link to user
    OrderStatusId TINYINT NOT NULL DEFAULT 1,
    Currency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    Subtotal DECIMAL(18,2) NOT NULL CONSTRAINT CK_Orders_Subtotal_NonNeg CHECK (Subtotal >= 0),
    Shipping DECIMAL(18,2) NOT NULL DEFAULT 0 CONSTRAINT CK_Orders_Shipping_NonNeg CHECK (Shipping >= 0),
    Tax DECIMAL(18,2) NOT NULL DEFAULT 0 CONSTRAINT CK_Orders_Tax_NonNeg CHECK (Tax >= 0),
    Total DECIMAL(18,2) NOT NULL CONSTRAINT CK_Orders_Total_NonNeg CHECK (Total >= 0),
    BillingAddressId INT NULL,
    ShippingAddressId INT NULL,
    Notes NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(UserId) ON DELETE SET NULL,
    CONSTRAINT FK_Orders_Status FOREIGN KEY (OrderStatusId) REFERENCES dbo.OrderStatuses(OrderStatusId) ON DELETE NO ACTION,
    CONSTRAINT FK_Orders_BillingAddress FOREIGN KEY (BillingAddressId) REFERENCES dbo.Addresses(AddressId) ON DELETE NO ACTION,
    CONSTRAINT FK_Orders_ShippingAddress FOREIGN KEY (ShippingAddressId) REFERENCES dbo.Addresses(AddressId) ON DELETE NO ACTION
);
CREATE INDEX IX_Orders_UserId ON dbo.Orders(UserId);
CREATE INDEX IX_Orders_OrderStatus ON dbo.Orders(OrderStatusId);

-- Order items (snapshot product details for historical integrity)
CREATE TABLE dbo.OrderItems (
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NULL,
    SKU NVARCHAR(50) NULL,          -- snapshot
    ProductName NVARCHAR(255) NULL, -- snapshot
    UnitPrice DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL CONSTRAINT CK_OrderItems_Qty_Positive CHECK (Quantity > 0),
    LineTotal AS (UnitPrice * Quantity) PERSISTED,
    Tax DECIMAL(18,2) NOT NULL DEFAULT 0,
    Discount DECIMAL(18,2) NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId) ON DELETE SET NULL
);
CREATE INDEX IX_OrderItems_OrderId ON dbo.OrderItems(OrderId);

-- Payments (an order may have multiple payments: partial, refunds etc.)
CREATE TABLE dbo.Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    PaymentMethodId INT NULL,
    Amount DECIMAL(18,2) NOT NULL CONSTRAINT CK_Payments_Amount_Positive CHECK (Amount >= 0),
    Currency NVARCHAR(3) NOT NULL DEFAULT 'USD',
    PaymentStatusId TINYINT NOT NULL DEFAULT 1,
    TransactionReference NVARCHAR(200) NULL,
    ProcessedAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Payments_Orders FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT FK_Payments_Method FOREIGN KEY (PaymentMethodId) REFERENCES dbo.PaymentMethods(PaymentMethodId) ON DELETE SET NULL,
    CONSTRAINT FK_Payments_Status FOREIGN KEY (PaymentStatusId) REFERENCES dbo.PaymentStatuses(PaymentStatusId) ON DELETE NO ACTION
);
CREATE INDEX IX_Payments_OrderId ON dbo.Payments(OrderId);
CREATE INDEX IX_Payments_TransactionReference ON dbo.Payments(TransactionReference);

-- Optional: product images table
CREATE TABLE dbo.ProductImages (
    ProductImageId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
    Url NVARCHAR(1000) NOT NULL,
    AltText NVARCHAR(255) NULL,
    SortOrder INT NOT NULL DEFAULT 0,
    IsPrimary BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_ProductImages_Products FOREIGN KEY (ProductId) REFERENCES dbo.Products(ProductId) ON DELETE CASCADE
);
CREATE INDEX IX_ProductImages_ProductId ON dbo.ProductImages(ProductId);

-- Useful indexes for queries
CREATE INDEX IX_Orders_CreatedAt ON dbo.Orders(CreatedAt);
CREATE INDEX IX_Products_Price ON dbo.Products(Price);

-- Example: ensure safe deletes by using soft-delete flags where appropriate, and keep FK behavior conservative (NO ACTION/SET NULL)