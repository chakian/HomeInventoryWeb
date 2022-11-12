using HomeInv.Persistence.Dbo;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeInv.Persistence.Migrations
{
    public partial class MoveSizeUnitId
    {
        private void UpdateSizeIds(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"
UPDATE ItemDefinitions
SET SizeUnitId = (SELECT TOP 1 S.SizeUnitId FROM ItemStocks AS S, ItemDefinitions AS D WHERE S.ItemDefinitionId = D.Id)
WHERE Id IN (SELECT ItemDefinitionId FROM ItemStocks)
");
            migrationBuilder.Sql(
                @"
UPDATE ItemDefinitions
SET SizeUnitId = 1
WHERE Id NOT IN (SELECT ItemDefinitionId FROM ItemStocks)
");
            migrationBuilder.AlterColumn<ItemDefinition>("SizeUnitId", "ItemDefinitions", nullable: false);
        }
    }
}
