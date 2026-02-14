using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoviesCrudApp.Repositories
{
    /// <summary>
    /// Generic Repository Interface for CRUD operations and queries
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        // CRUD Operations
        /// <summary>
        /// Get entity by id
        /// </summary>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Get all entities
        /// </summary>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Get entities with includes
        /// </summary>
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Add new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Update entity
        /// </summary>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Delete entity by id
        /// </summary>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Delete entity
        /// </summary>
        Task<bool> DeleteAsync(T entity);

        // Query Operations
        /// <summary>
        /// Find entities with predicate
        /// </summary>
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find entities with predicate and includes
        /// </summary>
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Find single entity with predicate
        /// </summary>
        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find single entity with predicate and includes
        /// </summary>
        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Check if entity exists
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        // Count and Pagination
        /// <summary>
        /// Get total count of entities
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Get count with predicate
        /// </summary>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get entities with pagination
        /// </summary>
        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Get entities with pagination and filtering
        /// </summary>
        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get entities with pagination, filtering and includes
        /// </summary>
        Task<PaginatedResult<T>> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes);

        // Bulk Operations
        /// <summary>
        /// Add multiple entities
        /// </summary>
        Task<List<T>> AddRangeAsync(List<T> entities);

        /// <summary>
        /// Delete multiple entities
        /// </summary>
        Task<bool> DeleteRangeAsync(List<T> entities);

        /// <summary>
        /// Save changes to database
        /// </summary>
        Task<bool> SaveAsync();
    }

    /// <summary>
    /// Pagination result wrapper
    /// </summary>
    public class PaginatedResult<T> where T : class
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
