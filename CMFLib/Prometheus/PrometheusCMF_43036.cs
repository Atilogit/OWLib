﻿using static CMFLib.Helpers;

namespace CMFLib.Prometheus {
    [CMFMetadata(AutoDetectVersion = true, BuildVersions = new uint[] { }, App = CMFApplication.Prometheus)]
    public class PrometheusCMF_43036 : ICMFProvider {
        public byte[] Key(CMFHeaderCommon header, string name, byte[] digest, int length) {
            byte[] buffer = new byte[length];

            uint kidx = Keytable[header.DataCount & 511];
            for (int i = 0; i != length; ++i) {
                buffer[i] = Keytable[SignedMod(kidx, 512)];
                kidx += 3;
            }

            return buffer;
        }

        public byte[] IV(CMFHeaderCommon header, string name, byte[] digest, int length) {
            byte[] buffer = new byte[length];

            uint kidx = (uint) length * header.BuildVersion;
            uint increment = header.BuildVersion * header.DataCount % 7;
            for (int i = 0; i != length; ++i) {
                buffer[i] = Keytable[SignedMod(kidx, 512)];
                kidx += increment;
                buffer[i] ^= digest[SignedMod(kidx - 73, SHA1_DIGESTSIZE)];
            }

            return buffer;
        }

