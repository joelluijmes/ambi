using System;
using System.Collections.Generic;

namespace ambi_win.driver
{
    internal class Frame
    {
        public Frame(IList<Color> pixels)
        {
            Pixels = pixels ??
            throw new ArgumentNullException(nameof(pixels));
        }

        public IList<Color> Pixels { get; }
    }
}
