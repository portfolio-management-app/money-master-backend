using System.Collections.Generic;

namespace ApplicationCore.Entity.Asset
{
    public class CustomInterestAssetInfo : BaseEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public List<CustomInterestAsset> CustomInterestAssets { get; set; }
    }
}