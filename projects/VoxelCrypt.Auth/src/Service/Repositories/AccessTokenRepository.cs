using Kernel.Models.Entities;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Service.Persistence.Database;

namespace Service.Persistence;

public sealed class AccessTokenRepository(AuthDbContext dbContext) : IAccessTokenRepository
{
    public async Task<IReadOnlyList<AccessToken>> ListByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
        => await dbContext.AccessTokens
            .AsNoTracking()
            .Where(token => token.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<AccessToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        => await dbContext.AccessTokens
            .AsNoTracking()
            .Include(accessToken => accessToken.User)
            .FirstOrDefaultAsync(accessToken => accessToken.Token == token, cancellationToken);

    public async Task UpsertAsync(AccessToken accessToken, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.AccessTokens
            .FirstOrDefaultAsync(candidate => candidate.Token == accessToken.Token, cancellationToken);

        if (existing is null)
        {
            var userExists = await dbContext.Users
                .AnyAsync(user => user.Id == accessToken.UserId, cancellationToken);

            if (!userExists)
            {
                throw new ResourceNotFoundException<User>($"User '{accessToken.UserId.Value}' was not found.");
            }

            if (dbContext.Entry(accessToken.User).State == EntityState.Detached)
            {
                dbContext.Users.Attach(accessToken.User);
            }

            await dbContext.AccessTokens.AddAsync(accessToken, cancellationToken);
        }
        else
        {
            if (existing.UserId != accessToken.UserId)
            {
                throw new InvalidOperationException("Access token cannot be reassigned to a different user.");
            }

            existing.UpdateExpiration(accessToken.ExpiresAt);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.AccessTokens
            .FirstOrDefaultAsync(accessToken => accessToken.Token == token, cancellationToken);

        if (existing is null)
        {
            return;
        }

        dbContext.AccessTokens.Remove(existing);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
