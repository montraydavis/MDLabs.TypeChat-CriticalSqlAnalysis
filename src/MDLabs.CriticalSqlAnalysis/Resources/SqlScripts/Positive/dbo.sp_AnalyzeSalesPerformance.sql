CREATE PROCEDURE [dbo].[sp_AnalyzeSalesPerformance]
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate input parameters
    IF @StartDate IS NULL OR @EndDate IS NULL OR @StartDate > @EndDate
    BEGIN
        THROW 50000, 'Invalid date range provided.', 1;
        RETURN;
    END

    -- Create temporary table to store intermediate results
    CREATE TABLE #SalesAnalysis (
        SalesPersonID INT,
        TotalSales DECIMAL(18, 2),
        OrderCount INT,
        AverageSaleAmount DECIMAL(18, 2),
        TopProduct NVARCHAR(100)
    );

    -- Populate the temporary table with sales data
    INSERT INTO #SalesAnalysis (SalesPersonID, TotalSales, OrderCount, AverageSaleAmount)
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

    -- Update the temporary table with the top-selling product for each salesperson
    WITH TopProducts AS (
        SELECT 
            soh.SalesPersonID,
            p.Name AS ProductName,
            SUM(sod.OrderQty) AS TotalQuantity,
            ROW_NUMBER() OVER (PARTITION BY soh.SalesPersonID ORDER BY SUM(sod.OrderQty) DESC) AS Rank
        FROM 
            Sales.SalesOrderHeader soh
            INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
            INNER JOIN Production.Product p ON sod.ProductID = p.ProductID
        WHERE 
            soh.OrderDate BETWEEN @StartDate AND @EndDate
        GROUP BY 
            soh.SalesPersonID, p.Name
    )
    UPDATE sa
    SET sa.TopProduct = tp.ProductName
    FROM #SalesAnalysis sa
    INNER JOIN TopProducts tp ON sa.SalesPersonID = tp.SalesPersonID
    WHERE tp.Rank = 1;

    -- Return the final result set
    SELECT 
        sa.SalesPersonID,
        p.FirstName + ' ' + p.LastName AS SalesPersonName,
        sa.TotalSales,
        sa.OrderCount,
        sa.AverageSaleAmount,
        sa.TopProduct,
        RANK() OVER (ORDER BY sa.TotalSales DESC) AS SalesRank
    FROM 
        #SalesAnalysis sa
        INNER JOIN Person.Person p ON sa.SalesPersonID = p.BusinessEntityID
    ORDER BY 
        sa.TotalSales DESC;

    -- Clean up
    DROP TABLE #SalesAnalysis;
END