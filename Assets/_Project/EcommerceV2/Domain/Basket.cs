using System;
using System.Collections.Generic;

namespace EcommerceV2.Domain
{
    public class Basket
    {
        public IUserContext User { get;}
        public List<Extent> Contents { get; set; } = new();

        public Basket(IUserContext user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}