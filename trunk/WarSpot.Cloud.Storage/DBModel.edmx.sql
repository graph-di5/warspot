
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/27/2013 12:43:03
-- Generated from EDMX file: C:\Users\Grigorii\Documents\Visual Studio 2012\Projects\Warspot\WarSpot.Cloud.Storage\DBModel.edmx
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
IF OBJECT_ID(N'[dbo].[FK_AccountUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_AccountUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournament]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tournament] DROP CONSTRAINT [FK_AccountTournament];
GO
IF OBJECT_ID(N'[dbo].[FK_GameIntellect_Game]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_GameIntellect_Game];
GO
IF OBJECT_ID(N'[dbo].[FK_GameIntellect_Intellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_GameIntellect_Intellect];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournament1_Account]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPlayer] DROP CONSTRAINT [FK_AccountTournament1_Account];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournament1_Tournament]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPlayer] DROP CONSTRAINT [FK_AccountTournament1_Tournament];
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
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[Tournament]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tournament];
GO
IF OBJECT_ID(N'[dbo].[GameIntellect]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameIntellect];
GO
IF OBJECT_ID(N'[dbo].[AccountPlayer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountPlayer];
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
    [AccountAccount_ID] uniqueidentifier  NOT NULL,
    [Replay] nvarchar(max)  NULL,
    [CreationTime] nvarchar(max)  NOT NULL,
    [Game_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [Role_ID] uniqueidentifier  NOT NULL,
    [Until] nvarchar(max)  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL,
    [Role_Code] int  NOT NULL
);
GO

-- Creating table 'Tournament'
CREATE TABLE [dbo].[Tournament] (
    [Tournament_ID] uniqueidentifier  NOT NULL,
    [MaxPlayers] bigint  NOT NULL,
    [When] nvarchar(max)  NOT NULL,
    [Creator_ID] uniqueidentifier  NOT NULL,
    [Tournament_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GameIntellect'
CREATE TABLE [dbo].[GameIntellect] (
    [Games_Game_ID] uniqueidentifier  NOT NULL,
    [Intellects_Intellect_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'AccountPlayer'
CREATE TABLE [dbo].[AccountPlayer] (
    [Player_Account_ID] uniqueidentifier  NOT NULL,
    [TournamentPlayer_Tournament_ID] uniqueidentifier  NOT NULL
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

-- Creating primary key on [Role_ID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [PK_UserRole]
    PRIMARY KEY CLUSTERED ([Role_ID] ASC);
GO

-- Creating primary key on [Tournament_ID] in table 'Tournament'
ALTER TABLE [dbo].[Tournament]
ADD CONSTRAINT [PK_Tournament]
    PRIMARY KEY CLUSTERED ([Tournament_ID] ASC);
GO

-- Creating primary key on [Games_Game_ID], [Intellects_Intellect_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [PK_GameIntellect]
    PRIMARY KEY NONCLUSTERED ([Games_Game_ID], [Intellects_Intellect_ID] ASC);
GO

-- Creating primary key on [Player_Account_ID], [TournamentPlayer_Tournament_ID] in table 'AccountPlayer'
ALTER TABLE [dbo].[AccountPlayer]
ADD CONSTRAINT [PK_AccountPlayer]
    PRIMARY KEY NONCLUSTERED ([Player_Account_ID], [TournamentPlayer_Tournament_ID] ASC);
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

-- Creating foreign key on [AccountAccount_ID] in table 'UserRole'
ALTER TABLE [dbo].[UserRole]
ADD CONSTRAINT [FK_AccountUserRole]
    FOREIGN KEY ([AccountAccount_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountUserRole'
CREATE INDEX [IX_FK_AccountUserRole]
ON [dbo].[UserRole]
    ([AccountAccount_ID]);
GO

-- Creating foreign key on [Creator_ID] in table 'Tournament'
ALTER TABLE [dbo].[Tournament]
ADD CONSTRAINT [FK_AccountTournament]
    FOREIGN KEY ([Creator_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountTournament'
CREATE INDEX [IX_FK_AccountTournament]
ON [dbo].[Tournament]
    ([Creator_ID]);
GO

-- Creating foreign key on [Games_Game_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [FK_GameIntellect_Game]
    FOREIGN KEY ([Games_Game_ID])
    REFERENCES [dbo].[Game]
        ([Game_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Intellects_Intellect_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [FK_GameIntellect_Intellect]
    FOREIGN KEY ([Intellects_Intellect_ID])
    REFERENCES [dbo].[Intellect]
        ([Intellect_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GameIntellect_Intellect'
CREATE INDEX [IX_FK_GameIntellect_Intellect]
ON [dbo].[GameIntellect]
    ([Intellects_Intellect_ID]);
GO

-- Creating foreign key on [Player_Account_ID] in table 'AccountPlayer'
ALTER TABLE [dbo].[AccountPlayer]
ADD CONSTRAINT [FK_AccountTournament1_Account]
    FOREIGN KEY ([Player_Account_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TournamentPlayer_Tournament_ID] in table 'AccountPlayer'
ALTER TABLE [dbo].[AccountPlayer]
ADD CONSTRAINT [FK_AccountTournament1_Tournament]
    FOREIGN KEY ([TournamentPlayer_Tournament_ID])
    REFERENCES [dbo].[Tournament]
        ([Tournament_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountTournament1_Tournament'
CREATE INDEX [IX_FK_AccountTournament1_Tournament]
ON [dbo].[AccountPlayer]
    ([TournamentPlayer_Tournament_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------