﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValveKeyValue;

namespace DSCPSetter.Model
{
    public class VDFModel
    {
        public uint AppID { get; set; }

        public uint InfoState { get; set; }

        public DateTime LastUpdated { get; set; }

        public ulong Token { get; set; }

        public ReadOnlyCollection<byte> Hash { get; set; }
        public ReadOnlyCollection<byte> BinaryDataHash { get; set; }

        public uint ChangeNumber { get; set; }

        public KVObject Data { get; set; }
    }
}