using K4os.Compression.LZ4.Internal;
using WebAPITest.Models.DB;

namespace WebAPITest.Services;

public class FollowService
{
    private readonly FilmplattformContext _db;

    public FollowService(FilmplattformContext db)
    {
        _db = db;
    }

    public async Task<(bool, string)> Follow(Member follower, Member following)
    {
        
        if (follower.Id == following.Id)
            return (false, "You cannot follow yourself");

        if (_db.Followings.Any(x => x.FollowerId == follower.Id && x.FollowingId == following.Id))
            return (false, "You are already following this user");
        
        follower.FollowingFollowers.Add(new Following
        {
            Follower = follower,
            FollowingNavigation = following
        });

        await _db.SaveChangesAsync();

        return (true, string.Empty);
    }

    public async Task<(bool, string)> UnFollow(Member follower, Member following)
    {
        var connection = _db.Followings.FirstOrDefault(x => x.Follower == follower && x.FollowingNavigation == following);

        if (connection != null)
        {
            _db.Followings.Remove(connection);
            await _db.SaveChangesAsync();
            return (true, string.Empty);

        }

        return (false, "You are not following this user");
    }
}