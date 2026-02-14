using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// MembershipType Repository Implementation - membership-specific data access logic
    /// </summary>
    public class MembershipTypeRepository : GenericRepository<MembershipType>, IMembershipTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MembershipType>> GetAllMembershipTypesWithCustomersAsync()
        {
            return await _context.MembershipTypes
                .Include(m => m.Customers)
                .ToListAsync();
        }

        public async Task<List<MembershipType>> GetMembershipTypesWithDiscountAsync(decimal discountPercentage)
        {
            return await _context.MembershipTypes
                .Include(m => m.Customers)
                .Where(m => m.DiscountRate > discountPercentage)
                .ToListAsync();
        }
    }
}
