class Day21
{
    private (long score, long position) _player1;
    private (long score, long position) _player2;

    private int _dice;
    public Day21()
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

    private int RollDice()
    {
        var diceRolls = Enumerable.Range(_dice, 3).Select(r => r > 100 ? r - 100 : r);
        _dice = _dice + 3 <= 100 ? _dice + 3 : diceRolls.Last() + 1;
        return diceRolls.Sum();
    }
    private Dictionary<int, ulong> _possibleRollOutComes = new Dictionary<int, ulong>();
    private ulong player1Victories = 0;
    private ulong player2Victories = 0;
    public ulong RunB()
    {
        for (int x = 1; x <= 3; x++)
        {
            for (int y = 1; y <= 3; y++)
            {
                for (int z = 1; z <= 3; z++)
                {
                    int total = x + y + z;
                    if (_possibleRollOutComes.ContainsKey(total))
                    {
                        _possibleRollOutComes[total] += 1;

                    }
                    else
                    {
                        _possibleRollOutComes.Add(total, 1);
                    }
                }
            }
        }

        RollDiracDie(0, 0, 10, 8, 1, 1);

        return Math.Max(player1Victories, player2Victories);
    }

    private void RollDiracDie(int player1Points, int player2Points, int player1Pos, int player2Pos, int playerTurn, ulong universes)
    {

        if (player1Points > 21 || player2Points > 21)
        {
            return;
        }

        if (playerTurn == 1)
        {
            foreach (var (key, value) in _possibleRollOutComes)
            {
                var pts = (player1Pos + key) % 10 == 0 ? 10 : (player1Pos + key) % 10;
                if (player1Points + pts < 21)
                {
                    RollDiracDie(player1Points + pts, player2Points, pts, player2Pos, 2, (value * universes));
                }
                else
                {
                    player1Victories += universes * value;
                }
            }
        }
        else
        {
            foreach (var (key, value) in _possibleRollOutComes)
            {
                var pts = (player2Pos + key) % 10 == 0 ? 10 : (player2Pos + key) % 10;
                if (player2Points + pts < 21)
                {
                    RollDiracDie(player1Points, player2Points + pts, player1Pos, pts, 1, (value * universes));
                }
                else
                {
                    player2Victories += universes * value;
                }
            }
        }
    }
}