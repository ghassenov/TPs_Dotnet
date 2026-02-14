using System;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Unit of Work Pattern - manages all repositories and transactions
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        ICustomerRepository Customers { get; }
        IGenreRepository Genres { get; }
        IMembershipTypeRepository MembershipTypes { get; }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        Task<bool> SaveAsync();

        /// <summary>
        /// Begin a transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commit the transaction
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        Task RollbackAsync();
    }
}
