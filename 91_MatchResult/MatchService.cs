namespace _91_MatchResult;

public enum MatchEvent
{
    HomeGoal,
    AwayGoal,
    NextPeriod,
    CancelHomeGoal,
    CancelAwayGoal,
}


public class MatchService
{
    private readonly IMatchRepository _matchRepository;
    
    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }
    
   
    public string QueryMatchResult(int matchId)
    {
        var matchResult = _matchRepository.GetMatchResult(matchId);
        
        return GetDisplayResult(matchResult);
    }
    
    private string GetDisplayResult(string matchResult)
    {
        var homeScore = matchResult.Count(c => c == 'H');
        var awayScore = matchResult.Count(c => c == 'A');
        var period = matchResult.Contains(';') ? "Second" : "First";
        
        return $"{homeScore}:{awayScore} ({period} Half)";
    }
    
    public string UpdateMatchResult(int matchId, MatchEvent matchEvent)
    {
        var matchResult = _matchRepository.GetMatchResult(matchId);

        switch (matchEvent)
        {
            case MatchEvent.HomeGoal:
                matchResult += "H";
                break;
            case MatchEvent.AwayGoal:
                matchResult += "A";
                break;
            case MatchEvent.NextPeriod:
                matchResult += ";";
                break;
            case MatchEvent.CancelHomeGoal:
                if (matchResult.EndsWith("H"))
                {
                    matchResult = matchResult.Remove(matchResult.Length - 1);
                }
                else if (matchResult.EndsWith("H;"))
                {
                    matchResult = matchResult.Remove(matchResult.Length - 2) + ";";
                }
                else
                {
                    throw new UpdateMatchResultException()
                    {
                        MatchEvent = matchEvent,
                        OriginalMatchResult = matchResult
                    };
                }
                break;
            case MatchEvent.CancelAwayGoal:
                if (matchResult.EndsWith("A"))
                {
                    matchResult = matchResult.Remove(matchResult.Length - 1);
                }
                else if (matchResult.EndsWith("A;"))
                {
                    matchResult = matchResult.Remove(matchResult.Length - 2) + ";";
                }
                else
                {
                    throw new UpdateMatchResultException()
                    {
                        MatchEvent = matchEvent,
                        OriginalMatchResult = matchResult
                    };
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchEvent), matchEvent, null);
        }
        
        _matchRepository.UpdateMatchResult(matchId, matchResult);
        return GetDisplayResult(matchResult);
    }
}

public class UpdateMatchResultException : Exception
{
    public MatchEvent MatchEvent { get; set; }
    public string OriginalMatchResult { get; set; } = null!;
}