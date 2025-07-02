using System;
using System.Net.PeerToPeer;

namespace baseballAPI.Models;

public class Player
{
   public int PlayerNumber { get; set; }
   public string? FirstName { get; set; }
   public string? LastName { get; set; }
   public string? Position { get; set; }
   public string? Country { get; set; }
   public Player(int pn, string fn, string ln, string pos, string country)
   {
      PlayerNumber = pn;
      FirstName = fn;
      LastName = ln;
      Position = pos;
      Country = country;
   }

   public Player() { }
   

}
