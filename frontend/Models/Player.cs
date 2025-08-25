using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snakes.Models;
internal class Player
{

    private static Player _instance;
    public static Player GetInstance() => _instance;
    public static void SetInstance(Player player) => _instance = player;
    public string username { get; set; }
    public string id { get; set; }

    public override string ToString()
    {
        return $"Username: {username}\nID: {id}";
    }
}
