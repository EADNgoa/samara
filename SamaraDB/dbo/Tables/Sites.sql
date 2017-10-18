CREATE TABLE [dbo].[Sites]
(
	[SiteID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProjectID] INT NULL, 
    [SiteName] VARCHAR(150) NULL, 
    CONSTRAINT [FK_Sites_Project] FOREIGN KEY ([ProjectID]) REFERENCES [Project]([ProjectID])
)
