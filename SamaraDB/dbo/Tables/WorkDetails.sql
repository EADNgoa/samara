CREATE TABLE [dbo].[WorkDetails]
(
	[WorkDetailID] INT NOT NULL PRIMARY KEY IDENTITY,
    [WorkID] INT NULL,
	[ItemID] INT NULL,
    [LabourID] INT NULL,
    [Qty] INT NULL,  
    [Amount] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_Work_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID]), 
    CONSTRAINT [FK_Work_Labour] FOREIGN KEY ([LabourID]) REFERENCES [Labour]([LabourID]), 

    CONSTRAINT [FK_WorkDetails_Work] FOREIGN KEY ([WorkID]) REFERENCES [Work]([WorkID])
)
