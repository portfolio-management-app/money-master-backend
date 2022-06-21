using System;
using ApplicationCore.Entity.Asset;
using ApplicationCore.Entity.Transactions;
using Ardalis.Specification;

namespace ApplicationCore.TransactionAggregate
{
    public class TransactionWithPagingAndTimeSpec : Specification<SingleAssetTransaction>
    {
        public TransactionWithPagingAndTimeSpec(PersonalAsset asset, int? pageNumber, int? pageSize,
            DateTime? startDate, DateTime? endDate, string transactionType)
        {

            if (asset.GetAssetType() == "fund")
            {
                if (transactionType == "all")
                    Query.Where(trans =>
                       trans.ReferentialAssetType == asset.GetAssetType()
                        || trans.DestinationAssetType == asset.GetAssetType());

                if (transactionType == "in")
                {
                    Query.Where(trans =>
                   trans.DestinationAssetType == asset.GetAssetType());

                }
                if (transactionType == "out")
                {
                    Query.Where(trans =>
                    trans.ReferentialAssetType == asset.GetAssetType());
                }
            }
            else
            {

                if (transactionType == "all")
                    Query.Where(trans =>
                        trans.ReferentialAssetId == asset.Id && trans.ReferentialAssetType == asset.GetAssetType()
                        || trans.DestinationAssetId == asset.Id && trans.DestinationAssetType == asset.GetAssetType());

                if (transactionType == "in")
                {
                    Query.Where(trans =>
                    trans.DestinationAssetId == asset.Id && trans.DestinationAssetType == asset.GetAssetType());

                }
                if (transactionType == "out")
                {
                    Query.Where(trans =>
                    trans.ReferentialAssetId == asset.Id && trans.ReferentialAssetType == asset.GetAssetType());
                }
            }
            if (pageNumber is not null && pageSize is not null)
                Query.OrderByDescending(t => t.CreatedAt)
                    .Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

            if (startDate is not null)
                Query.Where(t => t.CreatedAt >= startDate);
            if (endDate is not null)
                Query.Where(t => t.CreatedAt <= endDate);
        }
    }
}