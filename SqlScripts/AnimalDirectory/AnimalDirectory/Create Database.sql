-- =============================================
-- Create database template
-- =============================================

USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'AnimalDirectoy'
)
DROP DATABASE AnimalDirectoy
GO

CREATE DATABASE AnimalDirectoy
GO

USE AnimalDirectoy
GO

ALTER TABLE [dbo].[TaxonomicRank] DROP CONSTRAINT [FK_TaxonomicRank_TaxonomicRankTypeID]
GO

ALTER TABLE [dbo].[TaxonomicRank] DROP CONSTRAINT [FK_TaxonomicRank_TaxonomicRank]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaxonomicRank]') AND type in (N'U'))
DROP TABLE [dbo].[TaxonomicRank]
GO

IF OBJECT_ID('TaxonomicRankType', 'U') IS NOT NULL
  DROP TABLE TaxonomicRankType
GO

CREATE TABLE TaxonomicRankType
(
	TaxonomicRankTypeID int IDENTITY (1,1) NOT NULL, 
	Name NVARCHAR(256) NOT NULL,
	NameFr NVARCHAR(256) NOT NULL,
	ParentTaxonomicRankTypeID int NULL,
    CONSTRAINT PkTaxonomicRankTypeID PRIMARY KEY (TaxonomicRankTypeID),
	CONSTRAINT FK_TaxonomicRank_ParentTaxonomicRankTypeID FOREIGN KEY (ParentTaxonomicRankTypeID)
        REFERENCES dbo.TaxonomicRankType (TaxonomicRankTypeID)        
        ON DELETE NO ACTION 
		ON UPDATE NO ACTION
)
GO


CREATE TABLE [dbo].[TaxonomicRank](
	[TaxonomicRankID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[TaxonomicRankTypeID] [int] NOT NULL,
	[ParentTaxonomicRankID] [int] NULL,
 CONSTRAINT [PkTaxonomicRankID] PRIMARY KEY CLUSTERED 
(
	[TaxonomicRankID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaxonomicRank]  WITH CHECK ADD  CONSTRAINT [FK_TaxonomicRank_TaxonomicRank] FOREIGN KEY([ParentTaxonomicRankID])
REFERENCES [dbo].[TaxonomicRank] ([TaxonomicRankID])
GO

ALTER TABLE [dbo].[TaxonomicRank] CHECK CONSTRAINT [FK_TaxonomicRank_TaxonomicRank]
GO

ALTER TABLE [dbo].[TaxonomicRank]  WITH CHECK ADD  CONSTRAINT [FK_TaxonomicRank_TaxonomicRankTypeID] FOREIGN KEY([TaxonomicRankTypeID])
REFERENCES [dbo].[TaxonomicRankType] ([TaxonomicRankTypeID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TaxonomicRank] CHECK CONSTRAINT [FK_TaxonomicRank_TaxonomicRankTypeID]
GO

USE [AnimalDirectoy]
GO
--TOP
INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Domain'
		   ,'Domaine'
           ,NULL) 

DECLARE @DomainId INT;
SELECT @DomainId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Domain';

INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Kingdom'
		   ,'R�gne'
           ,@DomainId) 

DECLARE @KingdomId INT;
SELECT @KingdomId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Kingdom';


INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Phylum'
		   ,'Embranchement'
           ,@KingdomId) 

DECLARE @PhylumId INT;
SELECT @PhylumId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Phylum';

INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Class'
		   ,'Classe'
           ,@PhylumId) 


DECLARE @ClassId INT;
SELECT @ClassId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Class';

INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Order'
		   ,'Ordre'
           ,@ClassId) 


DECLARE @OrderId INT;
SELECT @OrderId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Order';


INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Family'
		   ,'Famille'
           ,@OrderId) 


DECLARE @FamilyId INT;
SELECT @FamilyId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Family';

INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Genus'
		   ,'Genre'
           ,@FamilyId) 


DECLARE @GenusId INT;
SELECT @GenusId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Genus';

INSERT INTO [dbo].[TaxonomicRankType]
           ([Name]
		   ,[NameFr]
           ,[ParentTaxonomicRankTypeID])
     VALUES
           ('Esp�ce'
		   ,'Species'
           ,@GenusId) 


DECLARE @SpeciesId INT;
SELECT @SpeciesId = [TaxonomicRankTypeID] FROM [dbo].[TaxonomicRankType] WHERE [Name]='Species';

GO