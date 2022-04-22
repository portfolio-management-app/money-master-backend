using System.Collections.Generic;

namespace PublicAPI.Endpoints.Portfolio.PersonalAsset.InterestAsset.CustomerInterestAsset
{
    public class GetAllCustomAssetResponse
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SingleCustomInterestAssetResponse> Assets { get; set; }
    }
}