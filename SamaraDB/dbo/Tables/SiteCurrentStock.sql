CREATE TABLE [dbo].[SiteCurrentStock]
(
	[SiteStockID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteID] INT NULL, 
    [ItemID] INT NULL, 
    [Qty] INT NULL, 
    CONSTRAINT [FK_SiteCurrentStock_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SIteID]), 
    CONSTRAINT [FK_SiteCurrentStock_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID])
)
