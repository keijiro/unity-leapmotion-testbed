namespace Stk
{
    public class Math
    {
        public static bool IsPrime (int number)
        {
            if ((number & 1) == 1) {
                var upto = (int)System.Math.Sqrt (number);
                for (var i = 3; i <= upto; i += 2) {
                    if (number % i == 0)
                        return false;
                }
                return true;
            } else {
                return (number == 2);
            }
        }
    }
}