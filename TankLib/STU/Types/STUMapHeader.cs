// Generated by TankLibHelper
using TankLib.STU.Types.Enums;

// ReSharper disable All
namespace TankLib.STU.Types
{
    [STU(0x02514B6A, 424)]
    public class STUMapHeader : STUInstance
    {
        [STUField(0x4E87690F, 8)] // size: 16
        public teStructuredDataAssetRef<ulong> m_map;
        
        [STUField(0x38F33424, 24)] // size: 16
        public teStructuredDataAssetRef<ulong> m_baseMap;
        
        [STUField(0x5FF3ACFB, 40, ReaderType = typeof(InlineInstanceFieldReader))] // size: 16
        public STUMapCelebrationOverride[] m_celebrationOverrides;
        
        [STUField(0xD608E9F3, 56)] // size: 16
        public teStructuredDataAssetRef<STUGameMode>[] m_D608E9F3;
        
        [STUField(0xDDC37F3D, 72)] // size: 16
        public teStructuredDataAssetRef<ulong> m_DDC37F3D;
        
        [STUField(0x86C1CFAB, 88)] // size: 16
        public teStructuredDataAssetRef<ulong> m_86C1CFAB;
        
        [STUField(0xC6599DEB, 104)] // size: 16
        public teStructuredDataAssetRef<ulong> m_C6599DEB;
        
        [STUField(0xA0AE2E3E, 120)] // size: 16
        public teStructuredDataAssetRef<ulong> m_musicTease;
        
        [STUField(0x956158FF, 136)] // size: 16
        public teStructuredDataAssetRef<ulong> m_announcerWelcome;
        
        [STUField(0x7F5B54B2, 152)] // size: 16
        public teStructuredDataAssetRef<ulong> m_7F5B54B2;
        
        [STUField(0x762B6796, 168, ReaderType = typeof(EmbeddedInstanceFieldReader))] // size: 16
        public STU_7D6D8405[] m_762B6796;
        
        [STUField(0x506FA8D8, 184)] // size: 16
        public teString m_mapName;
        
        [STUField(0xD7A516EC, 200)] // size: 16
        public teString m_D7A516EC;
        
        [STUField(0xCA7E6EDC, 216)] // size: 16
        public teString m_description;
        
        [STUField(0x5DB91CE2, 232)] // size: 16
        public teStructuredDataAssetRef<ulong> m_displayName;
        
        [STUField(0x1C706502, 248)] // size: 16
        public teStructuredDataAssetRef<ulong> m_1C706502;
        
        [STUField(0xEBCFAD22, 264)] // size: 16
        public teStructuredDataAssetRef<ulong> m_EBCFAD22;
        
        [STUField(0x5AFE2F61, 280)] // size: 16
        public teStructuredDataAssetRef<ulong> m_5AFE2F61;
        
        [STUField(0x8EBADA44, 296)] // size: 16
        public teStructuredDataAssetRef<ulong> m_8EBADA44;
        
        [STUField(0xACB95597, 312)] // size: 16
        public teStructuredDataAssetRef<ulong> m_ACB95597;
        
        [STUField(0x389CB894, 328)] // size: 16
        public teStructuredDataAssetRef<ulong> m_389CB894;
        
        [STUField(0x9386E669, 344)] // size: 16
        public teStructuredDataAssetRef<ulong> m_9386E669;
        
        [STUField(0xEBE72514, 360)] // size: 16
        public teStructuredDataAssetRef<ulong> m_EBE72514;
        
        [STUField(0xD58D0365, 376)] // size: 16
        public teStructuredDataAssetRef<ulong> m_D58D0365;
        
        [STUField(0xAF869CEC, 392)] // size: 16
        public byte[] m_entitySignature;
        
        [STUField(0xA125818B, 408)] // size: 4
        public Enum_668FA6B6 m_A125818B;
        
        [STUField(0x1DD3A0CD, 412)] // size: 4
        public STUMapType m_mapType;
        
        [STUField(0x44D13CC2, 416)] // size: 4
        public int m_44D13CC2;
    }
}
