using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace BikeWorkshop.Infrastructure.EF.Extensions;

internal static class AddIfNotExistsExtension
{
	public static EntityEntry<T>? AddIfNotExists<T>(
		this DbSet<T> dbSet,T entity,
		Expression<Func<T,bool>>? predicate = null) where T : class, new()
	{
		var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
		return !exists ? dbSet.Add(entity) : null;
	}
}
