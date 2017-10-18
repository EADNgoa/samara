CREATE TABLE [dbo].[SupervisorSites]
(
	[UserID] INT NOT NULL, 
    [SiteID] INT NULL, 
    CONSTRAINT [FK_SupervisorSites_Supervisor] FOREIGN KEY ([UserID]) REFERENCES [Supervisor]([UserID]), 
    CONSTRAINT [FK_SupervisorSites_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID])
)
