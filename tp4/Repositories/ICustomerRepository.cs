using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Customer Repository Interface - extends generic repository with customer-specific operations
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        /// <summary>
        /// Get all customers with their membership type included
        /// </summary>
        Task<List<Customer>> GetAllCustomersWithMembershipAsync();

        /// <summary>
        /// Get customers by membership type
        /// </summary>
        Task<List<Customer>> GetCustomersByMembershipAsync(int membershipTypeId);

        /// <summary>
        /// Get customers subscribed to newsletter
        /// </summary>
        Task<List<Customer>> GetNewsletterSubscribersAsync();

        /// <summary>
        /// Get customers with high discount (> 10%)
        /// </summary>
        Task<List<Customer>> GetCustomersWithHighDiscountAsync();

        /// <summary>
        /// Search customers by name or email
        /// </summary>
        Task<List<Customer>> SearchCustomersAsync(string searchTerm);
    }
}
