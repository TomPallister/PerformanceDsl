using System.Text.RegularExpressions;

namespace FirstOneTo.Authentication
{
    public class PasswordAdvisor
    {
        public static PasswordScore CheckStrength(string password)
        {
            int score = 1;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.IsMatch(password, @"[0-9]+(\.[0-9][0-9]?)?", RegexOptions.ECMAScript))
                //number only //"^\d+$" if you need to match more than one digit.
                score++;
            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript))
                //both, lower and upper case
                score++;
            if (Regex.IsMatch(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript)) //^[A-Z]+$
                score++;
            //you can get higher than 5 so this handles that by returning VeryStrong.
            if (score > 5)
            {
                return PasswordScore.VeryStrong;
            }
            return (PasswordScore) score;
        }
    }
}