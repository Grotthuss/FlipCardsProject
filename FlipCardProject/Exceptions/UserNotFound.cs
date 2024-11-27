namespace FlipCardProject.Exceptions;

public class UserNotFound: Exception
{
    
        /*public UserNotFound()
        {
        }*/

        public UserNotFound(string message)
            : base(message)
        {
        }

        public UserNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
}