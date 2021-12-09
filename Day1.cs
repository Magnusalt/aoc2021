class Day1
{
    private readonly int[] input;

    public Day1(string[] input)
    {
        this.input = input.Select(s=>int.Parse(s)).ToArray();
    }

    public int Run(){
        var totalIncrease = 0;

        for (var i = 1; i < input.Length; i++)
        {
            if(input[i] > input[i-1]){
                totalIncrease++;
            }
        }

        return totalIncrease;
    }
    public int RunB(){
        var totalIncrease = 0;
        var prevSlidingSum = 0;
        for (var i = 2; i < input.Length; i++)
        {
            var slidingSum = 0;
            for (int j = 0; j < 3; j++)
            {
                slidingSum+=input[i-j];
            }
            if(slidingSum > prevSlidingSum){
                totalIncrease++;
            }
            prevSlidingSum = slidingSum;
        }

        return totalIncrease - 1;
    }
}