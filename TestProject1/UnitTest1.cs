using _91_MatchResult;
using NSubstitute;

namespace TestProject1;

public class Tests
{
    
    private IMatchRepository _matchRepository;
    private MatchService _matchService;
    
    [SetUp]
    public void Setup()
    {
        _matchRepository = Substitute.For<IMatchRepository>();
        _matchService = new MatchService(_matchRepository);
    }

    [Test]
    public void UpdateMatchResult_HomeGoal_HomeScorePlusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.HomeGoal);
        
        Assert.That(actualResult, Is.EqualTo("1:0 (First Half)"));
    }
    
    [Test]
    public void UpdateMatchResult_AwayGoal_AwayScorePlusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.AwayGoal);
        
        Assert.That(actualResult, Is.EqualTo("0:1 (First Half)"));
    }
    
    [Test]
    public void UpdateMatchResult_CancelHomeGoal_HomeScoreMinusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("H");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.CancelHomeGoal);
        
        Assert.That(actualResult, Is.EqualTo("0:0 (First Half)"));
    }
    
    [Test]
    public void UpdateMatchResult_CancelHomeGoalAfterSecondHalf_HomeScoreMinusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("H;");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.CancelHomeGoal);
        
        Assert.That(actualResult, Is.EqualTo("0:0 (Second Half)"));
    }
    
    [Test]
    public void UpdateMatchResult_CancelAwayGoal_AwayScoreMinusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("A");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.CancelAwayGoal);
        
        Assert.That(actualResult, Is.EqualTo("0:0 (First Half)"));
    }
    
    [Test]
    public void UpdateMatchResult_CancelAwayGoalAfterSecondHalf_AwayScoreMinusOne()
    {
        _matchRepository.GetMatchResult(1).Returns("A;");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.CancelAwayGoal);
        
        Assert.That(actualResult, Is.EqualTo("0:0 (Second Half)"));
    }

    [Test]
    public void UpdateMatchResult_NextPeriod_DisplaySecondHalf()
    {
        _matchRepository.GetMatchResult(1).Returns("");
        var actualResult = _matchService.UpdateMatchResult(1, MatchEvent.NextPeriod);
        
        Assert.That(actualResult, Is.EqualTo("0:0 (Second Half)"));
    }
}