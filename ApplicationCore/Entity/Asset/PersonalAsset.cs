using System;

namespace ApplicationCore.Entity.Asset
{
    public class PersonalAsset: BaseEntity
    {
       public int Id {get; set; }
       public string Name { get; set; }
       public DateTime InputDay { get; set; }
       public double InputMoneyAmount { get; set; }
       public string InputCurrency { get; set; }
       public DateTime LastChanged { get; set; }
       public int UserId { get; set; }
       public User User { get; set; }
       
    }
}