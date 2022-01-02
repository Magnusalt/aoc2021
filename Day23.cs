class Day23
{
    public long Run()
    {
        var amphipods = new List<Amphipod>
        {
            new Amber("A1", 4, 1),
            new Amber("A2", 6, 2),
            new Bronze("B1", 6, 1),
            new Bronze("B2", 8, 2),
            new Copper("C1", 2, 1),
            new Copper("C2", 4, 2),
            new Desert("D1", 2, 2),
            new Desert("D2", 8, 1),
        };
        // var amphipods = new Dictionary<string, Amphipod>
        // {
        //     ["A1"] = new Amber(2, 2),
        //     ["A2"] = new Amber(8, 2),
        //     ["B1"] = new Bronze(2, 1),
        //     ["B2"] = new Bronze(6, 1),
        //     ["C1"] = new Copper(4, 1),
        //     ["C2"] = new Copper(6, 2),
        //     ["D1"] = new Desert(4, 2),
        //     ["D2"] = new Desert(8, 1),
        // };
        var startingSituation = new Situation(amphipods, 0, 2);

        var openSet = new PriorityQueue<Situation, int>();
        openSet.Enqueue(startingSituation, startingSituation.Energy);
        var closedSet = new HashSet<string>();
        while (openSet.TryDequeue(out var current, out _))
        {
            if (!closedSet.Add(current.SituationKey()))
            {
                continue;
            }
            if (current.Amphipods.All(a => a.Location.X == a.Target))
            {
                return current.Energy;
            }

            var possibleSubSituations = current.GetSituations();
            foreach (var situation in possibleSubSituations)
            {
                openSet.Enqueue(situation, situation.Energy);
            }
        }

        return 0;
    }
    public long RunB()
    {
        // var amphipodsTest = new List<Amphipod>
        // {
        //     new Amber("A1", 2, 4),
        //     new Amber("A2", 6, 3),
        //     new Amber("A3", 8, 2),
        //     new Amber("A4", 8, 4),
        //     new Bronze("B1", 2, 1),
        //     new Bronze("B2", 4, 3),
        //     new Bronze("B3", 6, 1),
        //     new Bronze("B4", 6, 2),
        //     new Copper("C1", 4, 1),
        //     new Copper("C2", 4, 2),
        //     new Copper("C3", 6, 4),
        //     new Copper("C4", 8, 3),
        //     new Desert("D1", 2, 2),
        //     new Desert("D2", 2, 3),
        //     new Desert("D3", 4, 4),
        //     new Desert("D4", 8, 1)
        // };

        // var startingSituation = new Situation(amphipodsTest, 0, 4);
        /*  
            #############
            #...........#
            ###C#A#B#D###
              #D#C#B#A#
              #D#B#A#C#
              #D#C#A#B#
              #########
        */

        var amphipods = new List<Amphipod>
        {
            new Amber("A1", 4, 1),
            new Amber("A2", 6, 3),
            new Amber("A3", 6, 4),
            new Amber("A4", 8, 2),
            new Bronze("B1", 4, 3),
            new Bronze("B2", 6, 1),
            new Bronze("B3", 6, 2),
            new Bronze("B4", 8, 4),
            new Copper("C1", 2, 1),
            new Copper("C2", 4, 2),
            new Copper("C3", 4, 4),
            new Copper("C4", 8, 3),
            new Desert("D1", 2, 2),
            new Desert("D2", 8, 1),
            new Desert("D3", 2, 3),
            new Desert("D4", 2, 4),
        };

        var startingSituation = new Situation(amphipods, 0, 4);

        var openSet = new PriorityQueue<Situation, int>();
        openSet.Enqueue(startingSituation, startingSituation.Energy);
        var closedSet = new HashSet<string>();
        var energySpent = new List<long>();
        while (openSet.TryDequeue(out var current, out _))
        {
            if (!closedSet.Add(current.SituationKey()))
            {
                continue;
            }
            if (current.Amphipods.All(a => a.Location.X == a.Target))
            {
                return current.Energy;
            }

            var possibleSubSituations = current.GetSituations();
            foreach (var situation in possibleSubSituations)
            {
                openSet.Enqueue(situation, situation.Energy);
            }
        }

        return 0;
    }
}

class Situation
{
    private const int HallwayLength = 11;
    private int _roomSize;
    public int Energy { get; }
    public Situation(List<Amphipod> amphipods, int energy, int roomSize)
    {
        _roomSize = roomSize;
        Amphipods = amphipods;
        Energy = energy;
        InHallway = amphipods.Where(a => a.Location.Y == 0).ToDictionary(a => a.Location.X, a => a);
        Rooms = new Dictionary<int, Room>
        {
            [2] = new Room(Amphipods.Where(a => a.Location.X == 2).ToList(), 2, roomSize),
            [4] = new Room(Amphipods.Where(a => a.Location.X == 4).ToList(), 4, roomSize),
            [6] = new Room(Amphipods.Where(a => a.Location.X == 6).ToList(), 6, roomSize),
            [8] = new Room(Amphipods.Where(a => a.Location.X == 8).ToList(), 8, roomSize)
        };
    }
    public List<Amphipod> Amphipods { get; }
    public Dictionary<int, Amphipod> InHallway { get; }
    public Dictionary<int, Room> Rooms { get; }

    private List<Amphipod> AmphipodsExceptOne(string name) => Amphipods.Where(amp => amp.Name != name).ToList();

