﻿using System.Collections.Generic;

namespace DiceParser.DiceDescriptors
{
    public abstract class Roll : Definition
    {
        public int Value { get; protected set; }
        public List<int> Values { get; protected set; } = new List<int>();
    }
}
