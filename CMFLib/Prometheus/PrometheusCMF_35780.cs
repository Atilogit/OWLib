﻿using static CMFLib.Helpers;

namespace CMFLib.Prometheus {
    [CMFMetadata(AutoDetectVersion = true, BuildVersions = new uint[] { }, App = CMFApplication.Prometheus)]
    public class PrometheusCMF_35780 : ICMFProvider {
        public byte[] Key(CMFHeaderCommon header, string name, byte[] digest, int length) {
            byte[] buffer = new byte[length];

            uint kidx = 193;
            const uint increment = 319;
            for (int i = 0; i != length; ++i) {
                buffer[i] = Keytable[kidx % 512];
                kidx -= increment;
            }

            return buffer;
        }

        public byte[] IV(CMFHeaderCommon header, string name, byte[] digest, int length) {
            byte[] buffer = new byte[length];

            uint kidx = Keytable[header.DataCount & 511];
            for (int i = 0; i != length; ++i) {
                kidx += header.EntryCount + digest[header.EntryCount % SHA1_DIGESTSIZE];
                buffer[i] = digest[kidx % SHA1_DIGESTSIZE];
            }

            return buffer;
        }

        private static readonly byte[] Keytable = {
            0x80, 0x8A, 0x26, 0xFC, 0x2E, 0xCC, 0x67, 0xEC, 0x9D, 0xEC, 0x33, 0xEC, 0xCA, 0xA8, 0x86, 0x38,
            0x54, 0x3F, 0x9E, 0xD3, 0x5A, 0xC4, 0xDC, 0x67, 0xA5, 0xE0, 0xB5, 0x06, 0x8D, 0xD7, 0xED, 0x5F, 
            0x4C, 0xE9, 0xF3, 0x28, 0x05, 0x19, 0x2E, 0xAF, 0x55, 0x15, 0x85, 0x34, 0x82, 0x82, 0xEA, 0xC3, 
            0xD6, 0x34, 0xC0, 0x74, 0xD2, 0xDE, 0x40, 0x8A, 0x56, 0x04, 0x7A, 0x50, 0x7F, 0x65, 0x68, 0xF2, 
            0x00, 0x0C, 0x6F, 0xEB, 0x11, 0x20, 0xE8, 0xDE, 0x03, 0x9E, 0xF4, 0x54, 0x09, 0xF5, 0xC1, 0x1A, 
            0x4A, 0x8C, 0x90, 0xD6, 0x2B, 0x39, 0xD6, 0x4A, 0x8B, 0x0A, 0x77, 0x7C, 0x40, 0x6C, 0xED, 0xD1, 
            0x19, 0xE0, 0x40, 0x48, 0x40, 0x6B, 0xF9, 0x25, 0x48, 0xD5, 0xCC, 0x99, 0x21, 0x95, 0x90, 0x9B, 
            0x81, 0xD6, 0xD9, 0x4D, 0xB4, 0xF2, 0xAD, 0x65, 0xE9, 0x21, 0x81, 0x33, 0x7D, 0x99, 0x3F, 0x96, 
            0xEE, 0x66, 0x15, 0xBB, 0x6E, 0x2D, 0xE8, 0xE7, 0x68, 0x7A, 0xA2, 0x47, 0x8B, 0x65, 0xC4, 0x38, 
            0xC1, 0xDE, 0x17, 0x75, 0x3F, 0x9A, 0x4F, 0x4F, 0x4C, 0x26, 0xDD, 0x45, 0x26, 0x7D, 0x46, 0x9D,
            0x92, 0x54, 0xDE, 0x22, 0x39, 0xDA, 0x7A, 0x50, 0x46, 0x78, 0x80, 0x4A, 0x12, 0x2E, 0x2C, 0x4D, 
            0x5F, 0x50, 0x8A, 0xB3, 0x2D, 0x7D, 0x74, 0x55, 0x8C, 0xF7, 0x69, 0xC0, 0x6C, 0x3E, 0x97, 0x38, 
            0xED, 0x20, 0x42, 0x2C, 0xA3, 0x00, 0xFF, 0xB7, 0x33, 0x0F, 0xF7, 0xB2, 0x84, 0x74, 0x7C, 0x31, 
            0xC3, 0x0B, 0x61, 0xCF, 0x68, 0x85, 0x8A, 0x59, 0x27, 0x1A, 0x6D, 0x5A, 0x21, 0xB5, 0x1D, 0x4B, 
            0x74, 0xD9, 0x5D, 0x86, 0x24, 0x03, 0x03, 0x61, 0x3E, 0x26, 0x06, 0x4C, 0xEA, 0xC8, 0xB1, 0x95, 
            0x03, 0xEE, 0xA7, 0x63, 0x87, 0x76, 0x6C, 0x87, 0xAF, 0xDC, 0xFB, 0x8C, 0x1A, 0xFC, 0x9A, 0xF0, 
            0xFF, 0x20, 0x47, 0x0B, 0xEC, 0xE1, 0x53, 0x81, 0x4B, 0xCD, 0xCE, 0x3C, 0x80, 0xC3, 0x1F, 0x57, 
            0xFC, 0xDB, 0x63, 0x9D, 0x5E, 0x53, 0x3F, 0xAC, 0x45, 0xA7, 0xB0, 0x13, 0xE2, 0x4D, 0x8B, 0x0F, 
            0xB1, 0xC3, 0x67, 0x3F, 0x80, 0xD0, 0xFF, 0xE6, 0x7A, 0x13, 0xEE, 0x87, 0x74, 0xC3, 0x31, 0xCF,
            0x85, 0xF1, 0x46, 0x52, 0x3D, 0x5B, 0x1E, 0xB6, 0x7C, 0xBB, 0x57, 0x58, 0x23, 0x01, 0x9D, 0xC1, 
            0x39, 0xD0, 0xC5, 0xD7, 0x02, 0x2E, 0x53, 0xBD, 0xAB, 0x22, 0x75, 0x78, 0x80, 0xAE, 0xAD, 0x42, 
            0xED, 0xBB, 0x74, 0xF4, 0x09, 0x3F, 0x60, 0x3E, 0x54, 0xF8, 0xA1, 0x12, 0xA4, 0xE2, 0xE1, 0x14, 
            0xD7, 0x2E, 0x78, 0x9F, 0xB2, 0x33, 0x80, 0x08, 0xFA, 0x76, 0xAB, 0x1C, 0xEE, 0x8E, 0x1F, 0x04, 
            0xD2, 0x01, 0xAF, 0x9A, 0x0E, 0xF1, 0xC5, 0x1F, 0x26, 0x0F, 0x11, 0xF4, 0x23, 0xD6, 0x1F, 0xB5, 
            0x79, 0xF7, 0x5D, 0x54, 0xC6, 0x85, 0xE0, 0xDE, 0x08, 0x5A, 0x62, 0x4B, 0x7B, 0x04, 0xB6, 0x1A, 
            0x3A, 0x65, 0xEB, 0xC2, 0xD2, 0x1E, 0xAE, 0x98, 0x30, 0x0E, 0xB7, 0x8A, 0x7A, 0xE2, 0x5A, 0x89, 
            0x9C, 0x9D, 0x57, 0x4D, 0xB0, 0x68, 0x97, 0xB5, 0x73, 0x42, 0x63, 0xA1, 0x38, 0xF7, 0xBE, 0x50, 
            0xF3, 0xFF, 0x29, 0xE9, 0x5A, 0x0B, 0x88, 0x94, 0x19, 0x39, 0xD2, 0xEE, 0xEF, 0x82, 0xE0, 0x83,
            0xCA, 0xFB, 0x39, 0xD9, 0xFF, 0x2B, 0x1F, 0xC9, 0x24, 0x3F, 0xAB, 0xAE, 0xA7, 0x59, 0x92, 0x58, 
            0x78, 0xB3, 0xB1, 0x52, 0x28, 0xF1, 0x50, 0x4A, 0x49, 0x53, 0x95, 0xDF, 0x0F, 0x2A, 0xF4, 0xAF, 
            0x00, 0x89, 0x6D, 0xA7, 0xEA, 0xA8, 0x97, 0x98, 0x05, 0x35, 0x01, 0xAF, 0xB4, 0x33, 0xF6, 0xCF, 
            0xC7, 0x7F, 0x18, 0xC3, 0x27, 0x3F, 0xC0, 0x36, 0x15, 0xE2, 0x29, 0x31, 0x99, 0x12, 0x44, 0x06
        };
    }
}