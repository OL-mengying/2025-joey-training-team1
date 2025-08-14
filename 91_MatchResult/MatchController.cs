using Microsoft.AspNetCore.Mvc;

namespace _91_MatchResult;

public class MatchController : Controller
{
    private readonly MatchService _matchService;
    public MatchController(MatchService matchService)
    {
        _matchService = matchService;
    }
    
    
    public string QueryMatchResult(int matchId)
    {
        return _matchService.QueryMatchResult(matchId);
    }
    
    public string UpdateMatchResult(int matchId, MatchEvent matchEvent)
    {
        return _matchService.UpdateMatchResult(matchId, matchEvent);
    }
    
}