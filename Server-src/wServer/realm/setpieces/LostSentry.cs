﻿using wServer.realm.worlds;

namespace wServer.realm.setpieces
{
    class LostSentry : ISetPiece
    {
        public int Size { get { return 63; } }

        public void RenderSetPiece(World world, IntPoint pos)
        {
            var proto = world.Manager.Resources.Worlds["LostSentry"];
            SetPieces.RenderFromProto(world, pos, proto);
        }
    }
}
