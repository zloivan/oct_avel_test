using UnityEngine;

namespace EcommerceV2.Domain
{
    public interface IUserContext
    {
        bool IsPreferredCustomer { get; }
    }
    
    public class LocalUserContext : IUserContext
    {
        public bool IsPreferredCustomer => PlayerPrefs.GetInt("IsPreferredCustomer", 0) == 1;
    }
}