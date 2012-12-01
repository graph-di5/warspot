
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/01/2012 14:23:49
-- Generated from EDMX file: D:\sem4\warspot\WarSpot.Cloud.Storage\DBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WarSpotDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountIntellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Intellect] DROP CONSTRAINT [FK_AccountIntellect];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Game] DROP CONSTRAINT [FK_AccountGame];
GO
IF OBJECT_ID(N'[dbo].[FK_GameGameIntellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_GameGameIntellect];
GO
IF OBJECT_ID(N'[dbo].[FK_IntellectGameIntellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_IntellectGameIntellect];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[Intellect]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Intellect];
GO
IF OBJECT_ID(N'[dbo].[Game]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Game];
GO
IF OBJECT_ID(N'[dbo].[GameIntellect]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameIntellect];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [Account_ID] uniqueidentifier  NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Intellect'
CREATE TABLE [dbo].[Intellect] (
    [Intellect_ID] uniqueidentifier  NOT NULL,
    [Intellect_Name] nvarchar(max)  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Game'
CREATE TABLE [dbo].[Game] (
    [Game_ID] uniqueidentifier  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'GameIntellectÕ‡·Ó'
CREATE TABLE [dbo].[GameIntellectÕ‡·Ó] (
    [GameGame_ID] uniqueidentifier  NOT NULL,
    [IntellectIntellect_ID] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Account_ID] in table 'Account'
ALTER TABLE [dbo].[Account]
ADD CONSTRAINT [PK_Account]
    PRIMARY KEY CLUSTERED ([Account_ID] ASC);
GO

-- Creating primary key on [Intellect_ID] in table 'Intellect'
ALTER TABLE [dbo].[Intellect]
ADD CONSTRAINT [PK_Intellect]
    PRIMARY KEY CLUSTERED ([Intellect_ID] ASC);
GO

-- Creating primary key on [Game_ID] in table 'Game'
ALTER TABLE [dbo].[Game]
ADD CONSTRAINT [PK_Game]
    PRIMARY KEY CLUSTERED ([Game_ID] ASC);
GO

-- Creating primary key on [GameGame_ID], [IntellectIntellect_ID] in table 'GameIntellectÕ‡·Ó'
ALTER TABLE [dbo].[GameIntellectÕ‡·Ó]
ADD CONSTRAINT [PK_GameIntellectÕ‡·Ó]
    PRIMARY KEY NONCLUSTERED ([GameGame_ID], [IntellectIntellect_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccountAccount_ID] in table 'Intellect'
ALTER TABLE [dbo].[Intellect]
ADD CONSTRAINT [FK_AccountIntellect]
    FOREIGN KEY ([AccountAccount_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountIntellect'
CREATE INDEX [IX_FK_AccountIntellect]
ON [dbo].[Intellect]
    ([AccountAccount_ID]);
GO

-- Creating foreign key on [AccountAccount_ID] in table 'Game'
ALTER TABLE [dbo].[Game]
ADD CONSTRAINT [FK_AccountGame]
    FOREIGN KEY ([AccountAccount_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountGame'
CREATE INDEX [IX_FK_AccountGame]
ON [dbo].[Game]
    ([AccountAccount_ID]);
GO

-- Creating foreign key on [GameGame_ID] in table 'GameIntellectÕ‡·Ó'
ALTER TABLE [dbo].[GameIntellectÕ‡·Ó]
ADD CONSTRAINT [FK_GameGameIntellect]
    FOREIGN KEY ([GameGame_ID])
    REFERENCES [dbo].[Game]
        ([Game_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IntellectIntellect_ID] in table 'GameIntellectÕ‡·Ó'
ALTER TABLE [dbo].[GameIntellectÕ‡·Ó]
ADD CONSTRAINT [FK_IntellectGameIntellect]
    FOREIGN KEY ([IntellectIntellect_ID])
    REFERENCES [dbo].[Intellect]
        ([Intellect_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IntellectGameIntellect'
CREATE INDEX [IX_FK_IntellectGameIntellect]
ON [dbo].[GameIntellectÕ‡·Ó]
    ([IntellectIntellect_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------