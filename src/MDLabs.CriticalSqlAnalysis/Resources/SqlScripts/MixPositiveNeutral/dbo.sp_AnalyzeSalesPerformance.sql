CREATE PROCEDURE [dbo].[sp_AnalyzeSalesPerformance]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Input validation (good practice)
    IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate > @EndDate
    BEGIN
        THROW 50000, 'Invalid date range provided.', 1;
        RETURN;
    END

    -- Use of table variable instead of temporary table (can be good or bad depending on the size of data)
    DECLARE @SalesAnalysis TABLE (
        SalesPersonID INT,
        TotalSales DECIMAL(18, 2),
        OrderCount INT,
        AverageSaleAmount DECIMAL(18, 2),
        TopProduct NVARCHAR(100)
    );

    -- Populate the table variable with sales data
    INSERT INTO @SalesAnalysis (SalesPersonID, TotalSales, OrderCount, AverageSaleAmount)
    SELECT 
        sp.SalesPersonID,
        SUM(soh.TotalDue) AS TotalSales,
        COUNT(DISTINCT soh.SalesOrderID) AS OrderCount,
        AVG(soh.TotalDue) AS AverageSaleAmount
    FROM 
        Sales.SalesPerson sp
        INNER JOIN Sales.SalesOrderHeader soh ON sp.SalesPersonID = soh.SalesPersonID
    WHERE 
        soh.OrderDate BETWEEN @StartDate AND @EndDate
    GROUP BY 
        sp.SalesPersonID;

    -- Update the table variable with the top-selling product using a correlated subquery (bad practice for performance)
    UPDATE sa
    SET sa.TopProduct = (
        SELECT TOP 1 p.Name
        FROM Sales.SalesOrderHeader soh
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        INNER JOIN Production.Product p ON sod.ProductID = p.ProductID
        WHERE soh.SalesPersonID = sa.SalesPersonID
          AND soh.OrderDate BETWEEN @StartDate AND @EndDate
        GROUP BY p.Name
        ORDER BY SUM(sod.OrderQty) DESC
    )
    FROM @SalesAnalysis sa;

    -- Return the final result set using dynamic SQL (can be a bad practice if not handled carefully)
    DECLARE @SQL NVARCHAR(MAX);
    SET @SQL = N'
    SELECT 
        sa.SalesPersonID,
        p.FirstName + '' '' + p.LastName AS SalesPersonName,
        sa.TotalSales,
        sa.OrderCount,
        sa.AverageSaleAmount,
        sa.TopProduct,
        RANK() OVER (ORDER BY sa.TotalSales DESC) AS SalesRank
    FROM 
        @SalesAnalysis sa
        INNER JOIN Person.Person p ON sa.SalesPersonID = p.BusinessEntityID
    ORDER BY 
        sa.TotalSales DESC;
    ';

    EXEC sp_executesql @SQL, N'@SalesAnalysis TABLE (SalesPersonID INT, TotalSales DECIMAL(18, 2), OrderCount INT, AverageSaleAmount DECIMAL(18, 2), TopProduct NVARCHAR(100))', @SalesAnalysis;

    -- No explicit cleanup needed for table variables
END