namespace BeanMachine
{
    public static class Random
    {
        private static System.Random _seededRandom;

        public static int RandomInt(int min, int max)
        {
            System.Random random = new System.Random();

            return random.Next(min, max);
        }

        public static int RandomSeededInt(int min, int max)
        {
            return _seededRandom.Next(min, max);
        }

        public static float RandomFloat(int min, int max)
        {
            float inter = RandomInt(min, max);

            System.Random random = new System.Random();

            float floa = random.NextSingle();

            return inter + floa;
        }

        public static void SetSeededRandom(int seed)
        {
            _seededRandom = new System.Random(seed);
        }
    }
}
