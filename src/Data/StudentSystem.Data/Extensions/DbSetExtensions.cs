using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<TEntity> FindAsync<TEntity>(this IDbSet<TEntity> dbset, object id)
            where TEntity : class
        {
            return await ((DbSet<TEntity>)dbset).FindAsync(id);
        }
    }
}
