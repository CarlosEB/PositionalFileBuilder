using PositionalFileBuilder.BlockBuilder;
using PositionalFileBuilder.BlockBuilder.Enums;

namespace PositionalFileBuilder.MapExample
{   
    public class MapAccount : Mapper<Account>
    {
        public MapAccount()
        {
            TotalSize = 76;

            Map(f => f.Operation).Position("1-20").Filler('_');
            Map(f => f.OperationDate.ToShortDateString()).Position("21-27");
            Map(f => f.TransactionType).Position("28-38").Padding(JustifiedEnum.RightJustified);
            MapFiller().Position("39-40");
            Map(f => f.Amount).Position("41-50");
            MapFiller().Position("51");
            Map(f => f.Fee).Position("52-60");
            MapFiller().Position("61-65");
            Map(f => f.Discount).Position("66-76");

            MapCheck();
        }        
    }    
}