    public string SituationKey()
    {
        return $"{string.Join(';', Amphipods.OrderBy(a => a.Name).Select(a => a.GetLocation()))};{Energy}";
    }

    public IEnumerable<Situation> GetSituations()
    {
        foreach (var (roomIndex, room) in Rooms.Where(r => !r.Value.AllIsHome))
        {
            var amphipod = room.ClosestToHallway;

            var currentX = amphipod.Location.X;
            while (currentX >= 0)
            {
                if (InHallway.ContainsKey(currentX))
                {
                    break;
                }
                if (currentX > 1 && currentX % 2 == 0)
                {
                    currentX--;
                    continue;
                }

                var nextLocations = CreateUpdatedAmphipodsList(amphipod, (currentX, 0));
                yield return new Situation(nextLocations, Energy + (amphipod.Location.Y + (amphipod.Location.X - currentX)) * amphipod.MovingEnergy, _roomSize);
                currentX--;
            }
            currentX = amphipod.Location.X;
            while (currentX < HallwayLength)
            {
                if (InHallway.ContainsKey(currentX))
                {
                    break;
                }
                if (currentX < 10 && currentX % 2 == 0)
                {
                    currentX++;
                    continue;
                }

                var nextLocations = CreateUpdatedAmphipodsList(amphipod, (currentX, 0));
                yield return new Situation(nextLocations, Energy + (amphipod.Location.Y + (currentX - amphipod.Location.X)) * amphipod.MovingEnergy, _roomSize);
                currentX++;
            }
        }
        foreach (var (key, amphipod) in InHallway)
        {
            if (!Rooms[amphipod.Target].CanAccept(amphipod))
            {
                continue;
            }
            if (amphipod.Location.X < amphipod.Target && !InHallway.Any(r => r.Key > amphipod.Location.X && r.Key < amphipod.Target))
            {
                var y = Rooms[amphipod.Target].Depth;
                var nextLocations = CreateUpdatedAmphipodsList(amphipod, (amphipod.Target, y));
                var steps = y + (amphipod.Target - amphipod.Location.X);
                yield return new Situation(nextLocations, Energy + (steps * amphipod.MovingEnergy), _roomSize);
            }
            if (amphipod.Location.X > amphipod.Target && !InHallway.Any(r => r.Key < amphipod.Location.X && r.Key > amphipod.Target))
            {
                var y = Rooms[amphipod.Target].Depth;
                var nextLocations = CreateUpdatedAmphipodsList(amphipod, (amphipod.Target, y));
                var steps = y + (amphipod.Location.X - amphipod.Target);
                yield return new Situation(nextLocations, Energy + (steps * amphipod.MovingEnergy), _roomSize);
            }
        }
    }

    private List<Amphipod> CreateUpdatedAmphipodsList(Amphipod amphipodToMove, (int x, int y) amphipodNewLocation)
    {
        var nextLocations = AmphipodsExceptOne(amphipodToMove.Name).Select(amp => amp.Move(amp.Location.X, amp.Location.Y)).ToList();
        nextLocations.Add(amphipodToMove.Move(amphipodNewLocation.x, amphipodNewLocation.y));
        return nextLocations;
    }
}

class Room
{
    private readonly int _roomSize;

    public Room(List<Amphipod> amphipods, int roomIndex, int roomSize)
    {
        Amphipods = amphipods;
        RoomIndex = roomIndex;
        _roomSize = roomSize;
    }
    public bool CanAccept(Amphipod amphipod)
    {
        return amphipod.Target == RoomIndex && AllIsHome && Amphipods.Count < _roomSize;
    }
    public int RoomIndex { get; set; }
    public List<Amphipod> Amphipods { get; }
    public Amphipod ClosestToHallway => Amphipods.OrderBy(a => a.Location.Y).First();
    public bool AllIsHome => Amphipods.All(a => a.Target == RoomIndex);
    public int Depth => _roomSize - Amphipods.Count;
}

abstract class Amphipod
{
    protected Amphipod(string name, int x, int y)
    {
        Location = (x, y);
        Name = name;
    }
    public string Name { get; }
    public abstract int MovingEnergy { get; }
    public abstract int Target { get; }
    public (int X, int Y) Location { get; }
    public string GetLocation()
    {
        return $"{Location.X},{Location.Y}";
    }

    public abstract Amphipod Move(int x, int y);
}

class Amber : Amphipod
{
    public Amber(string name, int x, int y) : base(name, x, y)
    {
    }

    public override int MovingEnergy => 1;

    public override int Target => 2;

    public override Amber Move(int x, int y)
    {
        return new Amber(Name, x, y);
    }
}
class Bronze : Amphipod
{
    public Bronze(string name, int x, int y) : base(name, x, y)
    {
    }

    public override int MovingEnergy => 10;

    public override int Target => 4;
    public override Bronze Move(int x, int y)
    {
        return new Bronze(Name, x, y);
    }
}
class Copper : Amphipod
{
    public Copper(string name, int x, int y) : base(name, x, y)
    {
    }

    public override int MovingEnergy => 100;

    public override int Target => 6;
    public override Copper Move(int x, int y)
    {
        return new Copper(Name, x, y);
    }
}

class Desert : Amphipod
{
    public Desert(string name, int x, int y) : base(name, x, y)
    {
    }

    public override int MovingEnergy => 1000;

    public override int Target => 8;
    public override Desert Move(int x, int y)
    {
        return new Desert(Name, x, y);
    }
}