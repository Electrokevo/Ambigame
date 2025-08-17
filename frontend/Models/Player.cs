using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes.Models;
internal class Player
{
    public string username { get; set; }
    public string password { get; set; }
    public Player(string _username, string _password)
    {
        username = _username;
        password = _password; // Start position
    }
}
