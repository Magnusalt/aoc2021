class Day21b{
    private (long score, long position) _player1;
    private (long score, long position) _player2;

    private int _dice;
    public Day21b()
    {
        _dice = 1;
        _player1 = (0, 10);
        _player2 = (0, 8);
    }

    public long Run()
    {
        var round = 1;
        while (_player1.score < 1000 && _player2.score < 1000)
        {
            var diceRoll = RollDice();
            if (round % 2 == 0)
            {
                var space = (_player2.position + diceRoll) % 10 == 0 ? 10 : (_player2.position + diceRoll) % 10;
                _player2.score += space;
                _player2.position = space;
            }
            else
            {
                var space = (_player1.position + diceRoll) % 10 == 0 ? 10 : (_player1.position + diceRoll) % 10;
                _player1.score += space;
                _player1.position = space;
            }
            round++;
        }
        return (round - 1) * 3 * Math.Min(_player1.score, _player2.score);
    }

    public long RunB()
    {
        var playerPositions = new Dictionary<(int, int), Dictionary<(int, int), long>>();
        playerPositions[(4, 8)] = new Dictionary<(int, int), long>() { [(0, 0)] = 1 };
        var round = 1;
        long score1 = 0;
        long score2 = 0;
        long matches = 0;
        while (playerPositions.Any(pp => pp.Value.Any(score => score.Key.Item1 < 21 && score.Key.Item1 < 21)))
        {
            var next = new Dictionary<(int, int), Dictionary<(int, int), long>>();

            foreach (var pos in playerPositions)
            {
                foreach (var score in pos.Value)
                {
                    if (score.Key.Item1 < 21 && score.Key.Item2 < 21 && score.Value > 0)
                    {
                        if (round % 2 == 0)
                        {
                            var space1 = (pos.Key.Item2 + 1) % 10 == 0 ? 10 : (pos.Key.Item2 + 1) % 10;
                            var space2 = (pos.Key.Item2 + 2) % 10 == 0 ? 10 : (pos.Key.Item2 + 2) % 10;
                            var space3 = (pos.Key.Item2 + 3) % 10 == 0 ? 10 : (pos.Key.Item2 + 3) % 10;
                            foreach (var space in new[] { space1, space2, space3 })
                            {
                                if (score.Key.Item2 + space >= 21)
                                {
                                    score2 += score.Value;
                                    continue;
                                }
                                matches++;

                                var newScoreKey = (score.Key.Item1, score.Key.Item2 + space);
                                var newPosKey = (pos.Key.Item1, space);
                                if (next.ContainsKey(newPosKey))
                                {
                                    if (next[newPosKey].ContainsKey(newScoreKey))
                                    {
                                        next[newPosKey][newScoreKey] += score.Value;
                                    }
                                    else
                                    {
                                        next[newPosKey].Add(newScoreKey, score.Value);
                                    }
                                }
                                else
                                {
                                    next.Add(newPosKey, new Dictionary<(int, int), long> { [newScoreKey] = score.Value });
                                }
                            }
                        }
                        else
                        {
                            var space1 = (pos.Key.Item1 + 1) % 10 == 0 ? 10 : (pos.Key.Item1 + 1) % 10;
                            var space2 = (pos.Key.Item1 + 2) % 10 == 0 ? 10 : (pos.Key.Item1 + 2) % 10;
                            var space3 = (pos.Key.Item1 + 3) % 10 == 0 ? 10 : (pos.Key.Item1 + 3) % 10;
                            foreach (var space in new[] { space1, space2, space3 })
                            {
                                if (score.Key.Item1 + space >= 21)
                                {
                                    score1 += score.Value;
                                    continue;
                                }
                                matches++;

                                var newScoreKey = (score.Key.Item1 + space, score.Key.Item2);
                                var newPosKey = (space, pos.Key.Item2);
                                if (next.ContainsKey(newPosKey))
                                {
                                    if (next[newPosKey].ContainsKey(newScoreKey))
                                    {
                                        next[newPosKey][newScoreKey] += score.Value;
                                    }
                                    else
                                    {
                                        next[newPosKey].Add(newScoreKey, score.Value);
                                    }
                                }
                                else
                                {
                                    next.Add(newPosKey, new Dictionary<(int, int), long> { [newScoreKey] = score.Value });
                                }
                            }
                        }
                    }
                }
            }
            round++;
            playerPositions = next;
        }
        return 0;
    }
    private int RollDice()
    {
        ;
        var diceRolls = Enumerable.Range(_dice, 3).Select(r => r > 100 ? r - 100 : r);
        _dice = _dice + 3 <= 100 ? _dice + 3 : diceRolls.Last() + 1;
        return diceRolls.Sum();
    }

    
}