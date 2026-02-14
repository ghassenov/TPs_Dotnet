using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// MembershipType Repository Interface - extends generic repository with membership-specific operations
    /// </summary>
    public interface IMembershipTypeRepository : IGenericRepository<MembershipType>
    {
        /// <summary>
        /// Get all membership types with their customers included
        /// </summary>
        Task<List<MembershipType>> GetAllMembershipTypesWithCustomersAsync();

        /// <summary>
        /// Get membership types with discount > specific percentage
        /// </summary>
        Task<List<MembershipType>> GetMembershipTypesWithDiscountAsync(decimal discountPercentage);
    }
}
