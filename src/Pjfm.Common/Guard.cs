using System;

namespace Pjfm.Common
{
    public static class Guard
    {
        public static T NotNull<T>(T? value, string name) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return value;
        }

        public static string NotNullOrEmpty(string? value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Value can't be null or an empty string", name);
            }

            return value;
        }
        
        public static Guid NotEmpty(Guid value, string name)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Value can't be an empty GUID", name);
            }

            return value;
        }

        public static TResult IsType<T, TResult>(T value) where T : class
        {
            if (value is TResult result)
            {
                return result;
            }

            throw new ArgumentException("Argument value not of right type.");
        }
    }
}