        private static readonly byte[] Keytable = {
            0x95, 0x57, 0x16, 0xDF, 0x02, 0x15, 0x0A, 0xF9, 0x2B, 0x51, 0xE8, 0x0D, 0xCE, 0x47, 0x01, 0x29,
            0xE2, 0x2F, 0x51, 0xD2, 0xD2, 0xBF, 0x1A, 0xD5, 0x3F, 0xDB, 0xA7, 0x6C, 0xC3, 0xCD, 0x2A, 0xAE,
            0x71, 0x6D, 0x4D, 0x87, 0x86, 0x8A, 0x4E, 0xC3, 0x76, 0x0C, 0x49, 0x29, 0x20, 0xB7, 0x2B, 0xE3,
            0x59, 0xC1, 0x9D, 0xF1, 0x07, 0xAB, 0x7D, 0xA9, 0xD9, 0xC8, 0xE2, 0xDB, 0x76, 0xF7, 0x73, 0xBB,
            0xD8, 0x24, 0x4A, 0x90, 0xA0, 0xA9, 0x26, 0xF0, 0x7B, 0xDC, 0xA5, 0x7F, 0xDE, 0x2C, 0xBC, 0x48,
            0xD2, 0x7F, 0x41, 0x3B, 0x9F, 0x85, 0x81, 0x3C, 0xFB, 0x8A, 0x63, 0xFF, 0x77, 0x5C, 0xAB, 0xA7,
            0x59, 0x25, 0xC7, 0x5F, 0xE4, 0x8B, 0x35, 0x60, 0x4B, 0x97, 0xD4, 0x84, 0x54, 0x12, 0x76, 0xF5,
            0x08, 0x56, 0xAF, 0x0A, 0x71, 0xB3, 0xB4, 0x12, 0x79, 0x50, 0x74, 0x26, 0x90, 0x8E, 0xE1, 0xBD,
            0xB4, 0x9D, 0x55, 0xED, 0xC3, 0x8D, 0x72, 0xBB, 0xBC, 0x7C, 0xA3, 0x76, 0x11, 0x39, 0xCB, 0x47,
            0x7E, 0x2C, 0x51, 0x60, 0x7B, 0x67, 0x0D, 0xB6, 0xE3, 0x7C, 0x1E, 0x34, 0x2D, 0x79, 0xD6, 0x44,
            0xE2, 0x5A, 0x23, 0x22, 0xFA, 0x77, 0xB2, 0x00, 0x08, 0x31, 0x51, 0xD5, 0x81, 0x28, 0x5D, 0x20,
            0x47, 0x00, 0x80, 0xAE, 0xD4, 0x03, 0xB9, 0x36, 0x06, 0x70, 0xBE, 0xA3, 0x20, 0xA3, 0xBA, 0xF9,
            0x6A, 0xBF, 0xC8, 0xD7, 0xEE, 0xB4, 0xD3, 0x68, 0x3B, 0x5D, 0xEC, 0x95, 0x01, 0xB7, 0xC5, 0x58,
            0x4B, 0x67, 0x15, 0xBF, 0xDE, 0xBC, 0x2B, 0xA5, 0xB1, 0x8B, 0xEF, 0x1A, 0xC2, 0x01, 0x2D, 0x75,
            0x5D, 0xEC, 0x89, 0x8F, 0xC8, 0xCB, 0x4B, 0x94, 0xC3, 0x2C, 0xD3, 0xE2, 0x82, 0x7E, 0xE6, 0x92,
            0x46, 0x4D, 0xB8, 0x69, 0xB0, 0x38, 0xC4, 0x4C, 0x7E, 0x97, 0xA4, 0x9D, 0x98, 0xB2, 0x78, 0x68,
            0x52, 0x99, 0xCB, 0xDC, 0x56, 0x84, 0x4E, 0x7B, 0x0C, 0x32, 0xD7, 0x08, 0xC1, 0x0F, 0xCC, 0x65,
            0x2A, 0x2B, 0x9E, 0xD6, 0xF9, 0xF4, 0xF7, 0xF4, 0xE8, 0x05, 0x62, 0x70, 0xAF, 0x8D, 0xCF, 0x99,
            0xCD, 0x94, 0x7E, 0x40, 0x6B, 0x56, 0x17, 0xCA, 0x2D, 0x06, 0x7A, 0xD7, 0x23, 0x75, 0x69, 0x56,
            0xBA, 0x1C, 0xD7, 0xE3, 0xCA, 0x0E, 0x0E, 0xB5, 0xC9, 0xB2, 0xFC, 0x9B, 0x92, 0xA7, 0x10, 0x49,
            0x9D, 0x3B, 0x62, 0x98, 0x3E, 0x85, 0xC9, 0xCA, 0x52, 0xF2, 0xD8, 0x19, 0x99, 0x63, 0x0B, 0x56,
            0xE6, 0x76, 0x82, 0xDD, 0xF3, 0x54, 0x82, 0x48, 0x92, 0x6B, 0xBD, 0xC8, 0x36, 0x1D, 0xF9, 0x56,
            0x02, 0x28, 0x6A, 0x9F, 0xEC, 0xE9, 0x36, 0xD8, 0x92, 0xF3, 0x99, 0x94, 0xC3, 0x31, 0xFB, 0xDC,
            0xB6, 0x3A, 0xA3, 0x72, 0x42, 0x7D, 0xA9, 0xA6, 0xE2, 0x3A, 0x58, 0x23, 0x3B, 0x91, 0x3C, 0x00,
            0x89, 0x7C, 0xA0, 0xC9, 0xC8, 0x78, 0xC6, 0x37, 0xF0, 0x47, 0xE6, 0x1E, 0xAA, 0x8B, 0xB4, 0x21,
            0x9D, 0xAF, 0x48, 0x49, 0x31, 0x70, 0x85, 0x62, 0xAE, 0x0A, 0x98, 0x8C, 0xA2, 0x9B, 0x99, 0xBB,
            0x51, 0x77, 0x08, 0x4A, 0x31, 0x2C, 0xA7, 0xF4, 0xFC, 0x75, 0xBD, 0x88, 0x32, 0x73, 0x13, 0xAA,
            0xBC, 0x59, 0x75, 0x30, 0x4B, 0x4D, 0xEF, 0x21, 0x13, 0x0A, 0xDC, 0x11, 0xCB, 0x3B, 0xA3, 0x5E,
            0x5E, 0x4F, 0x83, 0x3D, 0x9E, 0x5A, 0x8F, 0xDC, 0x21, 0x18, 0x3D, 0xD8, 0x6B, 0x4F, 0xA7, 0x52,
            0x53, 0x96, 0x19, 0xA0, 0x5F, 0x0E, 0x1A, 0x52, 0x26, 0x8D, 0xD4, 0xE4, 0xAC, 0x01, 0x1F, 0x3B,
            0xC5, 0x91, 0xD8, 0x83, 0x8F, 0xF4, 0xDE, 0xEE, 0x50, 0xF4, 0x69, 0x1D, 0x84, 0x67, 0x51, 0x1B,
            0x0D, 0x30, 0x61, 0xB2, 0x06, 0x06, 0x92, 0x36, 0x55, 0x1E, 0x46, 0xD1, 0xCB, 0x4E, 0x2C, 0x01
        };
    }
}