using Infrastructure;

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
            //for get endpoints
            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                // Skip transaction for GET requests
                await next(context);
                return;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await next(context);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
