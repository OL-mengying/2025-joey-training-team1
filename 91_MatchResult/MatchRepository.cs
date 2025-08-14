namespace _91_MatchResult;

public interface IMatchRepository
{
    string GetMatchResult(int matchId);

    void UpdateMatchResult(int matchId, string matchResult);
}