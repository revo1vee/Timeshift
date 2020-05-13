using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyGame.Domain
{
    public class GameModel
    {
        public Room CurrentRoom { get; set; }
        public Player Player { get; set; }

        public static Keys KeyPressed;
        public GameModel()
        {
            CurrentRoom = new Room(5, 5);
            Player = new Player(2, 2) { CurrentRoom = CurrentRoom };
        }
    }
}
