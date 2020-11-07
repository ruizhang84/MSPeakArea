using MSPeakArea.Spectrum;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSPeakArea.Process
{
    public interface IProcess
    {
        ISpectrum Process(ISpectrum spectrum);
    }
}
