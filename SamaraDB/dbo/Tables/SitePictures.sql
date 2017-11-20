CREATE TABLE [dbo].[SitePictures]
(
	[SitePictureID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteID] INT NULL, 
    [Picture] VARCHAR(100) NULL, 
    [UserID] NVARCHAR(100) NULL, 
    [DTime] DATETIME NULL, 
    [Comment] VARCHAR(300) NULL, 
    CONSTRAINT [FK_SitePictures_Site] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID])
)
