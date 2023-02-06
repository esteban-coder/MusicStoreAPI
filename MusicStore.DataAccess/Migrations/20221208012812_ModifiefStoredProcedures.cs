using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ModifiefStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER PROCEDURE uspListSales(@StartDate DATE, @EndDate DATE, @Page INT, @PageSize INT)
AS
BEGIN
	
	SELECT 
		S.Id SaleId,
		E.DateEvent,
		G.Name Genre,
		E.ImageUrl,
		E.Title,
		S.OperationNumber,
		C.FullName,
		C.Email,
		S.Quantity,
		S.SaleDate,
		S.Total
	FROM Sale S
	INNER JOIN Customer C ON S.CustomerForeignKey = C.Id
	INNER JOIN [Events] E ON S.ConcertId = E.Id
	INNER JOIN Genre G ON E.GenreId = G.Id
	WHERE S.Status = 1
	AND CAST(S.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
	ORDER BY E.Title
	OFFSET @PAGE ROWS FETCH NEXT @PAGESIZE ROWS ONLY;
	
END

GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER PROCEDURE uspListSales(@StartDate DATE, @EndDate DATE, @Page INT, @PageSize INT)
AS
BEGIN
	
	SELECT 
		S.Id SaleId,
		E.DateEvent,
		G.Name Genre,
		E.ImageUrl,
		E.Title,
		S.OperationNumber,
		C.FullName,
		S.Quantity,
		S.SaleDate,
		S.Total
	FROM Sale S
	INNER JOIN Customer C ON S.CustomerForeignKey = C.Id
	INNER JOIN [Events] E ON S.ConcertId = E.Id
	INNER JOIN Genre G ON E.GenreId = G.Id
	WHERE S.Status = 1
	AND CAST(S.SaleDate AS DATE) BETWEEN @StartDate AND @EndDate
	ORDER BY E.Title
	OFFSET @PAGE ROWS FETCH NEXT @PAGESIZE ROWS ONLY;
	
END

GO");
        }
    }
}
