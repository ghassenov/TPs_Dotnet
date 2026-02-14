using Microsoft.EntityFrameworkCore;
using MoviesCrudApp.Data;
using MoviesCrudApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Customer Repository Implementation - customer-specific data access logic
    /// </summary>
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersWithMembershipAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetCustomersByMembershipAsync(int membershipTypeId)
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.MembershipTypeId == membershipTypeId)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetNewsletterSubscribersAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.IsSubscribedToNewsletter)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetCustomersWithHighDiscountAsync()
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.MembershipType.DiscountRate > 10)
                .ToListAsync();
        }

        public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        {
            return await _context.Customers
                .Include(c => c.MembershipType)
                .Where(c => c.FirstName.Contains(searchTerm) || 
                           c.LastName.Contains(searchTerm) || 
                           c.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
