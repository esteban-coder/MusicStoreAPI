IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110010818_FirstMigration')
BEGIN
    CREATE TABLE [Genre] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Genre] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110010818_FirstMigration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221110010818_FirstMigration', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110013855_ConcertsTable')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Genre]') AND [c].[name] = N'Name');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Genre] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Genre] ALTER COLUMN [Name] nvarchar(150) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110013855_ConcertsTable')
BEGIN
    CREATE TABLE [Events] (
        [Id] int NOT NULL IDENTITY,
        [GenreId] int NOT NULL,
        [Title] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [DateEvent] datetime2 NOT NULL,
        [ImageUrl] nvarchar(max) NULL,
        [Place] nvarchar(100) NULL,
        [TicketsQuantity] int NOT NULL,
        [UnitPrice] decimal(11,2) NOT NULL,
        [Finalized] bit NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Events_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110013855_ConcertsTable')
BEGIN
    CREATE INDEX [IX_Events_GenreId] ON [Events] ([GenreId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110013855_ConcertsTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221110013855_ConcertsTable', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110020138_DataForGenres')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] ON;
    EXEC(N'INSERT INTO [Genre] ([Id], [Name], [Status])
    VALUES (1, N''Rock'', CAST(1 AS bit)),
    (2, N''Pop'', CAST(1 AS bit)),
    (3, N''Jazz'', CAST(1 AS bit)),
    (4, N''Metal'', CAST(1 AS bit)),
    (5, N''Disco'', CAST(1 AS bit)),
    (6, N''Blues'', CAST(1 AS bit)),
    (7, N''Reggae'', CAST(1 AS bit)),
    (8, N''Reggaeton'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'Status') AND [object_id] = OBJECT_ID(N'[Genre]'))
        SET IDENTITY_INSERT [Genre] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221110020138_DataForGenres')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221110020138_DataForGenres', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221117021404_CustomersAndSales')
BEGIN
    CREATE TABLE [Customer] (
        [Id] int NOT NULL IDENTITY,
        [Email] nvarchar(500) NOT NULL,
        [FullName] nvarchar(200) NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221117021404_CustomersAndSales')
BEGIN
    CREATE TABLE [Sale] (
        [Id] int NOT NULL IDENTITY,
        [CustomerForeignKey] int NOT NULL,
        [SaleDate] datetime2 NOT NULL,
        [OperationNumber] varchar(10) NOT NULL,
        [Total] decimal(11,2) NOT NULL,
        [ConcertId] int NOT NULL,
        [Quantity] int NOT NULL,
        [Status] bit NOT NULL,
        CONSTRAINT [PK_Sale] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Sale_Customer_CustomerForeignKey] FOREIGN KEY ([CustomerForeignKey]) REFERENCES [Customer] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Sale_Events_ConcertId] FOREIGN KEY ([ConcertId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221117021404_CustomersAndSales')
BEGIN
    CREATE INDEX [IX_Sale_ConcertId] ON [Sale] ([ConcertId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221117021404_CustomersAndSales')
BEGIN
    CREATE INDEX [IX_Sale_CustomerForeignKey] ON [Sale] ([CustomerForeignKey]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221117021404_CustomersAndSales')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221117021404_CustomersAndSales', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Age] int NOT NULL,
        [DocumentType] nvarchar(max) NOT NULL,
        [DocumentNumber] nvarchar(max) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221122024122_SecurityTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221122024122_SecurityTables', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221208010008_SaleStoredProcedures')
BEGIN
    CREATE PROCEDURE uspListSales(@StartDate DATE, @EndDate DATE, @Page INT, @PageSize INT)
    AS
    BEGIN
    	
    	SELECT 
    		S.Id SaleId,
    		E.DateEvent,
    		G.Name Genre,
    		E.ImageUrl,
    		E.Title,
    		S.OperationNumber,
    		C.FullName,
    		S.Quantity,
    		S.SaleDate,
    		S.Total
    	FROM Sale S
    	INNER JOIN Customer C ON S.CustomerForeignKey = C.Id
    	INNER JOIN [Events] E ON S.ConcertId = E.Id
    	INNER JOIN Genre G ON E.GenreId = G.Id
    	WHERE S.Status = 1
    	AND CAST(S.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
    	ORDER BY E.Title
    	OFFSET @PAGE ROWS FETCH NEXT @PAGESIZE ROWS ONLY;
    	
    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221208010008_SaleStoredProcedures')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221208010008_SaleStoredProcedures', N'7.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221208012812_ModifiefStoredProcedures')
BEGIN
    ALTER PROCEDURE uspListSales(@StartDate DATE, @EndDate DATE, @Page INT, @PageSize INT)
    AS
    BEGIN
    	
    	SELECT 
    		S.Id SaleId,
    		E.DateEvent,
    		G.Name Genre,
    		E.ImageUrl,
    		E.Title,
    		S.OperationNumber,
    		C.FullName,
    		C.Email,
    		S.Quantity,
    		S.SaleDate,
    		S.Total
    	FROM Sale S
    	INNER JOIN Customer C ON S.CustomerForeignKey = C.Id
    	INNER JOIN [Events] E ON S.ConcertId = E.Id
    	INNER JOIN Genre G ON E.GenreId = G.Id
    	WHERE S.Status = 1
    	AND CAST(S.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
    	ORDER BY E.Title
    	OFFSET @PAGE ROWS FETCH NEXT @PAGESIZE ROWS ONLY;
    	
    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221208012812_ModifiefStoredProcedures')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221208012812_ModifiefStoredProcedures', N'7.0.0');
END;
GO

COMMIT;
GO

