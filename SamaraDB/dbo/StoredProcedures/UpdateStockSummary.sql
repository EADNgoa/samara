 CREATE PROCEDURE [dbo].[UpdateStockSummary]
	@date date

As
   INSERT INTO StockSummary(Tdate,ItemID,SiteID,Qty)
	SELECT getdate(),ItemID,SiteID,Qty
	from SiteCurrentStock
	Group By ItemID,SiteID,Qty
RETURN  

