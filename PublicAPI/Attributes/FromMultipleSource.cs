using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PublicAPI.Attributes
{
    public sealed class FromMultipleSource: Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource { get; }
            = CompositeBindingSource.Create
                (new[] { BindingSource.Path, BindingSource.Query},
                    nameof(FromMultipleSource));
    }
}