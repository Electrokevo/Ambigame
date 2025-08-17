using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes.Models;
internal class Player
{
    public string Name { get; set; }
    public string Password { get; set; }
    public Player(string name, string password)
    {
        Name = name;
        Password = password; // Start position
    }
}
