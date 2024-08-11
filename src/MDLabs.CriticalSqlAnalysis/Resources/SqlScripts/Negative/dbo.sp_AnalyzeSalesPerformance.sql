CREATE PROCEDURE dbo.AnalyzeSalesPerformance
    @StartDate VARCHAR(10),
    @EndDate VARCHAR(10)
AS
BEGIN
    -- No input validation

    -- Use global temporary table
    IF OBJECT_ID('tempdb..##GlobalSalesAnalysis') IS NOT NULL
        DROP TABLE ##GlobalSalesAnalysis

    SELECT 
        sp.SalesPersonID,
        p.FirstName + ' ' + p.LastName AS SalesPersonName,
        SUM(soh.TotalDue) AS TotalSales,
        COUNT(*) AS OrderCount,
        AVG(soh.TotalDue) AS AverageSaleAmount,
        (SELECT TOP 1 prod.Name
         FROM Sales.SalesOrderHeader h
         INNER JOIN Sales.SalesOrderDetail d ON h.SalesOrderID = d.SalesOrderID
         INNER JOIN Production.Product prod ON d.ProductID = prod.ProductID
         WHERE h.SalesPersonID = sp.SalesPersonID
           AND h.OrderDate BETWEEN @StartDate AND @EndDate
         GROUP BY prod.Name
         ORDER BY SUM(d.OrderQty) DESC) AS TopProduct
    INTO ##GlobalSalesAnalysis
    FROM 
        Sales.SalesPerson sp
        INNER JOIN Sales.SalesOrderHeader soh ON sp.SalesPersonID = soh.SalesPersonID
        INNER JOIN Person.Person p ON sp.SalesPersonID = p.BusinessEntityID
    WHERE 
        soh.OrderDate BETWEEN @StartDate AND @EndDate
    GROUP BY 
        sp.SalesPersonID, p.FirstName, p.LastName

    -- Return the final result set with a cursor
    DECLARE @SalesPersonID INT, @SalesPersonName NVARCHAR(100), @TotalSales DECIMAL(18, 2), @OrderCount INT, @AverageSaleAmount DECIMAL(18, 2), @TopProduct NVARCHAR(100)
    DECLARE @Rank INT = 0

    DECLARE sales_cursor CURSOR FOR
    SELECT SalesPersonID, SalesPersonName, TotalSales, OrderCount, AverageSaleAmount, TopProduct
    FROM ##GlobalSalesAnalysis
    ORDER BY TotalSales DESC

    OPEN sales_cursor
    FETCH NEXT FROM sales_cursor INTO @SalesPersonID, @SalesPersonName, @TotalSales, @OrderCount, @AverageSaleAmount, @TopProduct

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @Rank = @Rank + 1
        PRINT 'Rank: ' + CAST(@Rank AS VARCHAR(10)) + ', SalesPerson: ' + @SalesPersonName + ', TotalSales: ' + CAST(@TotalSales AS VARCHAR(20))
        FETCH NEXT FROM sales_cursor INTO @SalesPersonID, @SalesPersonName, @TotalSales, @OrderCount, @AverageSaleAmount, @TopProduct
    END

    CLOSE sales_cursor
    DEALLOCATE sales_cursor

    -- No cleanup of global temporary table
END