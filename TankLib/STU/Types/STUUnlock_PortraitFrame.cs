// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x6FE91269, 264)]
    public class STUUnlock_PortraitFrame : STU_3021DDED
    {
        [STUField(0x949D9C2A, 224)] // size: 16
        public teStructuredDataAssetRef<ulong> m_949D9C2A;
        
        [STUField(0xA4A66AB6, 240)] // size: 16
        public teStructuredDataAssetRef<ulong> m_rankTexture;
        
        [STUField(0x8F736177, 256)] // size: 4
        public STUPortraitFrameRank m_rank;
        
        [STUField(0x2C01908B, 260)] // size: 2
        public ushort m_level;
        
        [STUField(0x78A2AC5C, 262)] // size: 2
        public ushort m_stars;
    }
}
