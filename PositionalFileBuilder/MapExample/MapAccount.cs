using PositionalFileBuilder.BlockBuilder;
using PositionalFileBuilder.BlockBuilder.Enums;

namespace PositionalFileBuilder.MapExample
{   
    public class MapAccount : Mapper<Account>
    {
        public MapAccount()
        {
            TotalSize = 86;

            Map(f => f.Operation).Position("1-20").Filler('_');
            Map(f => f.OperationDate).Position("21-39");
            Map(f => f.TransactionType).Position("40-48").Padding(JustifiedEnum.RightJustified);
            MapFiller().Position("49-50").Filler('|');
            Map(f => f.Amount).Position("51-60");
            MapFiller().Position("61-62").Filler('|');
            Map(f => f.Fee).Position("63-70");
            MapFiller().Position("71");
            Map(f => f.Discount).Position("72-86");

            MapCheck();
        }        
    }    
}
