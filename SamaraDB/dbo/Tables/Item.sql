CREATE TABLE [dbo].[Item]
(
	[ItemID] INT NOT NULL PRIMARY KEY IDENTITY,
	[ItemName] VARCHAR(100) NULL, 
    [UnitID] INT NULL, 
    [ReorderLevel] DECIMAL(18, 2) NULL, 
	[Rate] DECIMAL(18, 2) NULL, 
    [TaxPerc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_Item_Units] FOREIGN KEY ([UnitID]) REFERENCES [Units]([UnitID])
)
