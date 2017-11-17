CREATE TABLE [dbo].[WorkDetails]
(
	[WorkDetailID] INT NOT NULL PRIMARY KEY IDENTITY,
    [WorkID] INT NULL,
	[ItemID] INT NULL,
    [Qty] INT NULL, 
	[Rate] DECIMAL(18, 2) NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_Work_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID]), 
    CONSTRAINT [FK_WorkDetails_Work] FOREIGN KEY ([WorkID]) REFERENCES [Work]([WorkID])
)
