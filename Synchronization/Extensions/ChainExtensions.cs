namespace InstantMultiplayer.Synchronization.Extensions
{
    public static class ChainExtensions
    {
        public static T ValueIf<T>(this T value, T compareValue, T newValue)
        {
            return value.Equals(compareValue) ? newValue : value;
        }
    }
}
