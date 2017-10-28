CREATE TABLE [dbo].[SupervisorSites]
(
	[UserID] nvarchar(128) NOT NULL, 
    [SiteID] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([UserID] ASC, [SiteID] ASC),

    CONSTRAINT [FK_SupervisorSites_AspNetUsers] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_SupervisorSites_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID]), 
    
)
