using System;
namespace CalendarApp.Droid
{
    public static class ArrayExtension
    {
        public static T[] Shift<T>(this T[] array, int positions)
        {
            T[] copy = new T[array.Length];
            Array.Copy(array, 0, copy, array.Length - positions, positions);
            Array.Copy(array, positions, copy, 0, array.Length - positions);
            return copy;
        }
    }
}
