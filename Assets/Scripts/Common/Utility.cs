namespace Common
{
    public static class Utility
    {
        // Fact that in reference type situation this will populate array with references to single object
        // shouldn't matter, because it will be flyweight type object
        public static void Populate2DArray<T>(T[,] array, T value)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = value;
                }
            }
        }
    }
}