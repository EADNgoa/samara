CREATE TABLE [dbo].[Client]
(
	[ClientId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientName] VARCHAR(150) NOT NULL, 
    [Address] VARCHAR(300) NULL
)
