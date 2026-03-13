using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Service.Persistence.Database;

namespace Service.Persistence;

public sealed class UserRepository(AuthDbContext dbContext) : IUserRepository
{
    public async Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken = default)
        => await dbContext.Users
            .Include(user => user.AccessTokens)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await dbContext.Users
            .Include(user => user.AccessTokens)
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        => await dbContext.Users
            .Include(user => user.AccessTokens)
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Username == username, cancellationToken);

    public async Task UpsertAsync(User user, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Users
            .FirstOrDefaultAsync(candidate => candidate.Id == user.Id, cancellationToken);

        if (existing is null)
        {
            await dbContext.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            if (!string.Equals(existing.Username, user.Username, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Username cannot be changed once created.");
            }

            existing.UpdatePassword(user.PasswordHash);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Users
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

        if (existing is null)
        {
            return;
        }

        dbContext.Users.Remove(existing);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
