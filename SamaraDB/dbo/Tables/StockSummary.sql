CREATE TABLE [dbo].[StockSummary]
(
	[StockSummaryID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Tdate] DATE NULL, 
    [ItemID] INT NULL, 
    [SiteID] INT NULL, 
    [Qty] INT NULL, 
    CONSTRAINT [FK_StockSummary_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID]), 
    CONSTRAINT [FK_StockSummary_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID])
)
