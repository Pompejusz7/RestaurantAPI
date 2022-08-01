using System;

namespace RestaurantAPI.Exceptions
{
    public class BadRequestExpcetion : Exception
    {
        public BadRequestExpcetion(string message) : base(message) { }
    }
}
