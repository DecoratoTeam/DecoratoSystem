using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DecorteeSystem.MiddleWare
{
    public class TransactionMiddleware : IMiddleware
    {
        private readonly DecorteeDbContext _context;

        public TransactionMiddleware(DecorteeDbContext context)
        {
            _context = context;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Skip transaction for GET requests
            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await next(context);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
