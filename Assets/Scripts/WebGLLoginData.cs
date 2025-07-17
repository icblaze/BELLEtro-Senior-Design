// This Script helps format the login data for sending to the database
// Developers:
// Robert Morris (momomonkeyman)
// Original VirtuELLE Mentor team

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This class allows React to send multiple variables to the game instead of just the JWT upon login. 
public class WebGLLoginData
{
    public string jwt { get; set; }
    public int userID { get; set; }
}
