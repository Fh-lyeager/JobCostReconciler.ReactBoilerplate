namespace JobCostReconciliation.DataContext.Sapphire.Model
{
    public class Lot
    {
        public int LotRID { get; set; }
        public int CommunityRID { get; set; }
        public string LotID { get; set; }
        public virtual Community Community { get; set; }
    }
}
