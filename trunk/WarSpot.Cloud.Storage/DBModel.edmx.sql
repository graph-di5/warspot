
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/18/2013 00:57:15
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
IF OBJECT_ID(N'[dbo].[FK_GameGameIntellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_GameGameIntellect];
GO
IF OBJECT_ID(N'[dbo].[FK_IntellectGameIntellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameIntellect] DROP CONSTRAINT [FK_IntellectGameIntellect];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRole] DROP CONSTRAINT [FK_AccountUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournament]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tournament] DROP CONSTRAINT [FK_AccountTournament];
GO
IF OBJECT_ID(N'[dbo].[FK_TournamentTournamentPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TournamentPlayers] DROP CONSTRAINT [FK_TournamentTournamentPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournamentPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TournamentPlayers] DROP CONSTRAINT [FK_AccountTournamentPlayer];
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
IF OBJECT_ID(N'[dbo].[UserRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRole];
GO
IF OBJECT_ID(N'[dbo].[Tournament]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tournament];
GO
IF OBJECT_ID(N'[dbo].[TournamentPlayers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TournamentPlayers];
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

-- Creating table 'GameIntellect'
CREATE TABLE [dbo].[GameIntellect] (
    [GameGame_ID] uniqueidentifier  NOT NULL,
    [IntellectIntellect_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [Role_ID] smallint  NOT NULL,
    [Until] nvarchar(max)  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL,
    [Role_Code] smallint  NOT NULL
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

-- Creating table 'TournamentPlayers'
CREATE TABLE [dbo].[TournamentPlayers] (
    [TournamentPlayer_ID] uniqueidentifier  NOT NULL,
    [TournamentTournament_ID] uniqueidentifier  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL
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

-- Creating primary key on [GameGame_ID], [IntellectIntellect_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [PK_GameIntellect]
    PRIMARY KEY NONCLUSTERED ([GameGame_ID], [IntellectIntellect_ID] ASC);
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

-- Creating primary key on [TournamentPlayer_ID] in table 'TournamentPlayers'
ALTER TABLE [dbo].[TournamentPlayers]
ADD CONSTRAINT [PK_TournamentPlayers]
    PRIMARY KEY CLUSTERED ([TournamentPlayer_ID] ASC);
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

-- Creating foreign key on [GameGame_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [FK_GameGameIntellect]
    FOREIGN KEY ([GameGame_ID])
    REFERENCES [dbo].[Game]
        ([Game_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IntellectIntellect_ID] in table 'GameIntellect'
ALTER TABLE [dbo].[GameIntellect]
ADD CONSTRAINT [FK_IntellectGameIntellect]
    FOREIGN KEY ([IntellectIntellect_ID])
    REFERENCES [dbo].[Intellect]
        ([Intellect_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IntellectGameIntellect'
CREATE INDEX [IX_FK_IntellectGameIntellect]
ON [dbo].[GameIntellect]
    ([IntellectIntellect_ID]);
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

-- Creating foreign key on [TournamentTournament_ID] in table 'TournamentPlayers'
ALTER TABLE [dbo].[TournamentPlayers]
ADD CONSTRAINT [FK_TournamentTournamentPlayer]
    FOREIGN KEY ([TournamentTournament_ID])
    REFERENCES [dbo].[Tournament]
        ([Tournament_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TournamentTournamentPlayer'
CREATE INDEX [IX_FK_TournamentTournamentPlayer]
ON [dbo].[TournamentPlayers]
    ([TournamentTournament_ID]);
GO

-- Creating foreign key on [AccountAccount_ID] in table 'TournamentPlayers'
ALTER TABLE [dbo].[TournamentPlayers]
ADD CONSTRAINT [FK_AccountTournamentPlayer]
    FOREIGN KEY ([AccountAccount_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountTournamentPlayer'
CREATE INDEX [IX_FK_AccountTournamentPlayer]
ON [dbo].[TournamentPlayers]
    ([AccountAccount_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------