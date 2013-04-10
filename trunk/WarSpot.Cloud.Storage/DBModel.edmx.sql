
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/10/2013 23:41:55
-- Generated from EDMX file: C:\Users\deem\Documents\warspot_\trunk\WarSpot.Cloud.Storage\DBModel.edmx
-- --------------------------------------------------

create database WarSpotDB;

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
IF OBJECT_ID(N'[dbo].[FK_AccountTournament1_Account]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPlayer] DROP CONSTRAINT [FK_AccountTournament1_Account];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountTournament1_Tournament]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountPlayer] DROP CONSTRAINT [FK_AccountTournament1_Tournament];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_TeamGame];
GO
IF OBJECT_ID(N'[dbo].[FK_IntellectTeam_Intellect]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IntellectTeam] DROP CONSTRAINT [FK_IntellectTeam_Intellect];
GO
IF OBJECT_ID(N'[dbo].[FK_IntellectTeam_Team]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[IntellectTeam] DROP CONSTRAINT [FK_IntellectTeam_Team];
GO
IF OBJECT_ID(N'[dbo].[FK_GameGameDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GameDetails] DROP CONSTRAINT [FK_GameGameDetails];
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
IF OBJECT_ID(N'[dbo].[Securities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Securities];
GO
IF OBJECT_ID(N'[dbo].[Files]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Files];
GO
IF OBJECT_ID(N'[dbo].[GameDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GameDetails];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[Stages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stages];
GO
IF OBJECT_ID(N'[dbo].[AccountPlayer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountPlayer];
GO
IF OBJECT_ID(N'[dbo].[IntellectTeam]', 'U') IS NOT NULL
    DROP TABLE [dbo].[IntellectTeam];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [Account_ID] uniqueidentifier  NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Password] nvarchar(max)  NOT NULL,
    [UserFullName] nvarchar(max)  NOT NULL,
    [UserSurname] nvarchar(max)  NOT NULL,
    [Institution] nvarchar(max)  NOT NULL,
    [Course] int  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Intellect'
CREATE TABLE [dbo].[Intellect] (
    [Intellect_ID] uniqueidentifier  NOT NULL,
    [Intellect_Name] nvarchar(max)  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Game'
CREATE TABLE [dbo].[Game] (
    [Game_ID] uniqueidentifier  NOT NULL,
    [Creator_ID] uniqueidentifier  NOT NULL,
    [Replay] nvarchar(max)  NULL,
    [CreationTime] datetime  NOT NULL,
    [Game_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRole'
CREATE TABLE [dbo].[UserRole] (
    [Role_ID] uniqueidentifier  NOT NULL,
    [Until] datetime  NOT NULL,
    [AccountAccount_ID] uniqueidentifier  NOT NULL,
    [Role_Code] int  NOT NULL
);
GO

-- Creating table 'Tournament'
CREATE TABLE [dbo].[Tournament] (
    [Tournament_ID] uniqueidentifier  NOT NULL,
    [MaxPlayers] bigint  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Creator_ID] uniqueidentifier  NOT NULL,
    [Tournament_Name] nvarchar(max)  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Securities'
CREATE TABLE [dbo].[Securities] (
    [IllegalReferenceName] nvarchar(max)  NOT NULL,
    [Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Files'
CREATE TABLE [dbo].[Files] (
    [File_Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreationTime] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [LongComment] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GameDetails'
CREATE TABLE [dbo].[GameDetails] (
    [GameDetails_ID] uniqueidentifier  NOT NULL,
    [StepsCount] int  NOT NULL,
    [Winner_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Team_ID] uniqueidentifier  NOT NULL,
    [GameGame_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Stages'
CREATE TABLE [dbo].[Stages] (
    [Stage_ID] uniqueidentifier  NOT NULL,
    [TournamentTournament_ID] uniqueidentifier  NOT NULL,
    [State] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [StartTime] datetime  NOT NULL
);
GO

-- Creating table 'AccountPlayer'
CREATE TABLE [dbo].[AccountPlayer] (
    [Player_Account_ID] uniqueidentifier  NOT NULL,
    [TournamentPlayer_Tournament_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'IntellectTeam'
CREATE TABLE [dbo].[IntellectTeam] (
    [Intellects_Intellect_ID] uniqueidentifier  NOT NULL,
    [Teams_Team_ID] uniqueidentifier  NOT NULL
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

-- Creating primary key on [Id] in table 'Securities'
ALTER TABLE [dbo].[Securities]
ADD CONSTRAINT [PK_Securities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [File_Id] in table 'Files'
ALTER TABLE [dbo].[Files]
ADD CONSTRAINT [PK_Files]
    PRIMARY KEY CLUSTERED ([File_Id] ASC);
GO

-- Creating primary key on [GameDetails_ID] in table 'GameDetails'
ALTER TABLE [dbo].[GameDetails]
ADD CONSTRAINT [PK_GameDetails]
    PRIMARY KEY CLUSTERED ([GameDetails_ID] ASC);
GO

-- Creating primary key on [Team_ID] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Team_ID] ASC);
GO

-- Creating primary key on [Stage_ID] in table 'Stages'
ALTER TABLE [dbo].[Stages]
ADD CONSTRAINT [PK_Stages]
    PRIMARY KEY CLUSTERED ([Stage_ID] ASC);
GO

-- Creating primary key on [Player_Account_ID], [TournamentPlayer_Tournament_ID] in table 'AccountPlayer'
ALTER TABLE [dbo].[AccountPlayer]
ADD CONSTRAINT [PK_AccountPlayer]
    PRIMARY KEY NONCLUSTERED ([Player_Account_ID], [TournamentPlayer_Tournament_ID] ASC);
GO

-- Creating primary key on [Intellects_Intellect_ID], [Teams_Team_ID] in table 'IntellectTeam'
ALTER TABLE [dbo].[IntellectTeam]
ADD CONSTRAINT [PK_IntellectTeam]
    PRIMARY KEY NONCLUSTERED ([Intellects_Intellect_ID], [Teams_Team_ID] ASC);
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

-- Creating foreign key on [Creator_ID] in table 'Game'
ALTER TABLE [dbo].[Game]
ADD CONSTRAINT [FK_AccountGame]
    FOREIGN KEY ([Creator_ID])
    REFERENCES [dbo].[Account]
        ([Account_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountGame'
CREATE INDEX [IX_FK_AccountGame]
ON [dbo].[Game]
    ([Creator_ID]);
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

-- Creating foreign key on [GameGame_ID] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_TeamGame]
    FOREIGN KEY ([GameGame_ID])
    REFERENCES [dbo].[Game]
        ([Game_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamGame'
CREATE INDEX [IX_FK_TeamGame]
ON [dbo].[Teams]
    ([GameGame_ID]);
GO

-- Creating foreign key on [Intellects_Intellect_ID] in table 'IntellectTeam'
ALTER TABLE [dbo].[IntellectTeam]
ADD CONSTRAINT [FK_IntellectTeam_Intellect]
    FOREIGN KEY ([Intellects_Intellect_ID])
    REFERENCES [dbo].[Intellect]
        ([Intellect_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Teams_Team_ID] in table 'IntellectTeam'
ALTER TABLE [dbo].[IntellectTeam]
ADD CONSTRAINT [FK_IntellectTeam_Team]
    FOREIGN KEY ([Teams_Team_ID])
    REFERENCES [dbo].[Teams]
        ([Team_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IntellectTeam_Team'
CREATE INDEX [IX_FK_IntellectTeam_Team]
ON [dbo].[IntellectTeam]
    ([Teams_Team_ID]);
GO

-- Creating foreign key on [GameDetails_ID] in table 'GameDetails'
ALTER TABLE [dbo].[GameDetails]
ADD CONSTRAINT [FK_GameGameDetails]
    FOREIGN KEY ([GameDetails_ID])
    REFERENCES [dbo].[Game]
        ([Game_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------