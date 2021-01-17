using System;
using System.Collections.Generic;
using UnityEngine;

using PlayerControl = FFGALNAPKCD;

namespace AnonymousImpostorsMod.API
{
    public class PlayerController
    {
        public PlayerControl PlayerControl { get; }
        public PlayerController(PlayerControl playerControl)
        {
            PlayerControl = playerControl;
        }

        public PlayerData PlayerData
        {
            get
            {
                return new PlayerData(this);
            }
        }
        public uint NetId
        {
            get
            {
                return PlayerControl.NetId;
            }
            set
            {
                PlayerControl.NetId = value;
            }
        }
        public Vector2 Position
        {
            get
            {
                return PlayerControl.GetTruePosition();
            }
        }

        public bool Equals(PlayerController other)
        {
            return NetId == other.NetId;
        }
        public static PlayerController GetLocalPlayer()
        {
            return new PlayerController(PlayerControl.LocalPlayer);
        }

        public static List<PlayerController> GetAllPlayers()
        {
            List<PlayerController> allPlayers = new List<PlayerController>();

            foreach (var playerControl in PlayerControl.AllPlayerControls)
                allPlayers.Add(new PlayerController(playerControl));

            return allPlayers;
        }

        public static PlayerController getPlayerById(byte id)
        {
            var allPlayers = GetAllPlayers();
            foreach (var player in allPlayers)
            {
                if (player.PlayerControl.PlayerId == id)
                    return player;
            }
            return null;
        }

        public static double distBeetweenPlayers(PlayerController first, PlayerController second)
        {
            return Math.Sqrt(Math.Pow(second.Position.x - first.Position.x, 2) + Math.Pow(second.Position.y - first.Position.y, 2));
        }
    }
}
