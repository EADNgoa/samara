CREATE TABLE [dbo].[Item]
(
	[ItemID] INT NOT NULL PRIMARY KEY IDENTITY,
	[ItemName] VARCHAR(100) NULL, 
    [UnitID] INT NULL, 
    [ReorderLevel] INT NULL, 
    [TaxPerc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_Item_Units] FOREIGN KEY ([UnitID]) REFERENCES [Units]([UnitID])
)
