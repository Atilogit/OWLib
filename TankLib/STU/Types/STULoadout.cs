// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x07A0E32F, 152)]
    public class STULoadout : STUInstance
    {
        [STUField(0xB48F1D22, 8)] // size: 16
        public teStructuredDataAssetRef<ulong> m_name;
        
        [STUField(0xCA7E6EDC, 24)] // size: 16
        public teStructuredDataAssetRef<ulong> m_description;
        
        [STUField(0xFC33191B, 40)] // size: 16
        public teStructuredDataAssetRef<ulong> m_logicalButton;
        
        [STUField(0x9290B942, 56)] // size: 16
        public teStructuredDataAssetRef<ulong> m_9290B942;
        
        [STUField(0x3CD6DC1E, 72)] // size: 16
        public teStructuredDataAssetRef<ulong> m_texture;
        
        [STUField(0xC8D38D7B, 88)] // size: 16
        public teStructuredDataAssetRef<ulong> m_infoMovie;
        
        [STUField(0x7E3ED979, 104)] // size: 16
        public teStructuredDataAssetRef<STUTargetTag>[] m_7E3ED979;
        
        [STUField(0xB1124918, 120)] // size: 16
        public teStructuredDataAssetRef<ulong>[] m_B1124918;
        
        [STUField(0x2C54AEAF, 136)] // size: 4
        public LoadoutCategory m_category;
        
        [STUField(0x0E679979, 140)] // size: 4
        public int m_0E679979 = -1;
        
        [STUField(0xCF86B024, 144)] // size: 4
        public int m_CF86B024 = -1;
    }
}
