//  The sole purpose of this script is to add spaces before capital letters,
//  used for formatting Enum's ToString()

using System.Text;

public class SplitCase
{
    //  Add space before capital letters in string (not including the first)
    public static string Split(string pascalCase)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(pascalCase[0]); 

        //  Goes through input and check if each character after the first is capital
        for (int i = 1; i < pascalCase.Length; i++)
        {
            //  Add space before appending character if capital
            if(char.IsUpper(pascalCase[i]))
            {
                stringBuilder.Append(" ");
            }
            stringBuilder.Append(pascalCase[i]);
        }

        return stringBuilder.ToString();
    }
}
